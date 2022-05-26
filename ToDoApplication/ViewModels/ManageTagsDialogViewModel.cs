using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ToDoApplication.Command;
using ToDoApplication.Model;
using ToDoApplication.Properties;
using ToDoApplication.Repositories;
using ToDoApplication.Services;

namespace ToDoApplication.ViewModels
{
	class ManageTagsDialogViewModel : ValidationViewModelbase
	{
		private string _tagName;
		public Guid Id { get; set; }

		public string TagName
		{
			get { return _tagName; }
			set 
			{ 
				_tagName = value;
				if (ValidateName())
				{
					AddTagCommand.RaiseCanExecuteChanged();
				}
			}
		}

		private bool ValidateName()
		{
			if (String.IsNullOrWhiteSpace(TagName))
			{
				SetError(nameof(TagName), Resources.TagEmptyError);
				return false;
			}
			else if (NameIsNotunique())
			{
				SetError(nameof(TagName), Resources.TagNotUniqueError);
				return false;
			}
			else
			{
				ResetError(nameof(TagName));
				return true;
			}
		}
		private bool NameIsNotunique()
		{
			var otherTagNames = _tagRepository
				.GetAll()
				.Where(tag => tag.Id != this.Id)
				.Select(tag => tag.Name);
			return otherTagNames.Contains(TagName);
		}

		private TagColor _colors;

		public TagColor Colors
		{
			get { return _colors; }
			set
			{
				_colors = value;
				AddTagCommand.RaiseCanExecuteChanged();
			
			}
		}
		private ToDoItemTagsViewModel _selectedTag;
		private IDialogService _dialogService;
		private readonly ITagRepository _tagRepository;

		public ToDoItemTagsViewModel Selectedtag
		{
			get { return _selectedTag; }
			set 
			{ 
				_selectedTag = value;
				RemoveTagCommand.RaiseCanExecuteChanged();
			}
		}

		public ObservableCollection<ToDoItemTagsViewModel> Tags { get; }
		public ObservableCollection<TagColor> AvailableColours { get; }

		private IEnumerable<Guid> _referencetagId;

		public ActionCommand AddTagCommand { get; }
        
		public ActionCommand RemoveTagCommand { get; }

		public ActionCommand CloseManageTagsDialogCommand { get; }

		public ActionCommand<ToDoItemTagsViewModel> UpdateTagsCommand { get; }
		public ManageTagsDialogViewModel(ObservableCollection<ToDoItemTagsViewModel> tags,
			IEnumerable<Guid> referencetagId,
			IDialogService dialogService,ITagRepository tagRepository)
		{
			Tags = tags;
			_referencetagId = referencetagId;
			AddTagCommand = new ActionCommand(AddTag, CanAddTag);
			RemoveTagCommand = new ActionCommand(RemoveTag, CanRemoveTag);
			CloseManageTagsDialogCommand = new ActionCommand(CloseManageTagsDialog, () => true);
			_dialogService = dialogService;
			_tagRepository = tagRepository;
			UpdateTagsCommand = new ActionCommand<ToDoItemTagsViewModel>(UpdateTags, CanUpdatetags);
			AvailableColours = new ObservableCollection<TagColor>
			{
				TagColor.Default,
				TagColor.Color1,
				TagColor.Color2,
				TagColor.Color3,
				TagColor.Color4
			};

		}

		private bool CanUpdatetags(ToDoItemTagsViewModel arg)
		{
			return true;
		}

		private void UpdateTags(ToDoItemTagsViewModel obj)
		{
			var model = creatItemTagsModel(obj);
			_tagRepository.Update(model);
		}

		private void CloseManageTagsDialog()
		{
			_dialogService.closeManageTagsDialog();
		}

		private bool CanRemoveTag()
		{
			return Selectedtag != null && TagsNotBeingUsed(Selectedtag);
		}

		private bool TagsNotBeingUsed(ToDoItemTagsViewModel tag)
		{
			//return !_toDoItemViewModels.Any(tagid => tagid.Tags.
			//        Select(tagitem => tagitem.Id).Contains(tag.Id));

			return !_referencetagId.Contains(tag.Id);

		}

		private void RemoveTag()
		{
			_tagRepository.Remove(Selectedtag.Id);
			Tags.Remove(Selectedtag);
		}

		private bool CanAddTag()
		{
			//var value = (Color)Colors.SelectedColor;
			return !string.IsNullOrEmpty(TagName);
		}

		private async void AddTag()
		{
			if (!string.IsNullOrEmpty(TagName))
			{
				var tagModel = new ToDoItemTags()
				{
					Id = Guid.NewGuid(),
					Name = TagName,
					Color = Colors,
			};
			Tags.Add(new ToDoItemTagsViewModel(tagModel, _tagRepository));
				try 
				{
					await _tagRepository.Add(tagModel);
				}catch(Exception ex)
				{
					Application.Current.Dispatcher.Invoke(() =>
					{
						throw ex;

					});
				}
			 await _tagRepository.Add(tagModel);
			}
		}

		private ToDoItemTags creatItemTagsModel(ToDoItemTagsViewModel ITVM)
		{
			return new ToDoItemTags()
			{
				Id = ITVM.Id,
				Name = ITVM.Name,
				Color = ITVM.Color
			};
		}
	}

}

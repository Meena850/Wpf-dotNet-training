using log4net;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using ToDoApplication.Command;
using ToDoApplication.Model;
using ToDoApplication.Repositories;
using ToDoApplication.Services;

namespace ToDoApplication.ViewModels
{
	internal class MainWindowViewModel : ViewModelBase
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindowViewModel));
		private string _toListValue;
		private string _toDoDescription;
		private readonly ITodoItemRepository _todoItemRepository;
		private readonly ITagRepository _tagRepository;
		private readonly IDialogService _dialogService;
		private ToDoItemTagsViewModel _selectedtag;
		private ToDoItemViewModel _selectedtoDoItem;

		public ObservableCollection<ToDoItemViewModel> ToListItems { get; private set; }

        private ICollectionView _ToListItemsSort;

        public ICollectionView ToListItemsSort
        {
            get
            {
                return _ToListItemsSort;

            }
            set
            {
                _ToListItemsSort = value;
            }
        }

		public ObservableCollection<ToDoItemTagsViewModel> AvailableTags { get; private set; }
		public AsyncCommand AddToDoCommand { get; }
		public ActionCommand AddtagCommand { get; }
		public ActionCommand ShowManageTagsDialogCommand { get; }

		public ActionCommand SortNameCommand { get; }

		public AsyncCommand<ToDoItemViewModel> RemoveToDoCommand { get;}
		public ActionCommand<ToDoItemViewModel> UpdateToDoCommand { get; }

		public string ToListValue
		{
			get { return _toListValue; }
			set
			{
				_toListValue = value;
				AddToDoCommand.RaiseCanExecuteChanged();
				RaisePropertyChanged(nameof(ToListValue));

			}
		}

		public string ToDoDescription
		{
			get { return _toDoDescription; }
			set
			{
				_toDoDescription = value;
				AddToDoCommand.RaiseCanExecuteChanged();
				RaisePropertyChanged(nameof(ToDoDescription));

			}
		}

		public ToDoItemTagsViewModel selectedtag
		{
			get { return _selectedtag; }
			set { _selectedtag = value; AddtagCommand.RaiseCanExecuteChanged(); }
		}

		public ToDoItemViewModel selectedtoDoItem
		{
			get { return _selectedtoDoItem; }
			set { _selectedtoDoItem = value; AddtagCommand.RaiseCanExecuteChanged(); }
		}

		//ListSortDirection _lastDirection = ListSortDirection.Ascending;
		public ListSortDirection lastDirection;


		public MainWindowViewModel(ITodoItemRepository todoItemRepository,
			                       ITagRepository tagRepository,
								   IDialogService dialogService)
		{
			_todoItemRepository = todoItemRepository;
			_tagRepository = tagRepository;
			_dialogService = dialogService;

			AddToDoCommand = new AsyncCommand(AddToList, ToListValueFocus);
			RemoveToDoCommand = new AsyncCommand<ToDoItemViewModel>(this.RemoveItem, this.selectedItemFocus);
			ShowManageTagsDialogCommand = new ActionCommand(ShowManageTagsDialog, () => true);
			UpdateToDoCommand = new ActionCommand<ToDoItemViewModel>(this.UpdateItem, this.selectedItemFocus);


			AddtagCommand = new ActionCommand(AddTagtoSelectedTodOItem, canAddTag);
			SortNameCommand= new ActionCommand(SortName, () => true);
			//CloseManageTagsDialogCommand = new ActionCommand(CloseManageTagsDialog, () => true);

			
		}

        public override async Task onAttchedasync()
        {
			Log.Debug("Attaching main window viewmodel...");
			AvailableTags = new ObservableCollection<ToDoItemTagsViewModel>();
			var tagsResult = await _tagRepository.GetAll();
			if (!tagsResult.WasSuccessful)
			{
				_dialogService.ShowErrorDialog(tagsResult.Message);
				return;
			}
			foreach (var tag in tagsResult.Value)
			{
				AvailableTags.Add(new ToDoItemTagsViewModel(tag, _tagRepository));
			}
			RaisePropertyChanged(nameof(AvailableTags));
			

			var todoItemsResult = await _todoItemRepository.GetAll();
			if (!todoItemsResult.WasSuccessful)
			{
				_dialogService.ShowErrorDialog(todoItemsResult.Message);
				Log.Error(todoItemsResult.Message);
				return;
			}

			ToListItems = new ObservableCollection<ToDoItemViewModel>();
			foreach (var item in todoItemsResult.Value)
			{
				ToListItems.Add(CreateViewModelFromToDoItem(item));
			}
			RaisePropertyChanged(nameof(ToListItems));

		}

        private void SortName()
        {

            if (lastDirection == ListSortDirection.Ascending)
            {
                ToListItemsSort.SortDescriptions.Clear();
                ToListItemsSort.SortDescriptions.Add(new SortDescription(nameof(ToDoItemViewModel.Name), ListSortDirection.Descending));
                lastDirection = ListSortDirection.Descending;
            }
            else
            {
                ToListItemsSort.SortDescriptions.Clear();
                ToListItemsSort.SortDescriptions.Add(new SortDescription(nameof(ToDoItemViewModel.Name), ListSortDirection.Ascending));
                lastDirection = ListSortDirection.Ascending;
            }
        }

        private bool FilterListItems(object obj)
		{
			return true;
		}

		//AddToDOCommand ICommand 
		public async Task AddToList()
		{
			if (!string.IsNullOrEmpty(ToListValue) && !string.IsNullOrEmpty(ToDoDescription))
			{
				var item = CreatetoDoItemModel(ToListValue, ToDoDescription);
				var model = item.CreateModel();
				ToListItems.Add(item);
				await _todoItemRepository.Add(model);
				//ToListValue = string.Empty;
				//ToDoDescription = string.Empty;

			}
		}
		private bool ToListValueFocus()
		{
			return !string.IsNullOrEmpty(ToListValue) && !string.IsNullOrEmpty(ToDoDescription);
		}

		//RemoveToDoCommand UpdateToDOCommand Icommand
		public async Task RemoveItem(ToDoItemViewModel vmToremove)
		{
			if (vmToremove != null)
			{
				ToListItems.Remove(vmToremove);
				await _todoItemRepository.Remove(vmToremove.Id);
			}
		}

		public void UpdateItem(ToDoItemViewModel vmToremove)
		{
			if (vmToremove != null)
			{
				var model = vmToremove.CreateModel();
				_todoItemRepository.Update(model);
			}
		}

		public bool selectedItemFocus(ToDoItemViewModel vmToremove)
		{
			return vmToremove != null;
		}

		//AddTagCommand ICommand
		private void AddTagtoSelectedTodOItem()
		{
			selectedtoDoItem.Tags.Add(new ToDoItemTagsViewModel(selectedtag.createModel(),_tagRepository));
			_todoItemRepository.Update(selectedtoDoItem.CreateModel());
		}
		private bool canAddTag()
		{
			return selectedtag != null && selectedtoDoItem != null;
		}

		//ShowManageTagsDialogCommand
		private void ShowManageTagsDialog()
		{
			_dialogService.ShowManageTagsDialog(AvailableTags,ToListItems.SelectMany(Item => Item.CreateModel().TagId).Distinct());
		}

		//Close ManageTagsDialogCommand
		private void CloseManageTagsDialog()
		{
			
		}

		//ItemModel
		private ToDoItemViewModel CreateViewModelFromToDoItem(ToDoItemModel item)
		{
			var todoItemViewModel = new ToDoItemViewModel(item, _todoItemRepository);
			var tagviewmodel = item.TagId.Select(tagid => AvailableTags.Single(t => t.Id == tagid));
			todoItemViewModel.Tags = new ObservableCollection<ToDoItemTagsViewModel>(tagviewmodel);
			return todoItemViewModel;
		}

		private ToDoItemViewModel CreatetoDoItemModel(string todoName, string toDoDescription)
		{
			return new ToDoItemViewModel(Guid.NewGuid())
			{
				Name = todoName,
				Timestamp = DateTime.Now,
				IsDone = false,
				//Tags = AvailableTags,
				ToDoDescription = toDoDescription
			};
		}
	}
}

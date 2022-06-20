using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ToDoApplication.Command;
using ToDoApplication.Model;
using ToDoApplication.Properties;
using ToDoApplication.Repositories;
using ToDoApplication.Util;

namespace ToDoApplication.ViewModels
{
	internal class ToDoItemTagsViewModel : ValidationViewModelbase
	{
		
		private ITagRepository _tagRepository;

		public Guid Id { get; set; }
		//public string Name { get; set; }

		public ObservableCollection<Color> AvailableTagColors { get; private set; }

		private Color _colors;

		public Color Colors
		{
			get { return _colors; }
			set
			{
				_colors = value;
				_tagRepository.Update(createModel());
			}
		}


		private string _name;

		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				UpdateIfValidName();
			}
		}

		private void UpdateIfValidName()
		{
			AsyncVoidHelper.TryThrowOnDispatcher(async () =>
			{
				if (await ValidateName())
				{
					await _tagRepository.Update(createModel());
				}

			});
		}


		private async Task<bool> ValidateName()
        {
			if (String.IsNullOrWhiteSpace(Name))
			{
				SetError(nameof(Name), Resources.TagEmptyError);
				return false;
			}
			else if (await NameIsNotunique())
			{
				SetError(nameof(Name), Resources.TagNotUniqueError);
				return false;
			}
			else 
            {
				ResetError(nameof(Name));
				return true;
            }
        }

        private async Task<bool> NameIsNotunique()
        {
			var tagsResult = (await _tagRepository
				.GetAll());
			if (!tagsResult.WasSuccessful)
				return true; // TODO: Show Error dialog
			var otherTagNames = tagsResult.Value
		   .Where(tag => tag.Id != this.Id)
		   .Select(tag => tag.Name);
			return otherTagNames.Contains(Name);
		}

        private TagColor _color;

		public TagColor Color
		{
			get { return _color; }
			set
			{
				_color = value;
				_tagRepository.Update(createModel());
			}
		}

        public  ToDoItemTagsViewModel (ToDoItemTags tags,ITagRepository tagRepository)
		{
		
			Id = tags.Id;
			_name = tags.Name;
			_color = tags.Color;
			_tagRepository = tagRepository;
		}

		public ToDoItemTags createModel()
		{
			return new ToDoItemTags
			{
				Id = Id,
				Name = Name,
				Color = Color
			};
		}

	
	}
}

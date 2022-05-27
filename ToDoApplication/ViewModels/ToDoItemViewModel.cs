using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ToDoApplication.Command;
using ToDoApplication.Model;
using ToDoApplication.Repositories;

namespace ToDoApplication.ViewModels
{
	internal class ToDoItemViewModel 
    {
		private readonly ITodoItemRepository _repository;
		public Guid Id { get; }
        public string Name { get; set; }
		public string ToDoDescription { get; set; }
		public DateTime Timestamp { get; set; }

		public bool IsDone { get; set; }

		public ObservableCollection<ToDoItemTagsViewModel> Tags { get; set; }

		public AsyncCommand<ToDoItemTagsViewModel> RemoveTagCommand { get; }

		public ToDoItemViewModel(ToDoItemModel item,ITodoItemRepository repository) : this(item.Id)
		{
			Name = item.Name;
			Timestamp = item.Timestamp;
			IsDone = item.IsDone;
			ToDoDescription = item.ToDoDescription;
			_repository = repository;
			RemoveTagCommand = new AsyncCommand<ToDoItemTagsViewModel>(RemoveTag, CanRemoveTag);
		}

		private bool CanRemoveTag(ToDoItemTagsViewModel _)
		{
			return true;
		}

		public ToDoItemViewModel(Guid id)
		{
			Id = id;
			Tags = new ObservableCollection<ToDoItemTagsViewModel>();
		}

		public async Task RemoveTag(ToDoItemTagsViewModel tagRemove)
		{
			Tags.Remove(tagRemove);
			await update();
		}
		private async Task update()
		{
			var model = CreateModel();
			await _repository.Update(model);
		}

		public ToDoItemModel CreateModel()
		{
			return new ToDoItemModel()
			{
				Name = Name,
				Timestamp = Timestamp,
				IsDone = IsDone,
				Id = Id,
				TagId = Tags.Select(vm => vm.Id).ToList(),
				//GetTagId(item.Tags),
				ToDoDescription = ToDoDescription
			};
		}
	}
}

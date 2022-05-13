using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.Model;
using ToDoApplication.Services;

namespace ToDoApplication.Repositories
{
	class TodoItemRepository : ITodoItemRepository
	{
		const string _directoryPath = @"C:\Krones_CRD\TestFiles";
		const string _fileName = "ToDoItem.json";
		private readonly IAppConfigService _configService;

        public TodoItemRepository(IAppConfigService configService)
        {
			_configService = configService;
				
        }
		public List<ToDoItemModel> GetAll()
		{
			if (_configService.TodoItemFile.Exists)
			{
				string json = File.ReadAllText(Path.Combine(_configService.TodoItemFile.FullName));
				if (!string.IsNullOrEmpty(json))
					 return JsonConvert.DeserializeObject<List<ToDoItemModel>>(json);
			}
			return new List<ToDoItemModel>();
		}
		
		public void Remove(Guid id)
		{
			List<ToDoItemModel> removeFromItems = GetAll();
			//removeFromItems.RemoveAll(x => x.Name == item.Name);
			ToDoItemModel Itemtoremove = removeFromItems.First(item => item.Id == id);
			if (Itemtoremove != null)
			{
				removeFromItems.Remove(Itemtoremove);
				saveItems(removeFromItems);
			}
		}

		public void Add(ToDoItemModel item)
		{
			List<ToDoItemModel> addToItems = GetAll();
			addToItems.Add(item);
			saveItems(addToItems);
		}
		 
		private void saveItems(List<ToDoItemModel> items)
		{
			var todoItemsFile = _configService.TodoItemFile;
			if (!todoItemsFile.Directory.Exists)
				todoItemsFile.Directory.Create();

			if (items.Count > 0)
			{
				File.WriteAllText(todoItemsFile.FullName, 
					JsonConvert.SerializeObject(items, Formatting.Indented));
			}
			else
			{
				File.Delete(todoItemsFile.FullName);
			}
		}

		public void Update(ToDoItemModel todoitem)
		{
			List<ToDoItemModel> updateinItems = GetAll();
			//var itemtoUpdate=updateinItems.Where(w => w.Id == item.Id).ToList().ForEach
			//	(i => 
			//	i.IsDone = item.IsDone);
			var itemToUpdate = updateinItems.First(items => items.Id == todoitem.Id);
			itemToUpdate.IsDone = todoitem.IsDone;
			itemToUpdate.Name = todoitem.Name;
			itemToUpdate.TagId = todoitem.TagId;
			itemToUpdate.ToDoDescription = todoitem.ToDoDescription;
			itemToUpdate.Timestamp = todoitem.Timestamp;
			saveItems(updateinItems);

		}
	}
}

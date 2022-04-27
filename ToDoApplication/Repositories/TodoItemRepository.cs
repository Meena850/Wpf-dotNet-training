using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.Model;

namespace ToDoApplication.Repositories
{
	class TodoItemRepository : ITodoItemRepository
	{
		private string _directoryPath = @"C:\Krones_CRD\TestFiles";
		private string _fileName = "ToDoItem.json";
		public List<ToDoItemModel> GetAll()
		{
			if (File.Exists(Path.Combine(_directoryPath, _fileName)))
			{
				string json = File.ReadAllText(Path.Combine(_directoryPath, _fileName));
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
			if (!new DirectoryInfo(_directoryPath).Exists)
				Directory.CreateDirectory(_directoryPath);


			if (items.Count > 0)
			{
				File.WriteAllText(Path.Combine(_directoryPath, _fileName), JsonConvert.SerializeObject(items, Formatting.Indented));
			}
			else
			{
				File.Delete(Path.Combine(_directoryPath, _fileName));
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

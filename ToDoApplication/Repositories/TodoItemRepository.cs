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
		public async Task<List<ToDoItemModel>> GetAll()
		{
			if (_configService.TodoItemFile.Exists)
			{
				string json = await ReadFileAsync(_configService.TodoItemFile.FullName);
				if (!string.IsNullOrEmpty(json))
					 return JsonConvert.DeserializeObject<List<ToDoItemModel>>(json);
			}
			return new List<ToDoItemModel>();
		}
		
		public async Task Remove(Guid id)
		{
			List<ToDoItemModel> removeFromItems = await GetAll();
			//removeFromItems.RemoveAll(x => x.Name == item.Name);
			ToDoItemModel Itemtoremove = removeFromItems.First(item => item.Id == id);
			if (Itemtoremove != null)
			{
				removeFromItems.Remove(Itemtoremove);
				await saveItems(removeFromItems);
			}
		}

		public async Task Add(ToDoItemModel item)
		{
			List<ToDoItemModel> addToItems = await GetAll();
			addToItems.Add(item);
			await saveItems (addToItems);
		}
		 
		private async Task saveItems(List<ToDoItemModel> items)
		{
			var todoItemsFile = _configService.TodoItemFile;
			if (!todoItemsFile.Directory.Exists)
				todoItemsFile.Directory.Create();
			var todoItemString = JsonConvert.SerializeObject(items, Formatting.Indented);

			if (items.Count > 0)
			{
				await WriteFileAsync(todoItemsFile.FullName, todoItemString);
			}
			else
			{
				File.Delete(todoItemsFile.FullName);
			}
		}

		public async Task Update(ToDoItemModel todoitem)
		{
			List<ToDoItemModel> updateinItems = await GetAll();
			//var itemtoUpdate=updateinItems.Where(w => w.Id == item.Id).ToList().ForEach
			//	(i => 
			//	i.IsDone = item.IsDone);
			var itemToUpdate = updateinItems.First(items => items.Id == todoitem.Id);
			itemToUpdate.IsDone = todoitem.IsDone;
			itemToUpdate.Name = todoitem.Name;
			itemToUpdate.TagId = todoitem.TagId;
			itemToUpdate.ToDoDescription = todoitem.ToDoDescription;
			itemToUpdate.Timestamp = todoitem.Timestamp;
			await saveItems(updateinItems);

		}



		private async Task<string> ReadFileAsync(string filename)
		{
			using (var streamReader = new StreamReader(File.OpenRead(filename)))
			{
				return await streamReader.ReadToEndAsync();
			}
		}

		private async Task WriteFileAsync(string fileName, string content)
		{
			using (var memStream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
			{
				using (var fileStream = File.OpenWrite(fileName))
				{
					await memStream.CopyToAsync(fileStream);
				}
			}

		}
	}
}

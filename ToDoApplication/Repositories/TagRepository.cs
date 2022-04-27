using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ToDoApplication.Model;

namespace ToDoApplication.Repositories
{
	class TagRepository : ITagRepository
	{
	
		private string _directoryPath = @"C:\Krones_CRD\TestFiles";
		private string _fileName = "TagItem.json";

		public TagRepository()
		{

		}
		public void Add(ToDoItemTags tag)
		{
			var tags = GetAll();
			tags.Add(tag);
			saveItems(tags);
		}

		public List<ToDoItemTags> GetAll()
		{
			if (File.Exists(Path.Combine(_directoryPath, _fileName)))
			{
				string json = File.ReadAllText(Path.Combine(_directoryPath, _fileName));
				if (!string.IsNullOrEmpty(json))
					return JsonConvert.DeserializeObject<List<ToDoItemTags>>(json);
			}
			return new List<ToDoItemTags>();
		}

		public void Remove(Guid tagId)
		{
			var tags = GetAll();
			var tagtoremove = tags.Single(tag => tag.Id == tagId);
			tags.Remove(tagtoremove);
			saveItems(tags);
		}

		public void Update(ToDoItemTags tagItem)
		{
			List<ToDoItemTags> updateinItems = GetAll();
			var itemToUpdate = updateinItems.Single(items => items.Id == tagItem.Id);
			//itemToUpdate.Id = tagItem.Id;
			itemToUpdate.Name = tagItem.Name;
			itemToUpdate.Color = tagItem.Color;
			saveItems(updateinItems);
		}
		private void saveItems(List<ToDoItemTags> items)
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
	}
}

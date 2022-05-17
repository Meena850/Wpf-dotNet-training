using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ToDoApplication.Model;
using ToDoApplication.Services;

namespace ToDoApplication.Repositories
{
	class TagRepository : ITagRepository
	{
		private readonly IAppConfigService _configService;
		public TagRepository(IAppConfigService configService)
		{
			_configService = configService;
		}
		public void Add(ToDoItemTags tag)
		{
			var tags = GetAll();
			tags.Add(tag);
			saveItems(tags);
		}

		public List<ToDoItemTags> GetAll()
		{
			if (_configService.TagItemFile.Exists)
			{
				string json = File.ReadAllText(_configService.TagItemFile.FullName);
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
			var tagItemsFile = _configService.TagItemFile;
			if (!tagItemsFile.Directory.Exists)
				tagItemsFile.Directory.Create();

			if (items.Count > 0)
			{
				File.WriteAllText(_configService.TagItemFile.FullName, JsonConvert.SerializeObject(items, Formatting.Indented));
			}
			else
			{
				File.Delete(Path.Combine(tagItemsFile.FullName));
			}

		}
	}
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		public async Task Add(ToDoItemTags tag)
		{
			var tags = await GetAll();
			tags.Add(tag);
			await saveItems (tags);
		}

		public async Task<List<ToDoItemTags>> GetAll()
		{
			if (_configService.TagItemFile.Exists)
			{
				string json = await ReadFileAsync(_configService.TagItemFile.FullName);
				if (!string.IsNullOrEmpty(json))
					return JsonConvert.DeserializeObject<List<ToDoItemTags>>(json);
			}
			return new List<ToDoItemTags>();
		}

		public async Task Remove(Guid tagId)
		{
			var tags = await GetAll();
			var tagtoremove = tags.Single(tag => tag.Id == tagId);
			tags.Remove(tagtoremove);
			await saveItems(tags);
		}

		public async Task Update(ToDoItemTags tagItem)
		{
			List<ToDoItemTags> updateinItems = await GetAll();
			var itemToUpdate = updateinItems.Single(items => items.Id == tagItem.Id);
			//itemToUpdate.Id = tagItem.Id;
			itemToUpdate.Name = tagItem.Name;
			itemToUpdate.Color = tagItem.Color;
			await saveItems(updateinItems);
		}
		private async Task saveItems(List<ToDoItemTags> items)
		{
			var tagItemsFile = _configService.TagItemFile;
			if (!tagItemsFile.Directory.Exists)
				tagItemsFile.Directory.Create();
			var tagItemString = JsonConvert.SerializeObject(items, Formatting.Indented);

			if (items.Count > 0)
			{
				await WriteFileAsync(_configService.TagItemFile.FullName, tagItemString);
			}
			else
			{
				File.Delete(Path.Combine(tagItemsFile.FullName));
			}

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

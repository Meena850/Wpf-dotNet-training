using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.Model;
using ToDoApplication.Services;
using ToDoApplication.Util;

namespace ToDoApplication.Repositories
{
	class TagRepository : ITagRepository
	{
		private readonly IAppConfigService _configService;
		public TagRepository(IAppConfigService configService)
		{
			_configService = configService;
		}
		public async Task<Result<Unit>> Add(ToDoItemTags tag)
		{
			var tagsResult = await GetAll();
			if (tagsResult.WasSuccessful)
			{
				var tags = tagsResult.Value;
				tags.Add(tag);
				await saveItems(tags);
				return Result.CreateSuccess();
			}
			else
			{
				return tagsResult.Map(_ => Unit.Default);
			}
		}

		public async Task<Result<List<ToDoItemTags>>> GetAll()
		{
			var tagFile = _configService.TagItemFile;
			if (tagFile.Exists)
			{
				try 
				{
					var tagItemStringresult = await FileHelper.ReadFileAsync(tagFile.FullName);
					return tagItemStringresult.Map(JsonConvert.DeserializeObject<List<ToDoItemTags>>);
				}
				catch(Exception ex)
                {
					return new Error<List<ToDoItemTags>>($"Failed to load tag list from file: {ex.Message}");
				}
			}
			else
			{
				return Result.CreateSuccess(new List<ToDoItemTags>());
			}
		}

		public async Task<Result<Unit>> Remove(Guid tagId)
		{
			var tagsResult = await GetAll();
			if (tagsResult.WasSuccessful)
			{
				var tags = tagsResult.Value;
				var tagtoremove = tags.Single(tag => tag.Id == tagId);
				tags.Remove(tagtoremove);
				await saveItems(tags);
				return Result.CreateSuccess();
			}
			else
            {
				return tagsResult.Map(_ => Unit.Default);
            }
		}

		public async Task<Result<Unit>> Update(ToDoItemTags tagItem)
		{
			var tagsResult = await GetAll();
			if (tagsResult.WasSuccessful)
			{

				var tags = tagsResult.Value;
				var itemToUpdate = tags.Single(items => items.Id == tagItem.Id);
				//itemToUpdate.Id = tagItem.Id;
				itemToUpdate.Name = tagItem.Name;
				itemToUpdate.Color = tagItem.Color;
				await saveItems(tags);
				return Result.CreateSuccess();


			} 
			else
			{
				return tagsResult.Map(_ => Unit.Default);

			}
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
		private async Task WriteFileAsync(string fileName, string content)
        {
			using (var memStream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
				using (var fileStream = File.Open(fileName, FileMode.Create))
                {
					await memStream.CopyToAsync(fileStream);
                }
            }

        }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.Model;
using ToDoApplication.Repositories;

namespace ToDoApplication.UnitTest.Fake
{
	class FakeTagRepository : ITagRepository
	{
		public List<ToDoItemTags> _tags = new List<ToDoItemTags>();
		public void Add(ToDoItemTags tag)
		{
			_tags.Add(tag);
		}

		public List<ToDoItemTags> GetAll()
		{
			return _tags;
		}

		public void Remove(Guid tagId)
		{
			var tagtoremove = _tags.Single(tag => tag.Id == tagId);
			_tags.Remove(tagtoremove);
		}

		public void Update(ToDoItemTags item)
		{
			throw new NotImplementedException();
		}
	}
}

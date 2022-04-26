using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.Model;
using ToDoApplication.Repositories;

namespace ToDoApplication.UnitTest.Fake
{
	class FakeTodoItemRepository : ITodoItemRepository
	{
		public void Add(ToDoItemModel item)
		{
			
		}

		public List<ToDoItemModel> GetAll()
		{
			return new List<ToDoItemModel>();
		}

		public void Remove(Guid id)
		{
			
		}

		public void Update(ToDoItemModel item)
		{
			
		}
	}
}

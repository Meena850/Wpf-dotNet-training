using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.Model;

namespace ToDoApplication.Repositories
{
	internal  interface ITodoItemRepository
	{
		List<ToDoItemModel> GetAll();
		void Add(ToDoItemModel item);
		void Remove(Guid id);
		void Update(ToDoItemModel item);

	}
}

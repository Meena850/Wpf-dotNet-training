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
		Task<List<ToDoItemModel>> GetAll();
		Task Add(ToDoItemModel item);
		Task Remove(Guid id);
		Task Update(ToDoItemModel item);

	}
}

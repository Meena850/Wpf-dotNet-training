using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.CustomControls;
using ToDoApplication.Model;

namespace ToDoApplication.Repositories
{
	internal interface ITagRepository
	{
		Task<List<ToDoItemTags>> GetAll();
		Task Add(ToDoItemTags tag);

		Task Remove(Guid tagId);

		Task Update(ToDoItemTags item);
	}
}

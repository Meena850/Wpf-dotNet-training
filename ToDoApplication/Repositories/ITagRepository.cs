using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.CustomControls;
using ToDoApplication.Model;
using ToDoApplication.Util;

namespace ToDoApplication.Repositories
{
	internal interface ITagRepository
	{
		Task<Result<List<ToDoItemTags>>> GetAll();
		Task<Result<Unit>> Add(ToDoItemTags tag);

		Task<Result<Unit>> Remove(Guid tagId);

		Task<Result<Unit>> Update(ToDoItemTags item);
	}
}

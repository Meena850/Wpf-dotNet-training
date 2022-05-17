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
		List<ToDoItemTags> GetAll();
		void Add(ToDoItemTags tag);

		void Remove(Guid tagId);

		void Update(ToDoItemTags item);
	}
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.Model;
using ToDoApplication.Services;

namespace ToDoApplication.Repositories
{
	internal class TodoItemRepository :FileBasedRepository<ToDoItemModel>, ITodoItemRepository
	{
		public TodoItemRepository(IAppConfigService configservice) : base(configservice.TodoItemFile)
		{ 
		}
	}
}

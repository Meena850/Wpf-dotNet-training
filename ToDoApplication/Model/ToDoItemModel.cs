using System;
using System.Collections.Generic;

namespace ToDoApplication.Model
{
    internal class ToDoItemModel : BaseEntity
    {
       public string Name { get; set; }

		public string ToDoDescription { get; set; }
		public DateTime Timestamp { get; set; }

		public bool IsDone { get; set; }

		public List<Guid> TagId { get; set; } = new List<Guid>();
	}
}

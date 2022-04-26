using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApplication.Model
{
	internal class ToDoItemModel 
    {
		public Guid Id { get; set; }
       public string Name { get; set; }

		public string ToDoDescription { get; set; }
		public DateTime Timestamp { get; set; }

		public bool IsDone { get; set; }

		public List<Guid> TagId { get; set; } = new List<Guid>();

		//public List<ToDoItemTags> Tags { get; set; }

		//: IEquatable<ToDoItemModel>
		//public bool Equals(ToDoItemModel item)
		//{
		//	return
		//			 Name == item.Name &&
		//			 Timestamp == item.Timestamp &&
		//			 IsDone == item.IsDone;
		//}


		//public override bool Equals(object obj)
		//{
		//	if (obj is ToDoItemModel item)
		//	{
		//		return
		//			 Name == item.Name &&
		//			 Timestamp == item.Timestamp &&
		//			 IsDone == item.IsDone;
		//	}
		//	else
		//	{
		//		return false;
		//	}
		//}

		//public override int GetHashCode()
		//{
		//	return Name.GetHashCode() + Timestamp.GetHashCode() + IsDone.GetHashCode();
		//}

	}
}

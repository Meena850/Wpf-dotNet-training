using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ToDoApplication.Model
{
	internal class ToDoItemTags
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public TagColor Color { get; set; }
	}
}

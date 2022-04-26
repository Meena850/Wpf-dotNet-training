using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApplication.Exceptions
{
	public class MissingTemplateException :Exception
	{
		public MissingTemplateException( string message) :base(message)
		{


		}
	}
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.Converters;

namespace ToDoApplication.UnitTest.Converters
{
	[TestClass]
	public class upercaseConverterTest
	{
		[TestMethod]
		public void Convert_isStringvalueisLowerCase_ConvertToUppercase()
		{
			//Act
			var converter = createSut();
			//Assert
			var returnvalue = converter.Convert("CheckValue", typeof(string), null, CultureInfo.CurrentCulture);
			//Arrange
			returnvalue.ShouldBeEquivalentTo("CHECKVALUE");
		}

		private UpercaseConverter createSut()
		{
			return new UpercaseConverter();
		}
	}
}

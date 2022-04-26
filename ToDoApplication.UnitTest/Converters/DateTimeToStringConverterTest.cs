using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.Converters;

namespace ToDoApplication.UnitTest.Converters
{
	[TestClass]
	public class DateTimeToStringConverterTest
	{
		[TestMethod]
		public void Convert_ConverterDateandtimeValue_ExpectedTimeAsResult()
		{
			//Act
			var converter = createSut();
			DateTime CurrentTime = DateTime.Now;
			//Assert
			var resultTime= CurrentTime.ToString("hh:mm:ss tt");
			var returnvalue =converter.Convert(CurrentTime, typeof(string), null, null);
			//Arrange
			returnvalue.ShouldBeEquivalentTo(resultTime);
		}

		[TestMethod]
		public void Convert_ConverterDateandtimeValue_ExpectedUnknownResult()
		{
			//Act
			var converter = createSut();
			string CurrentTime = Convert.ToString(DateTime.Now);
			//Assert
			var returnvalue = converter.Convert(CurrentTime, typeof(string), null, null);
			//Arrange
			returnvalue.ShouldBeEquivalentTo("unknown");
		}

		private DateTimeToStringConverter createSut()
		{
			return new DateTimeToStringConverter();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using ToDoApplication.Model;

namespace ToDoApplication.Converters
{
	internal class ForegroundColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is TagColor tagcolor)
			{
				switch (tagcolor)
				{
					case TagColor.Color1:
						return GetBrush("ForeGroundColor1");
					case TagColor.Color2:
						return GetBrush("ForeGroundColor2");
					case TagColor.Color3:
						return GetBrush("ForeGroundColor3");
					case TagColor.Color4:
						return GetBrush("ForeGroundColor4");
				}
			};
			return GetBrush("ForeGroundColorDefault"); ;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		private SolidColorBrush GetBrush(string resourcekey)
		{
			var brush = Application.Current.Resources[resourcekey];
			return brush as SolidColorBrush;
		}
	}
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToDoApplication.UserControls
{
	/// <summary>
	/// Interaction logic for ToDoItemTag.xaml
	/// </summary>
	public partial class ToDoItemTag : UserControl
	{
		public static readonly DependencyProperty TagTextProperty =
			DependencyProperty.Register("TagText", typeof(string), typeof(ToDoItemTag),
				new PropertyMetadata("---default---", (obj, args) => ((ToDoItemTag)obj).TagTextChangedMethod()));

		public string TagText
		{
			get
			{
				return (string)GetValue(TagTextProperty);
			}
			set
			{
				SetValue(TagTextProperty, value);
			}
		}


	


		private void TagTextChangedMethod()
		{
			if( !string.IsNullOrEmpty(TagText))
			{
				TagTextBlock.Text = TagText.ToUpper();
			}
	
		}


		public ToDoItemTag()
		{
			InitializeComponent();
		}



	



	}
}

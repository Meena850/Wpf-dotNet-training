using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ToDoApplication.Exceptions;

namespace ToDoApplication.CustomControls
{
	[TemplatePart(Name = TagTextBlockName, Type =typeof(TextBlock))]
	internal class ToDoItemTag : Control
	{
		private const string TagTextBlockName = "CustomTagTextBlock";
		private TextBlock _tagTextBlock;
		public string TagText
		{
			get { return (string)GetValue(TagTextProperty); }
			set { SetValue(TagTextProperty, value); }
		}

		// Using a DependencyProperty as the backing store for TagText.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TagTextProperty =
			DependencyProperty.Register("TagText", typeof(string), typeof(ToDoItemTag),
				new PropertyMetadata("-default-",OnTagTextChanged));


		public ICommand RemoveTagCommand
		{
			get { return (ICommand)GetValue(RemoveTagCommandProperty); }
			set { SetValue(RemoveTagCommandProperty, value); }
		}

         public static readonly DependencyProperty RemoveTagCommandProperty =
			DependencyProperty.Register("RemoveTagCommand", typeof(ICommand), typeof(ToDoItemTag));


		public object RemoveTagCommandParameter
		{
			get { return (int)GetValue(RemoveTagCommandParameterProperty); }
			set { SetValue(RemoveTagCommandParameterProperty, value); }
		}

		// Using a DependencyProperty as the backing store for RemoveTagCommandParameter.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty RemoveTagCommandParameterProperty =
			DependencyProperty.Register("RemoveTagCommandParameter", typeof(object), typeof(ToDoItemTag), 
				new PropertyMetadata(null));



		private static void OnTagTextChanged(DependencyObject toDoItemtag, DependencyPropertyChangedEventArgs e)
		{
			((ToDoItemTag)toDoItemtag).OnTagTextChanged();

		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_tagTextBlock = Template.FindName(TagTextBlockName, this) as TextBlock;
			if(_tagTextBlock is null)
			{
				throw new MissingTemplateException($"could not find the element with the name {TagTextBlockName}");
			}
			OnTagTextChanged();
		}
		private void OnTagTextChanged()
		{
			if (_tagTextBlock != null)
			{
			
				_tagTextBlock.Text = TagText.ToUpper();
			}
		}

		static ToDoItemTag()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ToDoItemTag),
				new FrameworkPropertyMetadata(typeof(ToDoItemTag)));
		}
	}

	
}
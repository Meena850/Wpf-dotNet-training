using System.Windows;
using ToDoApplication.Util;
using ToDoApplication.ViewModels;

namespace ToDoApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
	{
		public MainWindow()
		{
			Title = "To Do HMI";
			InitializeComponent();
			Loaded += OnLoaded;
		}

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
			if (DataContext is ViewModelBase vm)
			{
				AsyncVoidHelper.TryThrowOnDispatcher(vm.onAttchedasync);
			}
        }
    }
}

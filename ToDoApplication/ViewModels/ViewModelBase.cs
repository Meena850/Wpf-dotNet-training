using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ToDoApplication.ViewModels
{
  internal abstract	class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName)
		{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public static bool IsRunningInDesigner()
        {
			return (bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue);
        }

		public virtual Task onAttchedasync()
		{
			return Task.CompletedTask;
		}
	}
}

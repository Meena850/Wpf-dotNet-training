using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using ToDoApplication.ViewModels;

namespace ToDoApplication.MarkUpExtension
{
    internal class ViewModelProviderExtension : MarkupExtension
    {
        public Type ViewModeltype { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if(ViewModeltype == null)
            {
                return null;
            }
            else if(ViewModelBase.IsRunningInDesigner())
            {
                return null;
            }
            else
            {
                return Ioc.IocContainer.current.Resolve(ViewModeltype);
            }
        }
    }
}

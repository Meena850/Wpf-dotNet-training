using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.Repositories;
using ToDoApplication.Services;
using ToDoApplication.ViewModels;

namespace ToDoApplication.Ioc
{
	internal class IocContainer
	{
		 public static IContainer current { get; }

		static IocContainer()
		{
			var builder = new ContainerBuilder();

			//repositeries

			//builder.RegisterType<TagRepository>().As<ITagRepository>().InstancePerLifetimeScope();

			builder.RegisterType<TagRepository>().As<ITagRepository>().SingleInstance();
			builder.RegisterType<TodoItemRepository>().As<ITodoItemRepository>();

			//services

			builder.RegisterType<DialogService>().As<IDialogService>();

			builder.RegisterType<LoggingService>().As<ILoggingService>();
			builder.RegisterType<AppConfigService>().As<IAppConfigService>().SingleInstance();

			//Viewmodels

			builder.RegisterType<MainWindowViewModel>();

			current = builder.Build();

			//var scope = current.BeginLifetimeScope("Test");
			//scope.Resolve<>

		}
	}
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToDoApplication.Dialogs;
using ToDoApplication.Repositories;
using ToDoApplication.ViewModels;

namespace ToDoApplication.Services
{
	internal class DialogService : IDialogService
	{
		private ITagRepository _tagRepository;
		public DialogService(ITagRepository tagRepository)
		{
			_tagRepository = tagRepository;
		}
		public void ShowManageTagsDialog(ObservableCollection<ToDoItemTagsViewModel> tags,IEnumerable<Guid> refernceIds) 
		{
			var managetagsDialog = new ManageTagsDialog();
			managetagsDialog.DataContext = new ManageTagsDialogViewModel(tags, refernceIds,this, _tagRepository);
			setDialogHostContent(managetagsDialog);
		}

		public  void closeManageTagsDialog()
		{
			setDialogHostContent(null);
		}

		private static void setDialogHostContent(object content)
		{
			var mainWindow = Application.Current.MainWindow as MainWindow;
			mainWindow.DialogHost.Content = content;

		}

	}
}

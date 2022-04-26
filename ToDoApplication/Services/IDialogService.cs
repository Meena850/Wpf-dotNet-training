using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.ViewModels;

namespace ToDoApplication.Services
{
	interface IDialogService
	{
		void ShowManageTagsDialog(ObservableCollection<ToDoItemTagsViewModel> tags, IEnumerable<Guid> refernceIds);

		void closeManageTagsDialog();
	}
}

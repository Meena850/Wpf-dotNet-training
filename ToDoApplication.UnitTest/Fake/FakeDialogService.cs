using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.Services;
using ToDoApplication.ViewModels;

namespace ToDoApplication.UnitTest.Fake
{
	class FakeDialogService : IDialogService
	{
		public void closeManageTagsDialog()
		{
			
		}

		public void ShowManageTagsDialog(ObservableCollection<ToDoItemTagsViewModel> tags, IEnumerable<Guid> refernceIds)
		{
			
		}
	}
}

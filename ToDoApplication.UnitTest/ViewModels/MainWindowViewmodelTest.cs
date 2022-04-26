using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoApplication.ViewModels;
using ToDoApplication.Model;
using System;
using Shouldly;
using System.Text.Json;
using ToDoApplication.UnitTest.Fake;
using System.Linq;
using ToDoApplication.Repositories;
using Moq;
using System.Collections.Generic;
using ToDoApplication.Services;

namespace ToDoApplication.UnitTest.ViewModels
{
	[TestClass]
	public class MainWindowViewmodelTest
	{
		[TestMethod]
		public void AddToList_ToListValueEmpty_ToListItemNotAdded()
		{
			//Arrange
			var mainWindowVar = createSut();
			mainWindowVar.ToListValue = string.Empty;
			mainWindowVar.ToListItems.Clear();
			//Act
			var result = mainWindowVar.AddToDoCommand.CanExecute(null);
			//Assert
			result.ShouldBeFalse();
		}

		[TestMethod]
		public void AddToList_ToListValueNotEmpty_ToListItemNotAdded()
		{
			//Arrange
			var mainWindowVar = createSut();
			mainWindowVar.ToListValue = "NotEmpty";
			mainWindowVar.ToDoDescription = "NotEmpty";
			//Act
			var result = mainWindowVar.AddToDoCommand.CanExecute(null);
			//Assert
			result.ShouldBeTrue();
		}

		[TestMethod]
		public void RemoveItem_NoToListItemSelected_ToListItemCannotRemove()
		{
			var mainWindowVar = createSut();
			//Act
			var canRemoveListItem = mainWindowVar.RemoveToDoCommand.CanExecute(null);

			//Assert
			canRemoveListItem.ShouldBeFalse();
		}

		[TestMethod]
		public void RemoveItem_ToDoItemSelected_ToListItemCanBeRemoved()
		{
			//Arrange
			var mainWindowVar = createSut();
			var Item = CreatetoDoItemModel("Selected Item");
			//Act
			var canRemoveListItem = mainWindowVar.RemoveToDoCommand.CanExecute(Item);
			//Assert
			canRemoveListItem.ShouldBeTrue();
		}


		[TestMethod]
		public void AddToList_ToListValue_ToListItemsExists()
		{
			//Arrange
			var mainWindowVar = createSut();
			mainWindowVar.ToListValue = "MyValue";
			mainWindowVar.ToDoDescription = "Onathor Value";
			//Act
			mainWindowVar.AddToDoCommand.Execute(null);
			//Assert
			mainWindowVar.ToListItems.ShouldContain(VM=> VM.Name=="MyValue");
		}

		[TestMethod]
		public void RemoveItem_ToDoItemSelected_ToDoListItemRemoved()
		{
			//Arrange
			var mainWindowVar = createSut();
			var toDoItem = "MyValue";
			var todoItemCreate = CreatetoDoItemModel(toDoItem);
			mainWindowVar.ToListItems.Add(todoItemCreate);
			
			//Act
			mainWindowVar.RemoveToDoCommand.Execute(todoItemCreate);

			//Assert
			mainWindowVar.ToListItems.ShouldNotContain(todoItemCreate);
		}

		[TestMethod]
		public void AddTag_selectedItemIsNull_TagCannotBeadded()
		{
			//Arrange
			var mainWindowVar = createSut();

			//Act
			mainWindowVar.selectedtag = createToDoItemTagViewModel("value");
			mainWindowVar.selectedtoDoItem = null;
			var canAddTag = mainWindowVar.AddtagCommand.CanExecute(null);
			//Assert
			canAddTag.ShouldBeFalse();
		}

		[TestMethod]
		public void AddTag_ToDoItemSelectedAndTagSelected_toDoItemAdded()
		{
			//Arrange
			var mainWindowVar = createSut();
			var ToDoItem = CreatetoDoItemModel("Some Item");
			//Act
			mainWindowVar.selectedtag = createToDoItemTagViewModel("Some Tag");
			mainWindowVar.selectedtoDoItem = ToDoItem;
		    mainWindowVar.AddtagCommand.Execute(null);
			//Assert
			ToDoItem.Tags.Single().Name.ShouldBe("Some Tag");
		}


		private MainWindowViewModel createSut()
		{
			var todoItemrepository = new Mock<ITodoItemRepository>();
			todoItemrepository.Setup(repo => repo.GetAll()).Returns(new List<ToDoItemModel>());

			var tagRepository = new Mock<ITagRepository>();
			tagRepository.Setup(repo => repo.GetAll()).Returns(new List<ToDoItemTags>());

			return new MainWindowViewModel(
				todoItemrepository.Object,
				tagRepository.Object,
				Mock.Of<IDialogService>(),null);

		}

		private ToDoItemViewModel CreatetoDoItemModel(string todoName)
		{
			return new ToDoItemViewModel(Guid.NewGuid())
			{

				Name = todoName,
				Timestamp = DateTime.Now,
				IsDone=false
			};
		}

		private ToDoItemTagsViewModel createToDoItemTagViewModel(string tagname)
		{
			return new ToDoItemTagsViewModel(new Model.ToDoItemTags()
			{
				Id = Guid.NewGuid(),
				Name = tagname
			},Mock.Of<ITagRepository>());
		}

	}
}

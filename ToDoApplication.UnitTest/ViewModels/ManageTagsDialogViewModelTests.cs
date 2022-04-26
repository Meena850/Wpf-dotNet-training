using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.Model;
using ToDoApplication.Repositories;
using ToDoApplication.Services;
using ToDoApplication.UnitTest.Fake;
using ToDoApplication.ViewModels;

namespace ToDoApplication.UnitTest.ViewModels
{
	[TestClass]
	public class ManageTagsDialogViewModelTests
	{
		[TestMethod]
		public void AddTag_tagNameISEmpty_TagcannotbeAdded()
		{

			//Arrange
			var viewmodel = createSut();
			viewmodel.TagName = "";
			//Act
			//Assert
			viewmodel.AddTagCommand.CanExecute(null).ShouldBeFalse();
		}
		[TestMethod]
		public void AddTag_tagNameIsSet_TagCanBeAdded()
		{
			//Arrange
			var viewmodel = createSut();
			viewmodel.TagName = "Check";
			//Act
			//Assert
			viewmodel.AddTagCommand.CanExecute(null).ShouldBeTrue();

		}

		[TestMethod]
		public void RemoveTag_NoTagIsSelected_TagcannotbeRemoved()
		{

			//Arrange
			var viewmodel = createSut();
			viewmodel.Selectedtag = null;
			//Act
			//Assert
			viewmodel.RemoveTagCommand.CanExecute(null).ShouldBeFalse();
		}

		[TestMethod]
		public void RemoveTag_TagIsSelected_TagbeRemoved()
		{

			//Arrange
			var viewmodel = createSut();
			viewmodel.Selectedtag = CreateModel("check");
			//Act
			//Assert
			viewmodel.RemoveTagCommand.CanExecute(null).ShouldBeTrue();
		}

		[TestMethod]
		public void UpdateTagname_AddTagCommand_CanExecuteEventIsRaised()
		{
			//arrange
			bool eventisRaised = false;
			var viewmodel = createSut();
			//Act
			viewmodel.AddTagCommand.CanExecuteChanged +=
				(Sender, args) => { eventisRaised = true; };

			viewmodel.TagName = "check";
			//Assert
			eventisRaised.ShouldBeTrue();
		}

		[TestMethod]
		public void UpdateTagname_TagNameisNotChanged_CanExecuteEventIsNotRaised()
		{
			//arrange
			bool eventisRaised = false;
			var viewmodel = createSut();
			//Act
			viewmodel.AddTagCommand.CanExecuteChanged +=
				(Sender, args) => { eventisRaised = true; };

			//Assert
			eventisRaised.ShouldBeFalse();
		}

		[TestMethod]
		public void UpdateTagname_RemoveTagCommand_CanExecuteEventIsRaised()
		{
			//arrange
			bool eventisRaised = false;
			var viewmodel = createSut();
			//Act
			viewmodel.RemoveTagCommand.CanExecuteChanged +=
				(Sender, args) => eventisRaised = true; 

			viewmodel.Selectedtag = null;
			//Assert
			eventisRaised.ShouldBeTrue();
		}


		[TestMethod]
		public void AddTag_tagIsSet_TagAddedToTagRepository()
		{

			//Arrange
			var tagrepositoryMock = new Mock<ITagRepository>();
			var viewmodel = createSut(null,tagrepositoryMock.Object);
			viewmodel.TagName = "some Tag";
			//Act

			viewmodel.AddTagCommand.Execute(null);
			//Assert
			tagrepositoryMock.Verify(repo => repo.Add(It.Is<ToDoItemTags>(todoItem => todoItem.Name ==  "some Tag")));

		}


		[TestMethod]
		public void Remove_tagIsSelected_RemovedFromTagRepository()
		{
			//Arrange
			var tagrepositoryMock = new Mock<ITagRepository>();
			var tagtoRemoveVM = CreateModel("Tag To Remove");
			var viewmodel = createSut(null,tagrepositoryMock.Object);

			viewmodel.Selectedtag = tagtoRemoveVM;
			//Act
			viewmodel.RemoveTagCommand.Execute(null);
			//Assert
			//tagrepositoryMock.Verify(repo => repo.Remove(It.IsAny<Guid>()));
			tagrepositoryMock.Verify(repo => repo.Remove(tagtoRemoveVM.Id));
		}


		[TestMethod]
		public void Removetag_TagisUsedByToDoItem_TagCannotBeremoved()
		{
			//Arrange
			var selectedtag = CreateModel("selected Tag");
			var referTagIds = new[] { selectedtag.Id };
			var viewmodel = createSut(referTagIds);
			//Act
			var result= viewmodel.RemoveTagCommand.CanExecute(null);
			//Assert
			//tagrepositoryMock.Verify(repo => repo.Remove(It.IsAny<Guid>()));
			result.ShouldBeFalse();
		}


		private IDialogService dialogService;

		private ManageTagsDialogViewModel createSut(IEnumerable<Guid> referencetagId = null,ITagRepository tagRepository = null)
		{
			tagRepository = tagRepository ?? new TagRepository();
			referencetagId = referencetagId ?? Enumerable.Empty<Guid>();
			return new ManageTagsDialogViewModel(
				new ObservableCollection<ToDoItemTagsViewModel>(),
				referencetagId,
				 dialogService,
				 tagRepository);
		}

		private ToDoItemTagsViewModel CreateModel(string value)
		{
			return new ToDoItemTagsViewModel(new ToDoItemTags()
			{
				Id = Guid.NewGuid(),
				Name = value
			},Mock.Of<ITagRepository>());
		}

	}
}

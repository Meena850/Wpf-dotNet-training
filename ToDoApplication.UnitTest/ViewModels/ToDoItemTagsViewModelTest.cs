using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.Model;
using ToDoApplication.Properties;
using ToDoApplication.Repositories;
using ToDoApplication.ViewModels;

namespace ToDoApplication.UnitTest.ViewModels
{
	[TestClass]

	public class ToDoItemTagsViewModelTest
	{
		[TestMethod]
		public void settagName_TagNameisUpdated_InTagRepository()
		{
			//Arrange
			var tagRepoMock = new Mock<ITagRepository>();
			var tagItem = Createtag("Tag 123");
			var tagvm = createsut(tagItem, tagRepoMock.Object);
			//Act

			tagvm.Name = "changed Name";
			//Assert
			tagRepoMock.Verify(repo => repo.Update(It.Is<ToDoItemTags>
				(tag => tag.Id == tagItem.Id && tag.Name == "changed Name")));

		}
		[TestMethod]
		public void SetTagName_ToEmptyString_NamePropertyHasError()
		{
			//Arrange
			var tagVM = createsut(Createtag("Tag 1"));
			//Act
			tagVM.Name = String.Empty;
			//Assert
			tagVM[nameof(tagVM.Name)].ShouldBe(Resources.TagEmptyError);

		}

        [TestMethod]
        public void SetTagName_NameIsNotUnique_NamePropertyHasError()
        {
			//Arrange
			var existingTags = new List<ToDoItemTags>
			{
				Createtag("I already  exist"),
			};
			var tagRepositoryMock = new Mock<ITagRepository>();
			tagRepositoryMock.Setup(repo => repo.GetAll()).Returns(existingTags);

			var tagVM = createsut(Createtag("Tag 1"), tagRepositoryMock.Object);

			//Act
			tagVM.Name = "I alredy exist";
			//Assert
			tagVM[nameof(tagVM.Name)].ShouldBe(Resources.TagNotUniqueError);

        }

        [TestMethod]
        public void SetTagName_NameIsNotValid_UpdateIsNotCalled()
        {
			//Arrange
			var tagRepositoryMock = new Mock<ITagRepository>();
			var tagVM = createsut(Createtag("Some Name"), tagRepositoryMock.Object);
			//Act
			tagVM.Name = String.Empty;
			//Assert
			tagRepositoryMock.Verify(repo => repo.Update(It.IsAny<ToDoItemTags>()), Times.Never);
        }
		private ToDoItemTags Createtag(string name)
		{
		    return new ToDoItemTags()
		    {
			   Id = Guid.NewGuid(),
			     Name = name
		    };

		}

		private ToDoItemTagsViewModel createsut(ToDoItemTags toDoItem, ITagRepository tagRepo = null)
		{
			tagRepo = tagRepo ?? Mock.Of<ITagRepository>();

			return new ToDoItemTagsViewModel(toDoItem,tagRepo);
		}
	}
}

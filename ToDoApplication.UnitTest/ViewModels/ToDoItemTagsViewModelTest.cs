using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.Model;
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

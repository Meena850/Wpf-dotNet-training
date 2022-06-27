using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ToDoApplication.Model;
using ToDoApplication.Repositories;
using ToDoApplication.Services;

namespace ToDOApplication.IntegrationTest.Repositories.TodoItemRepositoryTest.Data
{
    [TestClass]
    public class TodoItemRepositoryTest: IntegrationTestBase
    {
        public TodoItemRepositoryTest():
            base("Repositories", "TodoItemRepositoryTest", "Data")
        {
                
        }
        
        [TestMethod]
        public async Task  GetAll_FileContainsTwoItems_ReturnsListWIthTwoItems()
        {                
                //Arrange
                //Create a unique directory for this test method
                //Copy test files to directory
                var todoItemTestFile = CopyFileTotestDir("GetAllTExtFile.json");
                var appConfigMock = new Mock<IAppConfigService>();
                appConfigMock.Setup(s => s.TodoItemFile).Returns(todoItemTestFile);
                var repository = CreateSut(todoItemTestFile);
                //Act
                var todosResult = await repository.GetAll();
            //Assert
            todosResult.Value.Count.ShouldBe(2);
        }

        [TestMethod]
        public async Task Add_FileIsEmpty_ItemIsAddedToTheFile()
        {
            //Arrange
            //Create a unique directory for this test method
            //Copy test files to directory
            var todoItemTestFile = CopyFileTotestDir("EmptyTestFile.json");
            var repository = CreateSut(todoItemTestFile);
            //Act
            var _ = await repository.Add(CreateToDoitem("Create Integration Tests"));
            //Assert
            File.ReadAllText(todoItemTestFile.FullName).ShouldContain("\"Name\": \"Create Integration Tests\"");
        }

        [TestMethod]
        public async Task Remove_FileHas2TodoItems_FirstItemIsRemovedFromFile()
        {
            //Arrange
            var testFile = CopyFileTotestDir("GetAllTextFile.json");
            var repository = CreateSut(testFile);
            //Act
            var _ = await repository.Remove(Guid.Parse("af774bcb-4bcb-4dfa-b25b-40bedb3fff24"));

            //Assert
            File.ReadAllText(testFile.FullName).ShouldNotContain("af774bcb-4bcb-4dfa-b25b-40bedb3fff24");

        }

        private IAppConfigService CreateFakeConfigservice(FileInfo todoItemFile)
        {
            var appConfigMock = new Mock<IAppConfigService>();
            appConfigMock.Setup(s => s.TodoItemFile).Returns(todoItemFile);
            return appConfigMock.Object;

        }


     
        private ToDoItemModel  CreateToDoitem(string name)
        {
            return new ToDoItemModel()
            {
                Name = name,
                Id = Guid.NewGuid(),
                IsDone = false,
                TagId = new List<Guid>(),
                Timestamp = DateTime.Now,
            };

        }
        private ITodoItemRepository CreateSut(FileInfo testFile)
        {
            var configService = CreateFakeConfigservice(testFile);
            return new TodoItemRepository(configService);
        }
    }
}

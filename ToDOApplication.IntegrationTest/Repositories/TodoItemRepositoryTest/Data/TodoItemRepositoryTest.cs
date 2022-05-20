using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ToDoApplication.Model;
using ToDoApplication.Repositories;
using ToDoApplication.Services;

namespace ToDOApplication.IntegrationTest.Repositories.TodoItemRepositoryTest.Data
{
    [TestClass]
    public class TodoItemRepositoryTest
    {
        private readonly string _testDir = Path.Combine(Path.GetTempPath(), "TodoItemRepositoryTest");
        [TestMethod]
        public void GetAll_FileContainsTwoItems_ReturnsListWIthTwoItems()
        {                
                //Arrange
                //Create a unique directory for this test method
                ReCreateDirectory(_testDir);
                //Copy test files to directory
                var todoItemTestFile = CopyFileTotestDir(_testDir, "GetAllTExtFile.json");
                var appConfigMock = new Mock<IAppConfigService>();
                appConfigMock.Setup(s => s.TodoItemFile).Returns(todoItemTestFile);
                var repository = CreateSut(todoItemTestFile);
                //Act
                var todos = repository.GetAll();
                //Assert
                todos.Count.ShouldBe(2);
        }

        [TestMethod]
        public void Add_FileIsEmpty_ItemIsAddedToTheFile()
        {
            //Arrange
            //Create a unique directory for this test method
            //Copy test files to directory
            ReCreateDirectory(_testDir);
            var todoItemTestFile = CopyFileTotestDir(_testDir, "EmptyTestFile.json");
            var appConfigMock = new Mock<IAppConfigService>();
            appConfigMock.Setup(s => s.TodoItemFile).Returns(todoItemTestFile);
            var repository = CreateSut(todoItemTestFile);
            //Act
            repository.Add(CreateToDoitem("Create Integration Tests"));
            //Assert
            File.ReadAllText(todoItemTestFile.FullName).ShouldContain("\"Name\": \"Create Integration Tests\"");
        }

        [TestMethod]
        public void Remove_FileHas2TodoItems_FirstItemIsRemovedFromFile()
        {
            //Arrange
            ReCreateDirectory(_testDir);
            var testFile = CopyFileTotestDir(_testDir, "GetAllTextFile.json");
            var repository = CreateSut(testFile);
            //Act
            repository.Remove(Guid.Parse("af774bcb-4bcb-4dfa-b25b-40bedb3fff24"));

            //Assert
            File.ReadAllText(testFile.FullName).ShouldNotContain("af774bcb-4bcb-4dfa-b25b-40bedb3fff24");

        }

        [TestCleanup]
        public void CleanUp()
        {
            //CleanUp
            Directory.Delete(_testDir, true);

        }

        private IAppConfigService CreateFakeConfigservice(FileInfo todoItemFile)
        {
            var appConfigMock = new Mock<IAppConfigService>();
            appConfigMock.Setup(s => s.TodoItemFile).Returns(todoItemFile);
            return appConfigMock.Object;

        }

        private void ReCreateDirectory(String directory)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
            Directory.CreateDirectory(directory);

        }

        private FileInfo CopyFileTotestDir(string testDir, string filename)
        {
            var SourceFileName = Path.Combine(Environment.CurrentDirectory, "Repositories", "TodoItemRepositoryTest", "Data", filename);
            var TargetFileName = Path.Combine(testDir, filename);
            File.Copy(SourceFileName, TargetFileName, true);
            return new FileInfo(TargetFileName);

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

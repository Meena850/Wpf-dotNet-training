using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.IO;
using ToDoApplication.Repositories;
using ToDoApplication.Services;

namespace ToDOApplication.IntegrationTest.Repositories.TodoItemRepositoryTest.Data
{
    [TestClass]
    public class TodoItemRepositoryTest
    {
        [TestMethod]
        public void GetAll_FileContainsTwoItems_ReturnsListWIthTwoItems()
        {                
                //Arrange
                //Create a unique directory for this test method
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var testDir = Path.Combine(Path.GetTempPath(), "TodoAppTest", Path.GetRandomFileName());
                if (Directory.Exists(testDir))
                {
                    Directory.Delete(testDir, true);
                }
                Directory.CreateDirectory(testDir);
            try
            {
                //Copy test files to directory
                var SourceFileName = Path.Combine(Environment.CurrentDirectory, "Repositories", "TodoItemRepositoryTest", "Data", "GetAllTextFile.json");
                var TargetFileName = Path.Combine(testDir, "GetAllFile.json");
                File.Copy(SourceFileName, TargetFileName, true);

                var todoItemTestFile = new FileInfo(TargetFileName);
                var appConfigMock = new Mock<IAppConfigService>();
                appConfigMock.Setup(s => s.TodoItemFile).Returns(todoItemTestFile);
                var repository = CreateSut(appConfigMock.Object);
                //Act
                var todos = repository.GetAll();
                //Assert
                todos.Count.ShouldBe(1);
            }
            //CleanUp
            finally
            {
                //Remove the Created test dircetory
                Directory.Delete(testDir, true);
            }
        }
        private ITodoItemRepository CreateSut(IAppConfigService configService)
        {
            return new TodoItemRepository(configService);
        }
    }
}

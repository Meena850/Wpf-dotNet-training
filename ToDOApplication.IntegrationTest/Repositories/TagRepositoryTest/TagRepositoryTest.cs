using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.IO;
using ToDoApplication.Model;
using ToDoApplication.Repositories;
using ToDoApplication.Services;

namespace ToDOApplication.IntegrationTest.Repositories.TagRepositoryTest
{
    [TestClass]
    public class TagRepositoryTest
    {

        private readonly string  testDir = Path.Combine(Path.GetTempPath(),"TagRepositoryTest");
        [TestMethod]
        public void GetAll_FileContains4Tags_ReturnsListWith4Tags()
        {
            //Arrange
            RecreateDirectory(testDir);
            //Copy test files to directory
            var tagItemTestFile = CopyFileTotestDir("ReadTagItems.json");
            var repository = CreateSut(tagItemTestFile);
            //Act
            var tags = repository.GetAll();

            //Assert
            tags.Count.ShouldBe(4);
        }

        [TestMethod]
        public void Add_FileIsEmpty_TagIsAddedToTheFile()
        {
            //Arrange
            //Create Test Directory
            RecreateDirectory(testDir);

            //Copy Test Files to directory
            var tagItemTestFile = CopyFileTotestDir("EmptyTagFile.json");
            var appConfigMock = new Mock<IAppConfigService>();
            appConfigMock.Setup(s => s.TagItemFile).Returns(tagItemTestFile);
            var repository = CreateSut(tagItemTestFile);
            //Act
            repository.Add(CreateTagItem("Tag1"));
            //Assert
            File.ReadAllText(tagItemTestFile.FullName).ShouldContain("\"Name\": \"Tag1\"");
        }

        [TestMethod]
        public void Remove_FileHas2TodoItems_FirstItemIsRemovedFromFile()
        {
            //Arrange
            RecreateDirectory(testDir);
            var TagItemtestFile = CopyFileTotestDir("GetAllTagFile.json");
            var repository = CreateSut(TagItemtestFile);
            //Act
            repository.Remove(Guid.Parse("ce8146d2-1312-4bf0-a90b-8cd8f0106ac0"));

            //Assert
            File.ReadAllText(TagItemtestFile.FullName).ShouldNotContain("ce8146d2-1312-4bf0-a90b-8cd8f0106ac0");

        }



        private ToDoItemTags CreateTagItem(string tagname)
        {
            return new ToDoItemTags()
            {
                
                Id = Guid.NewGuid(),
                Name = tagname

            };
        }

        [TestCleanup]
        public void CleanUp()
        {
            //CleanUp
            Directory.Delete(testDir, true);

        }
        private void RecreateDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
            Directory.CreateDirectory(directory);

        }

        private FileInfo CopyFileTotestDir( string filename)
        {
            var SourceFileName = Path.Combine(Environment.CurrentDirectory, "Repositories", "TagRepositoryTest", "Data", "GetAllTagFile.json");
            var TargetFileName = Path.Combine(filename);
            File.Copy(SourceFileName, TargetFileName, true);
            return new FileInfo(TargetFileName);
        }
        private ITagRepository CreateSut(FileInfo filename)
        {
            var appconfig=CreateFakeConfigservice(filename);
            return new TagRepository(appconfig);
        }

        private IAppConfigService CreateFakeConfigservice(FileInfo tagItemFile)
        {
            var appConfigMock = new Mock<IAppConfigService>();
            appConfigMock.Setup(s => s.TagItemFile).Returns(tagItemFile);
            return appConfigMock.Object;

        }



    }
}

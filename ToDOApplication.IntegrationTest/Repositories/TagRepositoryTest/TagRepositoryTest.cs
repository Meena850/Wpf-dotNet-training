using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.IO;
using System.Threading.Tasks;
using ToDoApplication.Model;
using ToDoApplication.Repositories;
using ToDoApplication.Services;

namespace ToDOApplication.IntegrationTest.Repositories.TagRepositoryTest
{
    [TestClass]
    public class TagRepositoryTest : IntegrationTestBase
    {
        public TagRepositoryTest():
            base("Repositories", "TagRepositoryTest", "Data")
        {
                
        }
       
        [TestMethod]
        public async Task GetAll_FileContains4Tags_ReturnsListWith4Tags()
        {
            //Arrange
            //Copy test files to directory
            var tagItemTestFile = CopyFileTotestDir("GetAllTagFile.json");
            var repository = CreateSut(tagItemTestFile);
            //Act
            var tags = await repository.GetAll();

            //Assert
            tags.Count.ShouldBe(4);
        }

        [TestMethod]
        public void Add_FileIsEmpty_TagIsAddedToTheFile()
        {
            //Arrange
            //Create Test Directory

            //Copy Test Files to directory
            var tagItemTestFile = CopyFileTotestDir("GetAllTagFile.json");
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
            var TagItemtestFile = CopyFileTotestDir("GetAllTagFile.json");
            var repository = CreateSut(TagItemtestFile);
            //Act
            repository.Remove(Guid.Parse("ce8146d2-1312-4bf0-a90b-8cd8f0106ac0"));

            //Assert
            File.ReadAllText(TagItemtestFile.FullName).ShouldNotContain("ce8146d2-1312-4bf0-a90b-8cd8f0106ac0");

        }

        //[TestMethod]
        //public void settagName_TagNameisUpdated_InTagRepository()
        //{

        //    //Arrange
        //    //Create Test Directory
        //    RecreateDirectory(testDir);

        //    //Copy Test Files to directory
        //    var tagItemTestFile = CopyFileTotestDir("EmptyTagFile.json");
        //    var appConfigMock = new Mock<IAppConfigService>();
        //    var tagRepoMock = new Mock<ITagRepository>();
        //    appConfigMock.Setup(s => s.TagItemFile).Returns(tagItemTestFile);
        //    var repository = CreateSut(tagItemTestFile);
        //    //Act
        //    repository.Update(UpdateTagItem("Updated"));
        //    //Assert
        //  //  tagRepoMock.Verify(repo => repo.Update(It.Is<ToDoItemTags>(tag => tag.Name == "Updated")));
        //}



        private ToDoItemTags CreateTagItem(string tagname)
        {
            return new ToDoItemTags()
            {
                
                Id = Guid.NewGuid(),
                Name = tagname

            };
        }
        private ToDoItemTags UpdateTagItem(string tagname)
        {
            return new ToDoItemTags()
            {

                Id = Guid.Parse("ce8146d2-1312-4bf0-a90b-8cd8f0106ac0"),
                Name = "Tag2"

            };
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

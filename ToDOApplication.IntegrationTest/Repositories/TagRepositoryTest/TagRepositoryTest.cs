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
        [TestMethod]
        public void GetAll_FileContains4Tags_ReturnsListWith4Tags()
        {
            //Arrange
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var testDir = Path.Combine(Path.GetTempPath(), "TodoAppTest", Path.GetRandomFileName());
            if (Directory.Exists(testDir))
            {
                Directory.Delete(testDir, true);
            }
            Directory.CreateDirectory(testDir);
            //Act
            try
            {
                //Copy test files to directory
                var SourceFileName = Path.Combine(Environment.CurrentDirectory, "Repositories", "TagRepositoryTest", "Data", "GetAllTagFile.json");
                var TargetFileName = Path.Combine(testDir, "TagItem.json");
                File.Copy(SourceFileName, TargetFileName, true);

                var tagItemTestFile = new FileInfo(TargetFileName);
                var appConfigMock = new Mock<IAppConfigService>();
                appConfigMock.Setup(s => s.TagItemFile).Returns(tagItemTestFile);
                
                var repository = CreateSut(appConfigMock.Object);
                var tags = repository.GetAll();


                tags.Count.ShouldBe(4);
            }
            //CleanUp
            finally
            {
                //Remove the Created test dircetory
                Directory.Delete(testDir, true);
            }
        }
        private ITagRepository CreateSut(IAppConfigService configService)
        {
            return new TagRepository(configService);
        }

    }
}

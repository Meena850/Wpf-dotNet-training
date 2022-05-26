using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDOApplication.IntegrationTest.Repositories
{
    public abstract class IntegrationTestBase
    {
        protected string TestDir { get; }
        private readonly string[] _relativeTestDataFolders;
        public IntegrationTestBase( params string[] relativeTestDataFolders)
        {
            TestDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            _relativeTestDataFolders = relativeTestDataFolders;
        }

        [TestInitialize]
        public void Initialize()
        {
            ReCreateDirectory(TestDir);
        }
        
        [TestCleanup]
        public void CleanUp()
        {
            //CleanUp
            Directory.Delete(TestDir, true);

        }

        protected void ReCreateDirectory(String directory)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);  
            }
            Directory.CreateDirectory(directory);

        }
        protected FileInfo CopyFileTotestDir(string filename)
        {
            var sourcefileNameParts = _relativeTestDataFolders.Prepend(Environment.CurrentDirectory).Append(filename).ToArray();
            var SourceFileName = Path.Combine(sourcefileNameParts);
            var TargetFileName = Path.Combine(TestDir, filename);
            File.Copy(SourceFileName, TargetFileName, true);
            return new FileInfo(TargetFileName);

        }

    }
}

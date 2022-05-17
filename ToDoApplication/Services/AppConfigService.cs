using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApplication.Services
{
    internal class AppConfigService : IAppConfigService
    {
        const string _directoryPath = @"C:\Krones_CRD\TestFiles";
        const string _fileName = "ToDoItem.json";
        private string _tagfileName = "TagItem.json";
        public FileInfo TodoItemFile { get; }
        public FileInfo TagItemFile { get; }

        public AppConfigService()
        {
            TodoItemFile = GetTodoItemFile();
            TagItemFile = GetTagItemFile();
        }

        private FileInfo GetTagItemFile()
        {
            var path = Path.Combine(_directoryPath, _tagfileName);
            return new FileInfo(path);
        }

        private FileInfo GetTodoItemFile()
        {
            var path = Path.Combine(_directoryPath, _fileName);
            return new FileInfo(path);
        }
    }
}

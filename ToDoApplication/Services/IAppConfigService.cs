using System.IO;

namespace ToDoApplication.Services
{
    internal interface IAppConfigService
    {
        FileInfo TodoItemFile { get; }
        FileInfo TagItemFile { get; }
    }
}

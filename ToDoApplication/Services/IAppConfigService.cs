using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApplication.Services
{
    internal interface IAppConfigService
    {
        FileInfo TodoItemFile { get; }
        FileInfo TagItemFile { get; }
    }
}

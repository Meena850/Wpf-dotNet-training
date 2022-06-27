using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApplication.Model;
using ToDoApplication.Services;
using ToDoApplication.Util;

namespace ToDoApplication.Repositories
{
	class TagRepository : FileBasedRepository<ToDoItemTags>, ITagRepository
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(TagRepository));
		public TagRepository(IAppConfigService configService): base(configService.TagItemFile)
		{
		}
	}
}

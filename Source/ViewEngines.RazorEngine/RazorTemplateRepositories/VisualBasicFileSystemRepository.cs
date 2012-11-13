using System.IO;

using Junior.Route.Common;
using Junior.Route.ViewEngines.RazorEngine.RazorTemplateNameMappers;

namespace Junior.Route.ViewEngines.RazorEngine.RazorTemplateRepositories
{
	public class VisualBasicFileSystemRepository : FileSystemRepository
	{
		public VisualBasicFileSystemRepository(string viewRelativeRootDirectory, IRazorTemplateNameMapper nameMapper, IFileSystem fileSystem)
			: base(viewRelativeRootDirectory, "*.vbhtml", SearchOption.AllDirectories, nameMapper, fileSystem)
		{
		}

		public VisualBasicFileSystemRepository(string viewRelativeRootDirectory, SearchOption searchOption, IRazorTemplateNameMapper nameMapper, IFileSystem fileSystem)
			: base(viewRelativeRootDirectory, "*.vbhtml", searchOption, nameMapper, fileSystem)
		{
		}
	}
}
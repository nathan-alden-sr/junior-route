using System.IO;

using Junior.Route.Common;
using Junior.Route.ViewEngines.RazorEngine.RazorTemplateNameMappers;

namespace Junior.Route.ViewEngines.RazorEngine.RazorTemplateRepositories
{
	public class CSharpFileSystemRepository : FileSystemRepository
	{
		public CSharpFileSystemRepository(string viewRelativeRootDirectory, IRazorTemplateNameMapper nameMapper, IFileSystem fileSystem)
			: base(viewRelativeRootDirectory, "*.cshtml", SearchOption.AllDirectories, nameMapper, fileSystem)
		{
		}

		public CSharpFileSystemRepository(string viewRelativeRootDirectory, SearchOption searchOption, IRazorTemplateNameMapper nameMapper, IFileSystem fileSystem)
			: base(viewRelativeRootDirectory, "*.cshtml", searchOption, nameMapper, fileSystem)
		{
		}
	}
}
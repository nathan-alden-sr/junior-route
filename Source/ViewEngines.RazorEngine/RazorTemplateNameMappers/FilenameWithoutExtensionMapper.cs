using System.IO;

using Junior.Common;

namespace Junior.Route.ViewEngines.RazorEngine.RazorTemplateNameMappers
{
	public class FilenameWithoutExtensionMapper : IRazorTemplateNameMapper
	{
		public string Map(string relativePath)
		{
			relativePath.ThrowIfNull("relativePath");

			return Path.GetFileNameWithoutExtension(relativePath);
		}
	}
}
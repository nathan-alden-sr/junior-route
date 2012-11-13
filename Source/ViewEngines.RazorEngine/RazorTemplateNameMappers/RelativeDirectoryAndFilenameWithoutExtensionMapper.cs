using System.IO;

using Junior.Common;

namespace Junior.Route.ViewEngines.RazorEngine.RazorTemplateNameMappers
{
	public class RelativeDirectoryAndFilenameWithoutExtensionMapper : IRazorTemplateNameMapper
	{
		public string Map(string relativePath)
		{
			relativePath.ThrowIfNull("relativePath");

			string directory = Path.GetDirectoryName(relativePath);
			string filenameWithoutExtension = Path.GetFileNameWithoutExtension(relativePath);

			return Path.Combine(directory, filenameWithoutExtension);
		}
	}
}
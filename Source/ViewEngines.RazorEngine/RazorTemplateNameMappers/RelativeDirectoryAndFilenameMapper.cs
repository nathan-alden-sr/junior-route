using Junior.Common;

namespace Junior.Route.ViewEngines.RazorEngine.RazorTemplateNameMappers
{
	public class RelativeDirectoryAndFilenameMapper : IRazorTemplateNameMapper
	{
		public string Map(string relativePath)
		{
			relativePath.ThrowIfNull("relativePath");

			return relativePath;
		}
	}
}
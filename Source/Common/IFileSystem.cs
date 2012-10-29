using System.Collections.Generic;
using System.IO;

namespace Junior.Route.Common
{
	public interface IFileSystem
	{
		string ApplicationDirectory
		{
			get;
		}

		string AbsolutePath(string relativePath);
		string CombineRelativePaths(IEnumerable<string> relativePaths);
		string CombineRelativePaths(params string[] relativePaths);
		int FileSize(string path);
		Stream ReadFile(string path);
	}
}
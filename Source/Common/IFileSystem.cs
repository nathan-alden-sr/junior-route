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
		Stream OpenFile(string path);
		Stream OpenFile(string path, FileMode mode);
		Stream OpenFile(string path, FileMode mode, FileAccess access);
		Stream OpenFile(string path, FileMode mode, FileAccess access, FileShare share);
		string[] GetDirectoryFiles(string path);
		string[] GetDirectoryFiles(string path, string searchPattern);
		string[] GetDirectoryFiles(string path, string searchPattern, SearchOption searchOption);
		bool FileExists(string path);
		bool DirectoryExists(string directory);
	}
}
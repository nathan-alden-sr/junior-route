using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Junior.Common;
using Junior.Route.Common;
using Junior.Route.Routing;

namespace Junior.Route.AspNetIntegration
{
	public class FileSystem : IFileSystem
	{
		private readonly IHttpRuntime _httpRuntime;

		public FileSystem(IHttpRuntime httpRuntime)
		{
			httpRuntime.ThrowIfNull("httpRuntime");

			_httpRuntime = httpRuntime;
		}

		public string ApplicationDirectory
		{
			get
			{
				return _httpRuntime.AppDomainAppPath;
			}
		}

		public string AbsolutePath(string relativePath)
		{
			return Path.Combine(ApplicationDirectory, relativePath);
		}

		public string CombineRelativePaths(IEnumerable<string> relativePaths)
		{
			relativePaths.ThrowIfNull("relativePaths");

			return String.Join(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), relativePaths);
		}

		public string CombineRelativePaths(params string[] relativePaths)
		{
			return CombineRelativePaths((IEnumerable<string>)relativePaths);
		}

		public int FileSize(string path)
		{
			path.ThrowIfNull("path");

			long size = new FileInfo(path).Length;

			if (size > Int32.MaxValue)
			{
				throw new ArgumentException(String.Format("File size of '{0}' is larger than {1}.", path, Int32.MaxValue));
			}

			return (int)size;
		}

		public Stream OpenFile(string path)
		{
			path.ThrowIfNull("path");

			return File.OpenRead(path);
		}

		public Stream OpenFile(string path, FileMode mode)
		{
			return File.Open(path, mode);
		}

		public Stream OpenFile(string path, FileMode mode, FileAccess access)
		{
			return File.Open(path, mode, access);
		}

		public Stream OpenFile(string path, FileMode mode, FileAccess access, FileShare share)
		{
			return File.Open(path, mode, access, share);
		}

		public string[] GetDirectoryFiles(string path)
		{
			return Directory.GetFiles(path);
		}

		public string[] GetDirectoryFiles(string path, string searchPattern)
		{
			return Directory.GetFiles(path, searchPattern);
		}

		public string[] GetDirectoryFiles(string path, string searchPattern, SearchOption searchOption)
		{
			return Directory.GetFiles(path, searchPattern, searchOption);
		}

		public bool FileExists(string path)
		{
			path.ThrowIfNull("path");

			return File.Exists(path);
		}

		public bool DirectoryExists(string directory)
		{
			directory.ThrowIfNull("directory");

			return Directory.Exists(directory);
		}
	}
}
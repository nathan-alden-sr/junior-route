using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;

using Junior.Common;
using Junior.Route.Common;

namespace Junior.Route.AspNetIntegration
{
	public class FileSystem : IFileSystem
	{
		public string ApplicationDirectory
		{
			get
			{
				return HttpRuntime.AppDomainAppPath;
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

		public Stream ReadFile(string path)
		{
			path.ThrowIfNull("path");

			return File.OpenRead(path);
		}
	}
}
using System;
using System.IO;

using Junior.Common;
using Junior.Route.Common;

namespace Junior.Route.ViewEngines.RazorEngine
{
	public class RazorTemplate : IRazorTemplate
	{
		private readonly Lazy<string> _contents;
		private readonly string _name;
		private readonly string _path;

		public RazorTemplate(string name, string path, IFileSystem fileSystem)
		{
			name.ThrowIfNull("name");
			path.ThrowIfNull("relativePath");
			fileSystem.ThrowIfNull("fileSystem");

			_name = name;
			_path = path;
			_contents = new Lazy<string>(
				() =>
					{
						using (var reader = new StreamReader(fileSystem.OpenFile(_path, FileMode.Open, FileAccess.Read, FileShare.Read)))
						{
							return reader.ReadToEnd();
						}
					});
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public string Path
		{
			get
			{
				return _path;
			}
		}

		public string GetContents()
		{
			return _contents.Value;
		}
	}
}
using System.Collections.Generic;
using System.IO;
using System.Text;

using Junior.Common;
using Junior.Route.Common;

namespace Junior.Route.Assets.FileSystem
{
	public class FileAsset : IAsset
	{
		private readonly Encoding _encoding;
		private readonly string _relativePath;

		public FileAsset(string relativePath, Encoding encoding)
		{
			_relativePath = relativePath;
			_encoding = encoding;
		}

		public string RelativePath
		{
			get
			{
				return _relativePath;
			}
		}

		public Encoding Encoding
		{
			get
			{
				return _encoding;
			}
		}

		public IEnumerable<AssetFile> ResolveAssetFiles(IFileSystem fileSystem)
		{
			fileSystem.ThrowIfNull("fileSystem");

			string path = fileSystem.AbsolutePath(_relativePath);

			yield return new AssetFile(path, _encoding);
		}

		public FileSystemWatcher GetFileSystemWatcher(IFileSystem fileSystem)
		{
			string path = fileSystem.AbsolutePath(_relativePath);

			return new FileSystemWatcher(path);
		}
	}
}
using System.Collections.Generic;
using System.IO;
using System.Text;

using Junior.Route.Common;

namespace Junior.Route.Assets.FileSystem
{
	public interface IAsset
	{
		string RelativePath
		{
			get;
		}
		Encoding Encoding
		{
			get;
		}

		IEnumerable<AssetFile> ResolveAssetFiles(IFileSystem fileSystem);
		FileSystemWatcher GetFileSystemWatcher(IFileSystem fileSystem);
	}
}
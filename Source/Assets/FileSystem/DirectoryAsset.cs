using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Junior.Common;
using Junior.Route.Common;

namespace Junior.Route.Assets.FileSystem
{
	public class DirectoryAsset : IAsset
	{
		private readonly Encoding _encoding;
		private readonly IFileFilter _filter;
		private readonly string _relativeDirectory;
		private readonly SearchOption _searchOption;
		private readonly string _searchPattern;

		public DirectoryAsset(string relativeDirectory, Encoding encoding, string searchPattern = "*.*", SearchOption option = SearchOption.AllDirectories, IFileFilter filter = null)
		{
			relativeDirectory.ThrowIfNull("relativeDirectory");
			encoding.ThrowIfNull("encoding");
			filter.ThrowIfNull("filter");

			_relativeDirectory = relativeDirectory;
			_encoding = encoding;
			_searchPattern = searchPattern;
			_searchOption = option;
		}

		public DirectoryAsset(string relativeDirectory, string searchPattern = "*.*", SearchOption option = SearchOption.AllDirectories, IFileFilter filter = null)
		{
			relativeDirectory.ThrowIfNull("relativeDirectory");

			_relativeDirectory = relativeDirectory;
			_filter = filter;
			_searchPattern = searchPattern;
			_searchOption = option;
		}

		public DirectoryAsset(string relativeDirectory, Encoding encoding, string searchPattern = "*.*", SearchOption option = SearchOption.AllDirectories)
		{
			relativeDirectory.ThrowIfNull("relativeDirectory");
			encoding.ThrowIfNull("encoding");

			_relativeDirectory = relativeDirectory;
			_encoding = encoding;
			_searchPattern = searchPattern;
			_searchOption = option;
		}

		public DirectoryAsset(string relativeDirectory, string searchPattern = "*.*", SearchOption option = SearchOption.AllDirectories)
		{
			relativeDirectory.ThrowIfNull("relativeDirectory");

			_relativeDirectory = relativeDirectory;
			_searchPattern = searchPattern;
			_searchOption = option;
		}

		public string RelativePath
		{
			get
			{
				return _relativeDirectory;
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

			string directory = fileSystem.AbsolutePath(_relativeDirectory);
			IEnumerable<string> paths = Directory.GetFiles(directory, _searchPattern, _searchOption);

			return paths
				.Where(arg => _filter == null || _filter.Filter(arg) == FilterResult.Include)
				.Select(arg => new AssetFile(arg, _encoding));
		}

		public FileSystemWatcher GetFileSystemWatcher(IFileSystem fileSystem)
		{
			fileSystem.ThrowIfNull("fileSystem");

			string directory = fileSystem.AbsolutePath(_relativeDirectory);

			return new FileSystemWatcher(directory, _searchPattern)
				{
					IncludeSubdirectories = _searchOption == SearchOption.AllDirectories
				};
		}
	}
}
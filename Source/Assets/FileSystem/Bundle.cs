using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using Junior.Common;
using Junior.Route.Common;

namespace Junior.Route.Assets.FileSystem
{
	public class Bundle
	{
		private static readonly TimeSpan _fileReadRetryDelay = TimeSpan.FromMilliseconds(250);
		private readonly HashSet<IAsset> _assets = new HashSet<IAsset>();
		private readonly TimeSpan _fileReadTimeout;

		public Bundle(TimeSpan fileReadTimeout)
		{
			_fileReadTimeout = fileReadTimeout;
		}

		public Bundle()
			: this(TimeSpan.FromSeconds(30))
		{
		}

		public IEnumerable<IAsset> Assets
		{
			get
			{
				return _assets;
			}
		}

		public Bundle Directory(string relativeDirectory, Encoding encoding, string searchPattern = "*.*", SearchOption option = SearchOption.AllDirectories, IFileFilter filter = null)
		{
			relativeDirectory.ThrowIfNull("relativeDirectory");
			encoding.ThrowIfNull("encoding");
			searchPattern.ThrowIfNull("searchPattern");

			_assets.Add(new DirectoryAsset(relativeDirectory, encoding, searchPattern, option, filter));

			return this;
		}

		public Bundle Directory(string relativeDirectory, string searchPattern = "*.*", SearchOption option = SearchOption.AllDirectories, IFileFilter filter = null)
		{
			relativeDirectory.ThrowIfNull("relativeDirectory");
			searchPattern.ThrowIfNull("searchPattern");

			_assets.Add(new DirectoryAsset(relativeDirectory, searchPattern, option, filter));

			return this;
		}

		public Bundle Files(IEnumerable<string> relativePaths)
		{
			relativePaths.ThrowIfNull("relativePaths");

			foreach (string relativePath in relativePaths)
			{
				File(relativePath);
			}

			return this;
		}

		public Bundle File(string relativePath, Encoding encoding)
		{
			relativePath.ThrowIfNull("relativePath");
			encoding.ThrowIfNull("encoding");

			_assets.Add(new FileAsset(relativePath, encoding));

			return this;
		}

		public Bundle File(string relativePath)
		{
			relativePath.ThrowIfNull("relativePath");

			_assets.Add(new FileAsset(relativePath, null));

			return this;
		}

		public BundleContents GetContents(IFileSystem fileSystem, IComparer<AssetFile> assetOrder, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			fileSystem.ThrowIfNull("fileSystem");
			assetOrder.ThrowIfNull("assetOrder");
			concatenator.ThrowIfNull("concatenator");
			transformers.ThrowIfNull("transformers");

			IOrderedEnumerable<AssetFile> orderedAssets = _assets
				.SelectMany(arg => arg.ResolveAssetFiles(fileSystem))
				.OrderBy(arg => arg, assetOrder);

			return GetContents(orderedAssets, concatenator, transformers);
		}

		public BundleContents GetContents(IFileSystem fileSystem, IComparer<AssetFile> assetOrder, IAssetConcatenator concatenator, params IAssetTransformer[] transformers)
		{
			return GetContents(fileSystem, assetOrder, concatenator, (IEnumerable<IAssetTransformer>)transformers);
		}

		public BundleContents GetContents(IFileSystem fileSystem, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			fileSystem.ThrowIfNull("fileSystem");
			transformers.ThrowIfNull("transformers");
			concatenator.ThrowIfNull("concatenator");

			IEnumerable<AssetFile> unorderedAssets = _assets.SelectMany(arg => arg.ResolveAssetFiles(fileSystem));

			return GetContents(unorderedAssets, concatenator, transformers);
		}

		public BundleContents GetContents(IFileSystem fileSystem, IAssetConcatenator concatenator, params IAssetTransformer[] transformers)
		{
			return GetContents(fileSystem, concatenator, (IEnumerable<IAssetTransformer>)transformers);
		}

		private BundleContents GetContents(IEnumerable<AssetFile> assets, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			IEnumerable<string> transformedAssets = assets
				.Select(GetFileContents)
				.SelectMany(assetContents => transformers, (assetContents, transformer) => transformer.Transform(assetContents));

			return new BundleContents(concatenator.Concatenate(transformedAssets));
		}

		public static Bundle FromDirectory(string relativeDirectory, Encoding encoding, string searchPattern = "*.*", SearchOption option = SearchOption.AllDirectories, IFileFilter filter = null)
		{
			return new Bundle().Directory(relativeDirectory, encoding, searchPattern, option, filter);
		}

		public static Bundle FromDirectory(string relativeDirectory, string searchPattern = "*.*", SearchOption option = SearchOption.AllDirectories, IFileFilter filter = null)
		{
			return new Bundle().Directory(relativeDirectory, searchPattern, option, filter);
		}

		public static Bundle FromFiles(IEnumerable<string> relativePaths)
		{
			relativePaths.ThrowIfNull("relativePaths");

			var bundle = new Bundle();

			return bundle.Files(relativePaths);
		}

		public static Bundle FromFiles(params string[] relativePaths)
		{
			return FromFiles((IEnumerable<string>)relativePaths);
		}

		private string GetFileContents(AssetFile asset)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			FileStream stream = null;

			do
			{
				try
				{
					stream = System.IO.File.Open(asset.AbsolutePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

					StreamReader reader = asset.Encoding != null ? new StreamReader(stream, asset.Encoding) : new StreamReader(stream);

					return reader.ReadToEnd();
				}
				catch (FileNotFoundException)
				{
					Thread.Sleep(_fileReadRetryDelay);
				}
				catch (IOException)
				{
					Thread.Sleep(_fileReadRetryDelay);
				}
				catch (UnauthorizedAccessException)
				{
					Thread.Sleep(_fileReadRetryDelay);
				}
				finally
				{
					if (stream != null)
					{
						stream.Close();
						stream = null;
					}
				}
			} while (stopwatch.Elapsed < _fileReadTimeout);

			throw new ApplicationException(String.Format("The timeout of {0} was reached trying to read bundle file '{1}'.", _fileReadTimeout, asset.AbsolutePath));
		}
	}
}
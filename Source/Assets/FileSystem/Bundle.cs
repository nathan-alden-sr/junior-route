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

		public Bundle Directory(string relativeDirectory, Encoding encoding = null, string searchPattern = "*.*", SearchOption option = SearchOption.AllDirectories, IFileFilter filter = null)
		{
			relativeDirectory.ThrowIfNull("relativeDirectory");
			searchPattern.ThrowIfNull("searchPattern");

			_assets.Add(new DirectoryAsset(relativeDirectory, encoding, searchPattern, option, filter));

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

		public Bundle Files(params string[] relativePaths)
		{
			return Files((IEnumerable<string>)relativePaths);
		}

		public Bundle File(string relativePath, Encoding encoding = null)
		{
			relativePath.ThrowIfNull("relativePath");

			_assets.Add(new FileAsset(relativePath, encoding));

			return this;
		}

		public Bundle AddAssets(IEnumerable<IAsset> assets)
		{
			assets.ThrowIfNull("assets");

			_assets.AddRange(assets);

			return this;
		}

		public Bundle AddAssets(params IAsset[] assets)
		{
			return AddAssets((IEnumerable<IAsset>)assets);
		}

		public BundleContents GetContents(IFileSystem fileSystem, IComparer<AssetFile> assetOrder, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			fileSystem.ThrowIfNull("fileSystem");
			assetOrder.ThrowIfNull("assetOrder");
			concatenator.ThrowIfNull("concatenator");
			transformers.ThrowIfNull("transformers");

			AssetFile[] orderedAssets = _assets
				.SelectMany(arg => arg.ResolveAssetFiles(fileSystem))
				.OrderBy(arg => arg, assetOrder)
				.ToArray();

			return GetContents(fileSystem, orderedAssets, concatenator, transformers);
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

			AssetFile[] unorderedAssets = _assets.SelectMany(arg => arg.ResolveAssetFiles(fileSystem)).ToArray();

			return GetContents(fileSystem, unorderedAssets, concatenator, transformers);
		}

		public BundleContents GetContents(IFileSystem fileSystem, IAssetConcatenator concatenator, params IAssetTransformer[] transformers)
		{
			return GetContents(fileSystem, concatenator, (IEnumerable<IAssetTransformer>)transformers);
		}

		public BundleContents GetContents(IFileSystem fileSystem, IComparer<AssetFile> assetOrder, IEnumerable<IAssetTransformer> transformers)
		{
			fileSystem.ThrowIfNull("fileSystem");
			assetOrder.ThrowIfNull("assetOrder");
			transformers.ThrowIfNull("transformers");

			AssetFile[] orderedAssets = _assets
				.SelectMany(arg => arg.ResolveAssetFiles(fileSystem))
				.OrderBy(arg => arg, assetOrder)
				.ToArray();

			return GetContents(fileSystem, orderedAssets, new DelimiterConcatenator(), transformers);
		}

		public BundleContents GetContents(IFileSystem fileSystem, IComparer<AssetFile> assetOrder, params IAssetTransformer[] transformers)
		{
			return GetContents(fileSystem, assetOrder, new DelimiterConcatenator(), (IEnumerable<IAssetTransformer>)transformers);
		}

		public BundleContents GetContents(IFileSystem fileSystem, IEnumerable<IAssetTransformer> transformers)
		{
			fileSystem.ThrowIfNull("fileSystem");
			transformers.ThrowIfNull("transformers");

			AssetFile[] unorderedAssets = _assets.SelectMany(arg => arg.ResolveAssetFiles(fileSystem)).ToArray();

			return GetContents(fileSystem, unorderedAssets, new DelimiterConcatenator(), transformers);
		}

		public BundleContents GetContents(IFileSystem fileSystem, params IAssetTransformer[] transformers)
		{
			return GetContents(fileSystem, new DelimiterConcatenator(), (IEnumerable<IAssetTransformer>)transformers);
		}

		private BundleContents GetContents(IFileSystem fileSystem, IEnumerable<AssetFile> assets, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			string[] transformedFileContents = assets
				.Select(asset => GetFileContents(fileSystem, asset))
				.Select(fileContents => transformers.Aggregate(fileContents, (current, transformer) => transformer.Transform(current)))
				.ToArray();

			return new BundleContents(concatenator.Concatenate(transformedFileContents));
		}

		public static Bundle FromDirectory(string relativeDirectory, Encoding encoding = null, string searchPattern = "*.*", SearchOption option = SearchOption.AllDirectories, IFileFilter filter = null)
		{
			return new Bundle().Directory(relativeDirectory, encoding, searchPattern, option, filter);
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

		private string GetFileContents(IFileSystem fileSystem, AssetFile asset)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			Stream stream = null;

			do
			{
				try
				{
					stream = fileSystem.OpenFile(asset.AbsolutePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

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
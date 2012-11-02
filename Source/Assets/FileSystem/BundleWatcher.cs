using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;

using Junior.Common;
using Junior.Route.Common;

namespace Junior.Route.Assets.FileSystem
{
	public class BundleWatcher
	{
		private readonly IComparer<AssetFile> _assetOrder;
		private readonly Bundle _bundle;
		private readonly IAssetConcatenator _concatenator;
		private readonly IFileSystem _fileSystem;
		private readonly object _refreshLockObject = new object();
		private readonly IEnumerable<IAssetTransformer> _transformers;
		private readonly Timer _watchTimer = new Timer(500)
			{
				AutoReset = false
			};
		private readonly object _watchTimerLockObject = new object();
		private readonly HashSet<FileSystemWatcher> _watchers = new HashSet<FileSystemWatcher>();
		private string _contents;
		private string _hash;

		public event BundleChangedDelegate BundleChanged = null;

		public BundleWatcher(Bundle bundle, IFileSystem fileSystem, IComparer<AssetFile> assetOrder, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			bundle.ThrowIfNull("bundle");
			fileSystem.ThrowIfNull("fileSystem");
			assetOrder.ThrowIfNull("assetOrder");
			concatenator.ThrowIfNull("concatenator");
			transformers.ThrowIfNull("transformers");

			_bundle = bundle;
			_assetOrder = assetOrder;
			_concatenator = concatenator;
			_transformers = transformers;
			_fileSystem = fileSystem;
			_watchTimer.Elapsed += WatchTimerElapsed;
			RefreshContents();
			WatchForChanges();
		}

		public BundleWatcher(Bundle bundle, IFileSystem fileSystem, IComparer<AssetFile> assetOrder, IAssetConcatenator concatenator, params IAssetTransformer[] transformers)
			: this(bundle, fileSystem, assetOrder, concatenator, (IEnumerable<IAssetTransformer>)transformers)
		{
		}

		public BundleWatcher(Bundle bundle, IFileSystem fileSystem, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			bundle.ThrowIfNull("bundle");
			fileSystem.ThrowIfNull("fileSystem");
			concatenator.ThrowIfNull("concatenator");
			transformers.ThrowIfNull("transformers");

			_bundle = bundle;
			_concatenator = concatenator;
			_transformers = transformers;
			_fileSystem = fileSystem;
			_watchTimer.Elapsed += WatchTimerElapsed;
			RefreshContents();
			WatchForChanges();
		}

		public BundleWatcher(Bundle bundle, IFileSystem fileSystem, IAssetConcatenator concatenator, params IAssetTransformer[] transformers)
			: this(bundle, fileSystem, concatenator, (IEnumerable<IAssetTransformer>)transformers)
		{
		}

		public string Contents
		{
			get
			{
				lock (_refreshLockObject)
				{
					return _contents;
				}
			}
		}

		public string Hash
		{
			get
			{
				lock (_refreshLockObject)
				{
					return _hash;
				}
			}
		}

		private void WatchForChanges()
		{
			FileSystemWatcher[] watchers = _bundle.Assets.Select(arg => arg.GetFileSystemWatcher(_fileSystem)).ToArray();

			_watchers.AddRange(watchers);

			foreach (FileSystemWatcher watcher in watchers)
			{
				watcher.Created += WatcherChanged;
				watcher.Changed += WatcherChanged;
				watcher.Deleted += WatcherChanged;
				watcher.Renamed += WatcherChanged;
				watcher.EnableRaisingEvents = true;
			}
		}

		private void RefreshContents()
		{
			lock (_refreshLockObject)
			{
				BundleContents bundleContents = _assetOrder != null
					                                ? _bundle.GetContents(_fileSystem, _assetOrder, _concatenator, _transformers)
					                                : _bundle.GetContents(_fileSystem, _concatenator, _transformers);

				_contents = bundleContents.Contents;
				_hash = bundleContents.Hash;
			}
		}

		private void WatcherChanged(object sender, FileSystemEventArgs e)
		{
			lock (_watchTimerLockObject)
			{
				_watchTimer.Stop();
				_watchTimer.Start();
			}
		}

		private void WatchTimerElapsed(object sender, EventArgs e)
		{
			lock (_watchTimerLockObject)
			{
				RefreshContents();

				BundleChangedDelegate handler = BundleChanged;

				if (handler != null)
				{
					handler();
				}
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

using Junior.Route.Assets.FileSystem;
using Junior.Route.Common;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Assets.FileSystem
{
	public static class BundleWatcherTester
	{
		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_path = Path.GetTempFileName();
				File.WriteAllText(_path, "contents");
				_bundle = new Bundle().File("file1");
				_fileSystem = MockRepository.GenerateMock<IFileSystem>();
				_fileSystem.Stub(arg => arg.AbsolutePath("file1")).Return(_path);
				_fileSystem
					.Stub(arg => arg.OpenFile(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					.WhenCalled(arg => arg.ReturnValue = File.OpenRead(_path))
					.Return(null);
				_concatenator = MockRepository.GenerateMock<IAssetConcatenator>();
				_concatenator
					.Stub(arg => arg.Concatenate(Arg<IEnumerable<string>>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = ((IEnumerable<string>)arg.Arguments.First()).First())
					.Return(null);
				_transformer = MockRepository.GenerateMock<IAssetTransformer>();
				_transformer
					.Stub(arg => arg.Transform(Arg<string>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = arg.Arguments.First())
					.Return(null);
				_watcher = new BundleWatcher(_bundle, _fileSystem, _concatenator, _transformer);
			}

			[TearDown]
			public void TearDown()
			{
				File.Delete(_path);
			}

			private Bundle _bundle;
			private BundleWatcher _watcher;
			private IFileSystem _fileSystem;
			private IAssetConcatenator _concatenator;
			private IAssetTransformer _transformer;
			private string _path;

			[Test]
			public void Must_raise_event_when_file_changes()
			{
				bool changed = false;

				_watcher.BundleChanged += () => changed = true;

				File.WriteAllText(_path, "test");

				// BundleChanged event does not fire until the bundle has not changed for 500 ms
				Thread.Sleep(TimeSpan.FromSeconds(2));

				Assert.That(changed, Is.True);
			}

			[Test]
			public void Must_refresh_bundle_contents_when_bundle_changes()
			{
				File.WriteAllText(_path, "test");

				// Bundle is not refreshed until the bundle has not changed for 500 ms
				Thread.Sleep(TimeSpan.FromSeconds(2));

				Assert.That(_watcher.Contents, Is.EqualTo("test"));
			}

			[Test]
			public void Must_set_properties()
			{
				using (MD5 md5 = MD5.Create())
				{
					byte[] hashBytes = md5.ComputeHash(Encoding.ASCII.GetBytes("contents"));
					string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

					Assert.That(_watcher.Contents, Is.EqualTo("contents"));
					Assert.That(_watcher.Hash, Is.EqualTo(hash));
				}
			}
		}
	}
}
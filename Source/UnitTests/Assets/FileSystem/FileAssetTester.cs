using System.IO;
using System.Linq;
using System.Text;

using Junior.Route.Assets.FileSystem;
using Junior.Route.Common;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Assets.FileSystem
{
	public static class FileAssetTester
	{
		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_fileAsset = new FileAsset("file", Encoding.UTF8);
			}

			private FileAsset _fileAsset;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_fileAsset.Encoding, Is.SameAs(Encoding.UTF8));
				Assert.That(_fileAsset.RelativePath, Is.EqualTo("file"));
			}
		}

		[TestFixture]
		public class When_getting_filesystemwatcher
		{
			[SetUp]
			public void SetUp()
			{
				_fileAsset = new FileAsset("file", Encoding.UTF8);
				_fileSystem = MockRepository.GenerateMock<IFileSystem>();
				_tempPath = Path.GetTempFileName();
				_fileSystem.Stub(arg => arg.AbsolutePath("file")).Return(_tempPath);
				_watcher = _fileAsset.GetFileSystemWatcher(_fileSystem);
			}

			[TearDown]
			public void TearDown()
			{
				File.Delete(_tempPath);
			}

			private FileAsset _fileAsset;
			private IFileSystem _fileSystem;
			private FileSystemWatcher _watcher;
			private string _tempPath;

			[Test]
			public void Must_get_instance_with_correct_properties()
			{
				Assert.That(_watcher.IncludeSubdirectories, Is.False);
				Assert.That(_watcher.Path, Is.EqualTo(Path.GetDirectoryName(_tempPath)));
				Assert.That(_watcher.Filter, Is.EqualTo(Path.GetFileName(_tempPath)));
			}
		}

		[TestFixture]
		public class When_resolving_asset_files
		{
			[SetUp]
			public void SetUp()
			{
				_fileAsset = new FileAsset("file", Encoding.UTF8);
				_fileSystem = MockRepository.GenerateMock<IFileSystem>();
				_fileSystem.Stub(arg => arg.AbsolutePath("file")).Return("file");
				_resolveAssetFiles = _fileAsset.ResolveAssetFiles(_fileSystem).ToArray();
			}

			private FileAsset _fileAsset;
			private IFileSystem _fileSystem;
			private AssetFile[] _resolveAssetFiles;

			[Test]
			public void Must_resolve_correct_files()
			{
				Assert.That(_resolveAssetFiles, Has.Length.EqualTo(1));

				Assert.That(_resolveAssetFiles[0].AbsolutePath, Is.EqualTo("file"));
				Assert.That(_resolveAssetFiles[0].Encoding, Is.SameAs(Encoding.UTF8));
			}
		}
	}
}
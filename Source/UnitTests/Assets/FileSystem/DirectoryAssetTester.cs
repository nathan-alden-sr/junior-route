using System.IO;
using System.Linq;
using System.Text;

using Junior.Route.Assets.FileSystem;
using Junior.Route.Common;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Assets.FileSystem
{
	public static class DirectoryAssetTester
	{
		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_directoryAsset = new DirectoryAsset("directory", Encoding.UTF8, "*.txt", SearchOption.TopDirectoryOnly);
			}

			private DirectoryAsset _directoryAsset;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_directoryAsset.Encoding, Is.SameAs(Encoding.UTF8));
				Assert.That(_directoryAsset.RelativePath, Is.EqualTo("directory"));
			}
		}

		[TestFixture]
		public class When_getting_filesystemwatcher
		{
			[SetUp]
			public void SetUp()
			{
				_directoryAsset = new DirectoryAsset("directory", Encoding.UTF8, "*.txt", SearchOption.TopDirectoryOnly);
				_fileSystem = MockRepository.GenerateMock<IFileSystem>();
				_tempPath = Path.GetTempPath();
				_fileSystem.Stub(arg => arg.AbsolutePath("directory")).Return(_tempPath);
				_watcher = _directoryAsset.GetFileSystemWatcher(_fileSystem);
			}

			private DirectoryAsset _directoryAsset;
			private IFileSystem _fileSystem;
			private FileSystemWatcher _watcher;
			private string _tempPath;

			[Test]
			public void Must_get_instance_with_correct_properties()
			{
				Assert.That(_watcher.IncludeSubdirectories, Is.False);
				Assert.That(_watcher.Path, Is.EqualTo(_tempPath));
			}
		}

		[TestFixture]
		public class When_resolving_asset_files
		{
			[SetUp]
			public void SetUp()
			{
				_fileFilter = MockRepository.GenerateMock<IFileFilter>();
				_fileFilter.Stub(arg => arg.Filter("file1")).Return(FilterResult.Include);
				_fileFilter.Stub(arg => arg.Filter("file2")).Return(FilterResult.Exclude);
				_directoryAsset = new DirectoryAsset("directory", Encoding.UTF8, "*.txt", SearchOption.TopDirectoryOnly, _fileFilter);
				_fileSystem = MockRepository.GenerateMock<IFileSystem>();
				_fileSystem.Stub(arg => arg.AbsolutePath("directory")).Return("directory");
				_fileSystem.Stub(arg => arg.GetDirectoryFiles("directory", "*.txt", SearchOption.TopDirectoryOnly)).Return(new[] { "file1", "file2" });
				_resolveAssetFiles = _directoryAsset.ResolveAssetFiles(_fileSystem).ToArray();
			}

			private DirectoryAsset _directoryAsset;
			private IFileSystem _fileSystem;
			private AssetFile[] _resolveAssetFiles;
			private IFileFilter _fileFilter;

			[Test]
			public void Must_honor_filters()
			{
				_fileFilter.AssertWasCalled(arg => arg.Filter(Arg<string>.Is.Anything), options => options.Repeat.Twice());
			}

			[Test]
			public void Must_resolve_correct_files()
			{
				Assert.That(_resolveAssetFiles, Has.Length.EqualTo(1));

				Assert.That(_resolveAssetFiles[0].AbsolutePath, Is.EqualTo("file1"));
				Assert.That(_resolveAssetFiles[0].Encoding, Is.SameAs(Encoding.UTF8));
			}
		}
	}
}
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Junior.Route.Assets.FileSystem;
using Junior.Route.Common;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Assets.FileSystem
{
	public static class BundleTester
	{
		[TestFixture]
		public class When_adding_directory_assets
		{
			[SetUp]
			public void SetUp()
			{
				_bundle = new Bundle()
					.Directory("directory1", Encoding.UTF8)
					.Directory("directory2");
			}

			private Bundle _bundle;

			[Test]
			public void Must_add_assets()
			{
				IAsset[] assets = _bundle.Assets.ToArray();

				Assert.That(assets, Has.Length.EqualTo(2));

				Assert.That(assets[0].RelativePath, Is.EqualTo("directory1"));
				Assert.That(assets[0].Encoding, Is.SameAs(Encoding.UTF8));

				Assert.That(assets[1].RelativePath, Is.EqualTo("directory2"));
				Assert.That(assets[1].Encoding, Is.Null);
			}
		}

		[TestFixture]
		public class When_adding_file_assets
		{
			[SetUp]
			public void SetUp()
			{
				_bundle = new Bundle()
					.File("file1", Encoding.UTF8)
					.File("file2");
			}

			private Bundle _bundle;

			[Test]
			public void Must_add_assets()
			{
				IAsset[] assets = _bundle.Assets.ToArray();

				Assert.That(assets, Has.Length.EqualTo(2));

				Assert.That(assets[0].RelativePath, Is.EqualTo("file1"));
				Assert.That(assets[0].Encoding, Is.SameAs(Encoding.UTF8));

				Assert.That(assets[1].RelativePath, Is.EqualTo("file2"));
				Assert.That(assets[1].Encoding, Is.Null);
			}
		}

		[TestFixture]
		public class When_getting_contents
		{
			[SetUp]
			public void SetUp()
			{
				_bundle = new Bundle()
					.Directory("directory1")
					.File("file1");
				_fileSystem = MockRepository.GenerateMock<IFileSystem>();
				_fileSystem.Stub(arg => arg.AbsolutePath("directory1")).Return("directory1");
				_fileSystem.Stub(arg => arg.AbsolutePath("file1")).Return("file1");
				_fileSystem.Stub(arg => arg.AbsolutePath("file2")).Return("file2");
				_fileSystem.Stub(arg => arg.AbsolutePath("file3")).Return("file3");
				_fileSystem.Stub(arg => arg.GetDirectoryFiles("directory1", "*.*", SearchOption.AllDirectories)).Return(new[] { "file2", "file3" });
				_fileSystem
					.Stub(arg => arg.OpenFile(Arg<string>.Is.Anything, Arg<FileMode>.Is.Equal(FileMode.Open), Arg<FileAccess>.Is.Equal(FileAccess.Read), Arg<FileShare>.Is.Equal(FileShare.ReadWrite)))
					.WhenCalled(arg => arg.ReturnValue = new MemoryStream(Encoding.ASCII.GetBytes("ABC")))
					.Return(null);
				_comparer = MockRepository.GenerateMock<IComparer<AssetFile>>();
				_concatenator = MockRepository.GenerateMock<IAssetConcatenator>();
				_concatenator.Stub(arg => arg.Concatenate(Arg<IEnumerable<string>>.Is.Anything)).Return("");
				_transformer = MockRepository.GenerateMock<IAssetTransformer>();
				_transformer.Stub(arg => arg.Transform(Arg<string>.Is.Anything)).Return("");
				_bundle.GetContents(_fileSystem, _comparer, _concatenator, _transformer);
			}

			private Bundle _bundle;
			private IFileSystem _fileSystem;
			private IAssetConcatenator _concatenator;
			private IAssetTransformer _transformer;
			private IComparer<AssetFile> _comparer;

			[Test]
			public void Must_concatenate_results()
			{
				_concatenator.AssertWasCalled(arg => arg.Concatenate(Arg<IEnumerable<string>>.Is.Anything));
			}

			[Test]
			[TestCase("directory1")]
			public void Must_resolve_directory_absolute_path(string path)
			{
				_fileSystem.AssertWasCalled(arg => arg.AbsolutePath(path));
			}

			[Test]
			[TestCase("file1")]
			public void Must_resolve_file_absolute_paths(string path)
			{
				_fileSystem.AssertWasCalled(arg => arg.AbsolutePath(path));
			}

			[Test]
			[TestCase("file1")]
			[TestCase("file2")]
			[TestCase("file3")]
			public void Must_retrieve_file_contents(string path)
			{
				_fileSystem.AssertWasCalled(arg => arg.OpenFile(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
			}

			[Test]
			[TestCase("directory1")]
			public void Must_retrieve_files_in_directory(string path)
			{
				_fileSystem.AssertWasCalled(arg => arg.GetDirectoryFiles(path, "*.*", SearchOption.AllDirectories));
			}

			[Test]
			public void Must_transform_results()
			{
				_transformer.AssertWasCalled(arg => arg.Transform(Arg<string>.Is.Anything));
			}

			[Test]
			public void Must_use_asset_comparer()
			{
				_comparer.AssertWasCalled(arg => arg.Compare(Arg<AssetFile>.Is.Anything, Arg<AssetFile>.Is.Anything));
			}
		}
	}
}
using System.Text;

using Junior.Route.Assets.FileSystem;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Assets.FileSystem
{
	public static class AssetFileTester
	{
		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_assetFile = new AssetFile("path", Encoding.UTF8);
			}

			private AssetFile _assetFile;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_assetFile.AbsolutePath, Is.EqualTo("path"));
				Assert.That(_assetFile.Encoding, Is.SameAs(Encoding.UTF8));
			}
		}
	}
}
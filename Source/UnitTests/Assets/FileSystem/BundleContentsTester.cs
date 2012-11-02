using System;
using System.Security.Cryptography;
using System.Text;

using Junior.Route.Assets.FileSystem;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Assets.FileSystem
{
	public static class BundleContentsTester
	{
		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_bundleContents = new BundleContents("contents");
			}

			private BundleContents _bundleContents;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_bundleContents.Contents, Is.EqualTo("contents"));

				using (MD5 md5 = MD5.Create())
				{
					byte[] hashBytes = md5.ComputeHash(Encoding.ASCII.GetBytes("contents"));
					string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

					Assert.That(_bundleContents.Hash, Is.EqualTo(hash));
				}
			}
		}
	}
}
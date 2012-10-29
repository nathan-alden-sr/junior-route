using System;
using System.Security.Cryptography;
using System.Text;

using Junior.Common;

namespace Junior.Route.Assets.FileSystem
{
	public class BundleContents
	{
		private readonly string _contents;
		private readonly string _hash;

		public BundleContents(string contents)
		{
			contents.ThrowIfNull("contents");

			_contents = contents;
			using (MD5 md5 = MD5.Create())
			{
				byte[] buffer = Encoding.UTF8.GetBytes(contents);
				byte[] hash = md5.ComputeHash(buffer);

				_hash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
			}
		}

		public string Contents
		{
			get
			{
				return _contents;
			}
		}

		public string Hash
		{
			get
			{
				return _hash;
			}
		}
	}
}
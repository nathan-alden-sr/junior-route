using System.Text;

using Junior.Common;

namespace Junior.Route.Assets.FileSystem
{
	public class AssetFile
	{
		private readonly string _absolutePath;
		private readonly Encoding _encoding;

		public AssetFile(string absolutePath, Encoding encoding = null)
		{
			absolutePath.ThrowIfNull("absolutePath");

			_absolutePath = absolutePath;
			_encoding = encoding;
		}

		public string AbsolutePath
		{
			get
			{
				return _absolutePath;
			}
		}

		public Encoding Encoding
		{
			get
			{
				return _encoding;
			}
		}
	}
}
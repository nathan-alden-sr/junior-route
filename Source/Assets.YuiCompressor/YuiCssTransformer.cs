using Junior.Common;
using Junior.Route.Assets.FileSystem;

using Yahoo.Yui.Compressor;

namespace Assets.YuiCompressor
{
	public class YuiCssTransformer : IAssetTransformer
	{
		private readonly CssCompressor _compressor;

		public YuiCssTransformer(CompressionType compressionType = CompressionType.Standard, int lineBreakPosition = -1, bool removeComments = true)
		{
			_compressor = new CssCompressor
				{
					CompressionType = compressionType,
					LineBreakPosition = lineBreakPosition,
					RemoveComments = removeComments
				};
		}

		public string Transform(string assetContents)
		{
			assetContents.ThrowIfNull("assetContents");

			return _compressor.Compress(assetContents);
		}
	}
}
using System.Text;

using Junior.Common;
using Junior.Route.Assets.FileSystem;

using Yahoo.Yui.Compressor;

namespace Junior.Route.Assets.YuiCompressor
{
	public class YuiJavaScriptTransformer : IAssetTransformer
	{
		private readonly JavaScriptCompressor _compressor;

		public YuiJavaScriptTransformer(
			CompressionType compressionType = CompressionType.Standard,
			bool enableOptimizations = true,
			bool ignoreEval = false,
			int lineBreakPosition = -1,
			bool obfuscateJavaScript = true,
			bool preserveAllSemiColons = false)
		{
			_compressor = new JavaScriptCompressor
			{
				CompressionType = compressionType,
				DisableOptimizations = !enableOptimizations,
				IgnoreEval = ignoreEval,
				LineBreakPosition = lineBreakPosition,
				ObfuscateJavascript = obfuscateJavaScript,
				PreserveAllSemicolons = preserveAllSemiColons
			};
		}

		public YuiJavaScriptTransformer(
			Encoding encoding,
			CompressionType compressionType = CompressionType.Standard,
			bool enableOptimizations = true,
			bool ignoreEval = false,
			int lineBreakPosition = -1,
			bool obfuscateJavaScript = true,
			bool preserveAllSemiColons = false)
		{
			_compressor = new JavaScriptCompressor
			{
				CompressionType = compressionType,
				DisableOptimizations = !enableOptimizations,
				Encoding = encoding,
				IgnoreEval = ignoreEval,
				LineBreakPosition = lineBreakPosition,
				ObfuscateJavascript = obfuscateJavaScript,
				PreserveAllSemicolons = preserveAllSemiColons
			};
		}

		public string Transform(string assetContents)
		{
			assetContents.ThrowIfNull("assetContents");

			return _compressor.Compress(assetContents);
		}
	}
}
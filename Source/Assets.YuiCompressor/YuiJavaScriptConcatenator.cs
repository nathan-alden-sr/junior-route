using System;
using System.Collections.Generic;

using Junior.Route.Assets.FileSystem;

namespace Assets.YuiCompressor
{
	public class YuiJavaScriptConcatenator : IAssetConcatenator
	{
		private const string Delimiter = "";

		public string Concatenate(IEnumerable<string> assetContents)
		{
			return String.Join(Delimiter, assetContents);
		}
	}
}
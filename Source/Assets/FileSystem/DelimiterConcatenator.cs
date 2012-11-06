using System;
using System.Collections.Generic;

using Junior.Common;

namespace Junior.Route.Assets.FileSystem
{
	public class DelimiterConcatenator : IAssetConcatenator
	{
		private readonly string _delimiter;

		public DelimiterConcatenator(string delimiter = "")
		{
			delimiter.ThrowIfNull("delimiter");

			_delimiter = delimiter;
		}

		public string Concatenate(IEnumerable<string> assetContents)
		{
			return String.Join(_delimiter, assetContents);
		}
	}
}
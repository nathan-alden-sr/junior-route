using System.Collections.Generic;

namespace Junior.Route.Assets.FileSystem
{
	public interface IAssetConcatenator
	{
		string Concatenate(IEnumerable<string> assetContents);
	}
}
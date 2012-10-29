namespace Junior.Route.Assets.FileSystem
{
	public interface IAssetTransformer
	{
		string Transform(string assetContents);
	}
}
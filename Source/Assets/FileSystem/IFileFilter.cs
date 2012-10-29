namespace Junior.Route.Assets.FileSystem
{
	public interface IFileFilter
	{
		FilterResult Filter(string path);
	}
}
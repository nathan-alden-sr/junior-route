namespace Junior.Route.Routing.RelativeUrlResolvers
{
	public interface IRelativeUrlResolver
	{
		ResolveResult Resolve(params object[] args);
	}
}
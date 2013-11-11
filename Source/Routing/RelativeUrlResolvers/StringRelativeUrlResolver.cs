using Junior.Common;

namespace Junior.Route.Routing.RelativeUrlResolvers
{
	public class StringRelativeUrlResolver : IRelativeUrlResolver
	{
		private readonly string _relativeUrl;

		public StringRelativeUrlResolver(string relativeUrl)
		{
			relativeUrl.ThrowIfNull("relativeUrl");

			_relativeUrl = relativeUrl;
		}

		public ResolveResult Resolve(params object[] args)
		{
			return ResolveResult.UrlResolved(_relativeUrl);
		}
	}
}
using Junior.Route.Routing.Responses;

namespace Junior.Route.ViewEngines.RazorEngine
{
	public abstract class RazorHtmlResponseBase : ImmutableResponse
	{
		protected static readonly object LockObject = new object();

		protected RazorHtmlResponseBase(Response response)
			: base(response)
		{
		}
	}
}
using Junior.Common;
using Junior.Route.Routing;

namespace Junior.Route.AspNetIntegration.ResponseGenerators
{
	public class RouteMatchResult
	{
		private readonly MatchResult _matchResult;
		private readonly Routing.Route _route;

		public RouteMatchResult(Routing.Route route, MatchResult result)
		{
			route.ThrowIfNull("route");
			result.ThrowIfNull("result");

			_route = route;
			_matchResult = result;
		}

		public Routing.Route Route
		{
			get
			{
				return _route;
			}
		}

		public MatchResult MatchResult
		{
			get
			{
				return _matchResult;
			}
		}
	}
}
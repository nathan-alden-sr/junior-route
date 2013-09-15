using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.Routing;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.ResponseGenerators
{
	public class MostMatchingRestrictionsGenerator : IResponseGenerator
	{
		public async Task<ResponseResult> GetResponseAsync(HttpContextBase context, IEnumerable<RouteMatchResult> routeMatchResults)
		{
			context.ThrowIfNull("context");
			routeMatchResults.ThrowIfNull("routeMatchResults");

			routeMatchResults = routeMatchResults.ToArray();

			RouteMatchResult[] matchedResults = routeMatchResults.Where(arg => arg.MatchResult.ResultType == MatchResultType.RouteMatched).ToArray();

			if (!matchedResults.Any())
			{
				return ResponseResult.ResponseNotGenerated();
			}

			int maximumMatchedRestrictions = matchedResults.Max(arg => arg.MatchResult.MatchedRestrictions.Count());
			RouteMatchResult[] bestMatches = matchedResults.Where(arg => arg.MatchResult.MatchedRestrictions.CountEqual(maximumMatchedRestrictions)).ToArray();

			if (bestMatches.Length > 1)
			{
				return ResponseResult.ResponseGenerated(new Response().MultipleChoices());
			}
			if (bestMatches.Length == 0)
			{
				return ResponseResult.ResponseNotGenerated();
			}

			RouteMatchResult bestMatch = bestMatches[0];
			AuthenticateResult authenticateResult = await bestMatch.Route.AuthenticateAsync(context.Request, context.Response);

			if (authenticateResult.ResultType == AuthenticateResultType.AuthenticationFailed)
			{
				return ResponseResult.ResponseGenerated(authenticateResult.FailedResponse ?? new Response().Unauthorized());
			}

			Task<IResponse> responseTask = bestMatch.Route.ProcessResponseAsync(context);

			return ResponseResult.ResponseGenerated(responseTask, bestMatch.MatchResult.CacheKey);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Junior.Common;
using Junior.Route.Routing;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.ResponseGenerators
{
	public class MostMatchingRestrictionsGenerator : IResponseGenerator
	{
		public ResponseResult GetResponse(HttpRequestBase request, IEnumerable<RouteMatchResult> routeMatchResults)
		{
			request.ThrowIfNull("request");
			routeMatchResults.ThrowIfNull("routeMatchResults");

			routeMatchResults = routeMatchResults.ToArray();

			RouteMatchResult[] matchedResults = routeMatchResults.Where(arg => arg.MatchResult.ResultType == MatchResultType.RouteMatched).ToArray();

			if (matchedResults.Any())
			{
				int maximumMatchedRestrictions = matchedResults.Max(arg => arg.MatchResult.MatchedRestrictions.Count());
				RouteMatchResult[] bestMatches = matchedResults.Where(arg => arg.MatchResult.MatchedRestrictions.CountEqual(maximumMatchedRestrictions)).ToArray();

				if (bestMatches.Length > 1)
				{
					return ResponseResult.NonCachedResponse(Response.MultipleChoices());
				}
				if (bestMatches.Length == 1)
				{
					RouteMatchResult bestMatch = bestMatches[0];
					IResponse response = bestMatch.Route.ProcessResponse(request);

					if (response == null)
					{
						throw new ApplicationException("A matching route was found but it returned a null response.");
					}

					return ResponseResult.CachedResponse(response, bestMatch.MatchResult.CacheKey);
				}
			}

			return ResponseResult.NoResponse();
		}
	}
}
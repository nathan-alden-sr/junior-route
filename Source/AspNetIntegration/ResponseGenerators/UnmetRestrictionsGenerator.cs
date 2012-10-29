using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Junior.Common;
using Junior.Route.Http.RequestHeaders;
using Junior.Route.Routing;
using Junior.Route.Routing.Responses;
using Junior.Route.Routing.Restrictions;

namespace Junior.Route.AspNetIntegration.ResponseGenerators
{
	public class UnmetRestrictionsGenerator : IResponseGenerator
	{
		public ResponseResult GetResponse(HttpRequestBase request, IEnumerable<RouteMatchResult> routeMatchResults)
		{
			request.ThrowIfNull("request");
			routeMatchResults.ThrowIfNull("routeMatchResults");

			RouteMatchResult[] unmatchedResults = routeMatchResults.Where(arg => arg.MatchResult.ResultType == MatchResultType.RouteNotMatched).ToArray();

			if (unmatchedResults.Any())
			{
				RouteMatchResult[] unmatchedResultsThatMatchedOnRelativeUrl = unmatchedResults.Where(arg1 => RouteMatchedRelativeUrl(arg1.MatchResult)).ToArray();
				int minimumUnmatchedRestrictions = unmatchedResultsThatMatchedOnRelativeUrl.Min(arg => arg.MatchResult.UnmatchedRestrictions.Count());
				RouteMatchResult[] closestMatches = unmatchedResultsThatMatchedOnRelativeUrl.Where(arg => arg.MatchResult.UnmatchedRestrictions.Count() == minimumUnmatchedRestrictions).ToArray();

				if (closestMatches.Length == 1)
				{
					RouteMatchResult closestMatch = closestMatches[0];
					IRouteRestriction[] unmatchedRestrictions = closestMatch.MatchResult.UnmatchedRestrictions.ToArray();
					MethodRestriction[] methodRestrictions = unmatchedRestrictions.OfType<MethodRestriction>().ToArray();

					if (methodRestrictions.Any())
					{
						IEnumerable<string> methods = methodRestrictions
							.Select(arg => arg.Method)
							.Distinct(StringComparer.OrdinalIgnoreCase)
							.OrderBy(arg => arg);

						return ResponseResult.NonCachedResponse(Response.MethodNotAllowed().Header("Allow", String.Join(", ", methods)));
					}

					IEnumerable<HeaderRestriction<AcceptHeader>> acceptHeaderRestrictions = unmatchedRestrictions.OfType<HeaderRestriction<AcceptHeader>>();

					if (acceptHeaderRestrictions.Any())
					{
						return ResponseResult.NonCachedResponse(Response.NotAcceptable());
					}

					IEnumerable<HeaderRestriction<AcceptCharsetHeader>> acceptCharsetHeaderRestrictions = unmatchedRestrictions.OfType<HeaderRestriction<AcceptCharsetHeader>>();

					if (acceptCharsetHeaderRestrictions.Any())
					{
						return ResponseResult.NonCachedResponse(Response.NotAcceptable());
					}

					IEnumerable<HeaderRestriction<AcceptEncodingHeader>> acceptEncodingHeaderRestrictions = unmatchedRestrictions.OfType<HeaderRestriction<AcceptEncodingHeader>>();

					if (acceptEncodingHeaderRestrictions.Any())
					{
						return ResponseResult.NonCachedResponse(Response.NotAcceptable());
					}

					IEnumerable<HeaderRestriction<ContentEncodingHeader>> contentEncodingHeaderRestrictions = unmatchedRestrictions.OfType<HeaderRestriction<ContentEncodingHeader>>();

					if (contentEncodingHeaderRestrictions.Any())
					{
						return ResponseResult.NonCachedResponse(Response.UnsupportedMediaType());
					}
				}
			}

			return ResponseResult.NoResponse();
		}

		private static bool RouteMatchedRelativeUrl(MatchResult matchResult)
		{
			return
				matchResult.MatchedRestrictions != null &&
				matchResult.MatchedRestrictions.Any(arg2 => arg2 is RelativeUrlRestriction) &&
				matchResult.UnmatchedRestrictions.All(arg2 => !(arg2 is RelativeUrlRestriction));
		}
	}
}
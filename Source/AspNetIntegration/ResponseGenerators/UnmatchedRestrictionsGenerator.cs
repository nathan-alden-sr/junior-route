using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.Http.RequestHeaders;
using Junior.Route.Routing.Responses;
using Junior.Route.Routing.Restrictions;

using MatchResult = Junior.Route.Routing.MatchResult;
using MatchResultType = Junior.Route.Routing.MatchResultType;

namespace Junior.Route.AspNetIntegration.ResponseGenerators
{
	public class UnmatchedRestrictionsGenerator : IResponseGenerator
	{
		public Task<ResponseResult> GetResponseAsync(HttpContextBase context, IEnumerable<RouteMatchResult> routeMatchResults)
		{
			context.ThrowIfNull("context");
			routeMatchResults.ThrowIfNull("routeMatchResults");

			RouteMatchResult[] unmatchedResults = routeMatchResults.Where(arg => arg.MatchResult.ResultType == MatchResultType.RouteNotMatched).ToArray();

			if (!unmatchedResults.Any())
			{
				return ResponseResult.ResponseNotGenerated().AsCompletedTask();
			}

			RouteMatchResult[] unmatchedResultsThatMatchedOnUrlRelativePath = unmatchedResults.Where(arg1 => RouteMatchedUrlRelativePath(arg1.MatchResult)).ToArray();
			int minimumUnmatchedRestrictions = unmatchedResultsThatMatchedOnUrlRelativePath.Any() ? unmatchedResultsThatMatchedOnUrlRelativePath.Min(arg => arg.MatchResult.UnmatchedRestrictions.Count()) : 0;
			RouteMatchResult[] closestMatches = unmatchedResultsThatMatchedOnUrlRelativePath.Where(arg => arg.MatchResult.UnmatchedRestrictions.Count() == minimumUnmatchedRestrictions).ToArray();

			if (closestMatches.Length != 1)
			{
				return ResponseResult.ResponseNotGenerated().AsCompletedTask();
			}

			RouteMatchResult closestMatch = closestMatches[0];
			IRestriction[] unmatchedRestrictions = closestMatch.MatchResult.UnmatchedRestrictions.ToArray();
			MethodRestriction[] methodRestrictions = unmatchedRestrictions.OfType<MethodRestriction>().ToArray();

			if (methodRestrictions.Any())
			{
				IEnumerable<string> methods = methodRestrictions
					.Select(arg => arg.Method)
					.Distinct(StringComparer.OrdinalIgnoreCase)
					.OrderBy(arg => arg);

				return ResponseResult.ResponseGenerated(new Response().MethodNotAllowed().Header("Allow", String.Join(", ", methods))).AsCompletedTask();
			}
			if (unmatchedRestrictions.OfType<HeaderRestriction<AcceptHeader>>().Any())
			{
				return ResponseResult.ResponseGenerated(new Response().NotAcceptable()).AsCompletedTask();
			}
			if (unmatchedRestrictions.OfType<HeaderRestriction<AcceptCharsetHeader>>().Any())
			{
				return ResponseResult.ResponseGenerated(new Response().NotAcceptable()).AsCompletedTask();
			}
			if (unmatchedRestrictions.OfType<HeaderRestriction<AcceptEncodingHeader>>().Any())
			{
				return ResponseResult.ResponseGenerated(new Response().NotAcceptable()).AsCompletedTask();
			}
			if (unmatchedRestrictions.OfType<HeaderRestriction<ContentEncodingHeader>>().Any())
			{
				return ResponseResult.ResponseGenerated(new Response().UnsupportedMediaType()).AsCompletedTask();
			}

			return ResponseResult.ResponseNotGenerated().AsCompletedTask();
		}

		private static bool RouteMatchedUrlRelativePath(MatchResult matchResult)
		{
			return
				matchResult.MatchedRestrictions != null &&
				matchResult.MatchedRestrictions.Any(arg2 => arg2 is UrlRelativePathRestriction) &&
				matchResult.UnmatchedRestrictions.All(arg2 => !(arg2 is UrlRelativePathRestriction));
		}
	}
}
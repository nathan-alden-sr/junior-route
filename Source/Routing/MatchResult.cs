using System.Collections.Generic;

using Junior.Common;
using Junior.Route.Routing.Restrictions;

namespace Junior.Route.Routing
{
	public class MatchResult
	{
		private readonly string _cacheKey;
		private readonly IEnumerable<IRouteRestriction> _matchedRestrictions;
		private readonly MatchResultType _resultType;
		private readonly IEnumerable<IRouteRestriction> _unmatchedRestrictions;

		private MatchResult(MatchResultType resultType, IEnumerable<IRouteRestriction> matchedRestrictions, IEnumerable<IRouteRestriction> unmatchedRestrictions, string cacheKey)
		{
			_resultType = resultType;
			_matchedRestrictions = matchedRestrictions;
			_unmatchedRestrictions = unmatchedRestrictions;
			_cacheKey = cacheKey;
		}

		public MatchResultType ResultType
		{
			get
			{
				return _resultType;
			}
		}

		public IEnumerable<IRouteRestriction> MatchedRestrictions
		{
			get
			{
				return _matchedRestrictions;
			}
		}

		public IEnumerable<IRouteRestriction> UnmatchedRestrictions
		{
			get
			{
				return _unmatchedRestrictions;
			}
		}

		public string CacheKey
		{
			get
			{
				return _cacheKey;
			}
		}

		public static MatchResult RouteMatched(IEnumerable<IRouteRestriction> matchedRestrictions, string cacheKey)
		{
			matchedRestrictions.ThrowIfNull("matchedRestrictions");
			cacheKey.ThrowIfNull("cacheKey");

			return new MatchResult(MatchResultType.RouteMatched, matchedRestrictions, null, cacheKey);
		}

		public static MatchResult RouteNotMatched(IEnumerable<IRouteRestriction> matchedRestrictions, IEnumerable<IRouteRestriction> unmatchedRestrictions)
		{
			matchedRestrictions.ThrowIfNull("matchedRestrictions");
			unmatchedRestrictions.ThrowIfNull("unmatchedRestrictions");

			return new MatchResult(MatchResultType.RouteNotMatched, matchedRestrictions, unmatchedRestrictions, null);
		}
	}
}
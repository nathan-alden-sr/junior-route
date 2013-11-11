using System.Collections.Generic;
using System.Linq;

using Junior.Common;
using Junior.Route.Routing.Restrictions;

namespace Junior.Route.Routing
{
	public class MatchResult
	{
		private readonly string _cacheKey;
		private readonly IEnumerable<IRestriction> _matchedRestrictions;
		private readonly MatchResultType _resultType;
		private readonly IEnumerable<IRestriction> _unmatchedRestrictions;

		private MatchResult(MatchResultType resultType, IEnumerable<IRestriction> matchedRestrictions, IEnumerable<IRestriction> unmatchedRestrictions, string cacheKey = null)
		{
			_resultType = resultType;
			_unmatchedRestrictions = unmatchedRestrictions.ToArray();
			_matchedRestrictions = matchedRestrictions.ToArray();
			_cacheKey = cacheKey;
		}

		public MatchResultType ResultType
		{
			get
			{
				return _resultType;
			}
		}

		public IEnumerable<IRestriction> MatchedRestrictions
		{
			get
			{
				return _matchedRestrictions;
			}
		}

		public IEnumerable<IRestriction> UnmatchedRestrictions
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

		public static MatchResult RouteMatched(IEnumerable<IRestriction> matchedRestrictions, string cacheKey)
		{
			matchedRestrictions.ThrowIfNull("matchedRestrictions");
			cacheKey.ThrowIfNull("cacheKey");

			return new MatchResult(MatchResultType.RouteMatched, matchedRestrictions, Enumerable.Empty<IRestriction>(), cacheKey);
		}

		public static MatchResult RouteNotMatched(IEnumerable<IRestriction> matchedRestrictions, IEnumerable<IRestriction> unmatchedRestrictions)
		{
			matchedRestrictions.ThrowIfNull("matchedRestrictions");
			unmatchedRestrictions.ThrowIfNull("unmatchedRestrictions");

			return new MatchResult(MatchResultType.RouteNotMatched, matchedRestrictions, unmatchedRestrictions);
		}
	}
}
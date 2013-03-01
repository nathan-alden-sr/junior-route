using System.Collections.Generic;

using Junior.Common;
using Junior.Route.Routing.Restrictions;

namespace Junior.Route.Routing
{
	public class MatchResult
	{
		private readonly string _cacheKey;
		private readonly HashSet<IRestriction> _matchedRestrictions;
		private readonly MatchResultType _resultType;
		private readonly HashSet<IRestriction> _unmatchedRestrictions;

		private MatchResult(MatchResultType resultType, IEnumerable<IRestriction> matchedRestrictions, IEnumerable<IRestriction> unmatchedRestrictions, string cacheKey)
		{
			_resultType = resultType;
			_matchedRestrictions = new HashSet<IRestriction>(matchedRestrictions ?? new IRestriction[0]);
			_unmatchedRestrictions = new HashSet<IRestriction>(unmatchedRestrictions ?? new IRestriction[0]);
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

			return new MatchResult(MatchResultType.RouteMatched, matchedRestrictions, null, cacheKey);
		}

		public static MatchResult RouteNotMatched(IEnumerable<IRestriction> matchedRestrictions, IEnumerable<IRestriction> unmatchedRestrictions)
		{
			matchedRestrictions.ThrowIfNull("matchedRestrictions");
			unmatchedRestrictions.ThrowIfNull("unmatchedRestrictions");

			return new MatchResult(MatchResultType.RouteNotMatched, matchedRestrictions, unmatchedRestrictions, null);
		}
	}
}
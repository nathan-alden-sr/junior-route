using System.Collections.Generic;
using System.Linq;

using Junior.Common;

namespace Junior.Route.Routing.Restrictions
{
	public class MatchResult
	{
		private readonly IEnumerable<IRestriction> _matchedRestrictions;
		private readonly MatchResultType _resultType;
		private readonly IEnumerable<IRestriction> _unmatchedRestrictions;

		private MatchResult(MatchResultType resultType, IEnumerable<IRestriction> matchedRestrictions, IEnumerable<IRestriction> unmatchedRestrictions)
		{
			_resultType = resultType;
			_matchedRestrictions = matchedRestrictions.ToArray();
			_unmatchedRestrictions = unmatchedRestrictions.ToArray();
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

		public static MatchResult RestrictionMatched(IEnumerable<IRestriction> matchedRestrictions)
		{
			matchedRestrictions.ThrowIfNull("matchedRestrictions");

			return new MatchResult(MatchResultType.RestrictionMatched, matchedRestrictions, Enumerable.Empty<IRestriction>());
		}

		public static MatchResult RestrictionNotMatched(IEnumerable<IRestriction> matchedRestrictions, IEnumerable<IRestriction> unmatchedRestrictions)
		{
			matchedRestrictions.ThrowIfNull("matchedRestrictions");
			unmatchedRestrictions.ThrowIfNull("unmatchedRestrictions");

			return new MatchResult(MatchResultType.RestrictionNotMatched, matchedRestrictions, unmatchedRestrictions);
		}
	}
}
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Junior.Common;

namespace Junior.Route.Routing.Restrictions
{
	public class OrRestriction : IRestriction
	{
		private readonly IRestriction[] _restrictions;

		public OrRestriction(IEnumerable<IRestriction> restrictions)
		{
			restrictions.ThrowIfNull("restrictions");

			_restrictions = restrictions.ToArray();
		}

		public OrRestriction(params IRestriction[] restrictions)
			: this((IEnumerable<IRestriction>)restrictions)
		{
		}

		public MatchResult MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			MatchResult[] matchResults = _restrictions.Select(arg => arg.MatchesRequest(request)).ToArray();
			MatchResult matchingMatchResult = matchResults.FirstOrDefault(arg => arg.ResultType == MatchResultType.RestrictionMatched);

			return matchingMatchResult != null
				? MatchResult.RestrictionMatched(matchingMatchResult.MatchedRestrictions)
				: MatchResult.RestrictionNotMatched(Enumerable.Empty<IRestriction>(), matchResults.SelectMany(arg => arg.UnmatchedRestrictions));
		}
	}
}
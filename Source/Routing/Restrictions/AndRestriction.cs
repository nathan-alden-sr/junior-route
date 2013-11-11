using System.Collections.Generic;
using System.Linq;
using System.Web;

using Junior.Common;

namespace Junior.Route.Routing.Restrictions
{
	public class AndRestriction : IRestriction
	{
		private readonly IRestriction[] _restrictions;

		public AndRestriction(IEnumerable<IRestriction> restrictions)
		{
			restrictions.ThrowIfNull("restrictions");

			_restrictions = restrictions.ToArray();
		}

		public AndRestriction(params IRestriction[] restrictions)
			: this((IEnumerable<IRestriction>)restrictions)
		{
		}

		public MatchResult MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			MatchResult[] matchResults = _restrictions.Select(arg => arg.MatchesRequest(request)).ToArray();

			return matchResults.All(arg => arg.ResultType == MatchResultType.RestrictionMatched)
				? MatchResult.RestrictionMatched(matchResults.SelectMany(arg => arg.MatchedRestrictions))
				: MatchResult.RestrictionNotMatched(matchResults.SelectMany(arg => arg.MatchedRestrictions), matchResults.SelectMany(arg => arg.UnmatchedRestrictions));
		}
	}
}
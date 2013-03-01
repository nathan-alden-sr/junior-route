using System.Linq;

using Junior.Route.Routing;
using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Routing
{
	public static class MatchResultTester
	{
		[TestFixture]
		public class When_creating_matched_instance
		{
			[SetUp]
			public void SetUp()
			{
				_matchedRestrictions = new[]
					{
						new MethodRestriction("GET"),
						new MethodRestriction("POST")
					};
				_cacheKey = "id";
				_matchResult = MatchResult.RouteMatched(_matchedRestrictions, _cacheKey);
			}

			private MatchResult _matchResult;
			private MethodRestriction[] _matchedRestrictions;
			private string _cacheKey;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_matchResult.UnmatchedRestrictions.Any(), Is.False);
				Assert.That(_matchResult.CacheKey, Is.EqualTo(_cacheKey));
				Assert.That(_matchResult.MatchedRestrictions, Is.EquivalentTo(_matchedRestrictions));
				Assert.That(_matchResult.ResultType, Is.EqualTo(MatchResultType.RouteMatched));
			}
		}

		[TestFixture]
		public class When_creating_not_matched_instance
		{
			[SetUp]
			public void SetUp()
			{
				_matchedRestrictions = new[]
					{
						new MethodRestriction("GET"),
						new MethodRestriction("POST")
					};
				_unmatchedRestrictions = new[]
					{
						new UrlHostRestriction("host1", CaseInsensitivePlainComparer.Instance),
						new UrlHostRestriction("host2", CaseInsensitivePlainComparer.Instance)
					};
				_matchResult = MatchResult.RouteNotMatched(_matchedRestrictions, _unmatchedRestrictions);
			}

			private MatchResult _matchResult;
			private MethodRestriction[] _matchedRestrictions;
			private UrlHostRestriction[] _unmatchedRestrictions;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_matchResult.CacheKey, Is.Null);
				Assert.That(_matchResult.MatchedRestrictions, Is.EquivalentTo(_matchedRestrictions));
				Assert.That(_matchResult.ResultType, Is.EqualTo(MatchResultType.RouteNotMatched));
				Assert.That(_matchResult.UnmatchedRestrictions, Is.EquivalentTo(_unmatchedRestrictions));
			}
		}
	}
}
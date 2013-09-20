using System;
using System.Linq;

using Junior.Route.AspNetIntegration.ResponseGenerators;
using Junior.Route.Common;
using Junior.Route.Routing;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AspNetIntegration.ResponseGenerators
{
	public static class RouteMatchResultTester
	{
		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_route = new Route.Routing.Route((string)"name", Guid.NewGuid(), (Scheme)Scheme.NotSpecified, (string)"relative");
				_matchResult = MatchResult.RouteMatched(Enumerable.Empty<IRestriction>(), "key");
				_result = new RouteMatchResult(_route, _matchResult);
			}

			private Route.Routing.Route _route;
			private RouteMatchResult _result;
			private MatchResult _matchResult;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.MatchResult, Is.SameAs(_matchResult));
				Assert.That(_result.Route, Is.SameAs(_route));
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;

using Junior.Route.AspNetIntegration.ResponseGenerators;
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
				_route = new Route.Routing.Route("name", Guid.NewGuid(), "relative");
				_result = new RouteMatchResult(_route, MatchResult.RouteMatched(Enumerable.Empty<IRestriction>(), "key"));
			}

			private Route.Routing.Route _route;
			private RouteMatchResult _result;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.MatchResult.MatchedRestrictions, Is.Empty);
				Assert.That(_result.Route, Is.SameAs(_route));
			}
		}
	}
}
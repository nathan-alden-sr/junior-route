using Junior.Route.AutoRouting.AuthenticationStrategies;
using Junior.Route.AutoRouting.AuthenticationStrategies.Attributes;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.AuthenticationStrategies
{
	public static class AuthenticateAttributeStrategyTester
	{
		[TestFixture]
		public class When_determining_if_method_with_authenticateattribute_must_authenticate
		{
			[SetUp]
			public void SetUp()
			{
				_strategy = new AuthenticateAttributeStrategy();
			}

			private AuthenticateAttributeStrategy _strategy;

			public class Endpoint
			{
				[Authenticate]
				public void Method()
				{
				}
			}

			[Test]
			public void Must_return_true()
			{
				Assert.That(_strategy.MustAuthenticate(typeof(Endpoint), typeof(Endpoint).GetMethod("Method")), Is.True);
			}
		}
	}
}
using Junior.Route.Routing.Responses;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Routing.Responses
{
	public static class CookieValueTester
	{
		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_cookieValue = new CookieValue("name", "value");
			}

			private CookieValue _cookieValue;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_cookieValue.Name, Is.EqualTo("name"));
				Assert.That(_cookieValue.Value, Is.EqualTo("value"));
			}
		}
	}
}
using Junior.Route.Routing.Responses;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Routing.Responses
{
	public static class HeaderTester
	{
		[TestFixture]
		public class When_cloning_instance
		{
			[SetUp]
			public void SetUp()
			{
				_header1 = new Header("field", "value");
				_header2 = _header1.Clone();
			}

			private Header _header1;
			private Header _header2;

			[Test]
			public void Must_create_equal_instance()
			{
				Assert.That(_header1.Field, Is.EqualTo(_header2.Field));
				Assert.That(_header1.Value, Is.EqualTo(_header2.Value));
			}
		}

		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_cookieValue = new Header("field", "value");
			}

			private Header _cookieValue;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_cookieValue.Field, Is.EqualTo("field"));
				Assert.That(_cookieValue.Value, Is.EqualTo("value"));
			}
		}
	}
}
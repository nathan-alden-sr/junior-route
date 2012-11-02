using Junior.Route.AutoRouting.NameMappers.Attributes;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.NameMappers.Attributes
{
	public static class NameAttributeTester
	{
		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_attribute = new NameAttribute("name");
			}

			private NameAttribute _attribute;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_attribute.Name, Is.EqualTo("name"));
			}
		}
	}
}
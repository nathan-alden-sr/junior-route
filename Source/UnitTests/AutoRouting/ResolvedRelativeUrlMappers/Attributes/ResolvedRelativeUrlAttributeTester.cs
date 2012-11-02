using Junior.Route.AutoRouting.ResolvedRelativeUrlMappers.Attributes;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.ResolvedRelativeUrlMappers.Attributes
{
	public static class ResolvedRelativeUrlAttributeTester
	{
		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_attribute = new ResolvedRelativeUrlAttribute("relative");
			}

			private ResolvedRelativeUrlAttribute _attribute;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_attribute.ResolvedRelativeUrl, Is.EqualTo("relative"));
			}
		}
	}
}
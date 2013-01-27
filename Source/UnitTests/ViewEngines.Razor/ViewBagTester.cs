using Junior.Route.ViewEngines.Razor;

using NUnit.Framework;

namespace Junior.Route.UnitTests.ViewEngines.Razor
{
	public static class ViewBagTester
	{
		[TestFixture]
		public class When_accessing_dynamic_properties
		{
			[SetUp]
			public void SetUp()
			{
				_viewBag = new ViewBag();
			}

			private dynamic _viewBag;

			[Test]
			public void Must_get_and_set_values()
			{
				_viewBag.Test = "Foo";

				Assert.That(_viewBag.Test, Is.EqualTo("Foo"));
			}
		}

		[TestFixture]
		public class When_getting_dynamic_member_names
		{
			[SetUp]
			public void SetUp()
			{
				_viewBag = new ViewBag();
			}

			private dynamic _viewBag;

			[Test]
			public void Must_get_names_that_have_been_set()
			{
				_viewBag.Test1 = "Foo";
				_viewBag.Test2 = "Bar";

				Assert.That(_viewBag.GetDynamicMemberNames(), Is.EquivalentTo(new[] { "Test1", "Test2" }));
			}
		}
	}
}
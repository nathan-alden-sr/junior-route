using System.Web;

using Junior.Route.AutoRouting;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting
{
	public static class TypeExtensionsTester
	{
		[TestFixture]
		public class When_getting_default_value_of_reference_type
		{
			[Test]
			public void Must_return_null()
			{
				Assert.That(typeof(string).GetDefaultValue(), Is.Null);
			}
		}

		[TestFixture]
		public class When_getting_default_value_of_value_type
		{
			[Test]
			public void Must_return_default_value_of_type()
			{
				Assert.That(typeof(int).GetDefaultValue(), Is.EqualTo(default(int)));
			}
		}

		[TestFixture]
		public class When_testing_if_namespace_starts_with_string_and_it_does
		{
			[Test]
			[TestCase("System")]
			[TestCase("System.Web")]
			public void Must_return_true(string prefix)
			{
				Assert.That(typeof(HttpRequestBase).NamespaceStartsWith(prefix), Is.True);
			}
		}

		[TestFixture]
		public class When_testing_if_namespace_starts_with_string_and_it_does_not
		{
			[Test]
			[TestCase("Junior")]
			[TestCase("System.Web2")]
			[TestCase("System.")]
			[TestCase("System.Web.")]
			public void Must_return_true(string prefix)
			{
				Assert.That(typeof(HttpRequestBase).NamespaceStartsWith(prefix), Is.False);
			}
		}
	}
}
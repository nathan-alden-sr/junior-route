using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.RestrictionMappers;
using Junior.Route.AutoRouting.RestrictionMappers.Attributes;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.RestrictionMappers.Attributes
{
	public static class CustomRestrictionMapperAttributeTester
	{
		[TestFixture]
		public class When_constructing_instance_with_valid_type
		{
			public class Class : ICustomRestrictionMapper
			{
				public void Map(Routing.Route route, IContainer container)
				{
				}
			}

			[Test]
			public void Must_not_throw_exception()
			{
				Assert.That(() => new CustomRestrictionMapperAttribute(typeof(Class)), Throws.Nothing);
			}
		}

		[TestFixture]
		public class When_creating_instance_with_invalid_types
		{
			private class PrivateClass
			{
			}

			internal class InternalClass
			{
			}

			protected class ProtectedClass
			{
			}

			protected internal class ProtectedInternalClass
			{
			}

			public static class StaticClass
			{
			}

			public abstract class AbstractClass
			{
			}

			public class Class : ICustomRestrictionMapper
			{
				private Class()
				{
				}

				public void Map(Routing.Route route, IContainer container)
				{
				}
			}

			[Test]
			public void Must_throw_exceptions()
			{
				Assert.That(() => new CustomRestrictionMapperAttribute(typeof(PrivateClass)), Throws.ArgumentException);
				Assert.That(() => new CustomRestrictionMapperAttribute(typeof(InternalClass)), Throws.ArgumentException);
				Assert.That(() => new CustomRestrictionMapperAttribute(typeof(ProtectedClass)), Throws.ArgumentException);
				Assert.That(() => new CustomRestrictionMapperAttribute(typeof(ProtectedInternalClass)), Throws.ArgumentException);
				Assert.That(() => new CustomRestrictionMapperAttribute(typeof(StaticClass)), Throws.ArgumentException);
				Assert.That(() => new CustomRestrictionMapperAttribute(typeof(AbstractClass)), Throws.ArgumentException);
				Assert.That(() => new CustomRestrictionMapperAttribute(typeof(Class)), Throws.ArgumentException);
			}
		}
	}
}
using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.CustomMapperMappers;
using Junior.Route.AutoRouting.CustomMapperMappers.Attributes;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.CustomMappers.Attributes
{
	public static class CustomRelativeUrlResolverMapperAttributeTester
	{
		[TestFixture]
		public class When_constructing_instance_with_valid_type
		{
			public class Class : ICustomMapper
			{
				public void Map(Routing.Route route, IContainer container)
				{
				}
			}

			[Test]
			public void Must_not_throw_exception()
			{
				Assert.That(() => new CustomMapperAttribute(typeof(Class)), Throws.Nothing);
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

			public class Class : ICustomMapper
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
				Assert.That(() => new CustomMapperAttribute(typeof(PrivateClass)), Throws.ArgumentException);
				Assert.That(() => new CustomMapperAttribute(typeof(InternalClass)), Throws.ArgumentException);
				Assert.That(() => new CustomMapperAttribute(typeof(ProtectedClass)), Throws.ArgumentException);
				Assert.That(() => new CustomMapperAttribute(typeof(ProtectedInternalClass)), Throws.ArgumentException);
				Assert.That(() => new CustomMapperAttribute(typeof(StaticClass)), Throws.ArgumentException);
				Assert.That(() => new CustomMapperAttribute(typeof(AbstractClass)), Throws.ArgumentException);
				Assert.That(() => new CustomMapperAttribute(typeof(Class)), Throws.ArgumentException);
			}
		}
	}
}
using System;

using Junior.Route.ViewEngines.Razor;
using Junior.Route.ViewEngines.Razor.CompiledTemplateFactories;

using NUnit.Framework;

namespace Junior.Route.UnitTests.ViewEngines.Razor.CompiledTemplateFactories
{
	public static class ActivatorFactoryTester
	{
		[TestFixture]
		public class When_creating_instance_of_non_template_type
		{
			[SetUp]
			public void SetUp()
			{
				_factory = new ActivatorFactory();
			}

			private ActivatorFactory _factory;

			[Test]
			public void Must_throw_exception()
			{
				Assert.Throws<ArgumentException>(() => _factory.CreateFromType(typeof(object)));
			}
		}

		[TestFixture]
		public class When_creating_instance_of_template_type_with_model
		{
			[SetUp]
			public void SetUp()
			{
				_factory = new ActivatorFactory();
			}

			private ActivatorFactory _factory;

			private class FooTemplate : Template<string>
			{
			}

			[Test]
			public void Must_create_instance()
			{
				Assert.That(_factory.CreateFromType(typeof(FooTemplate)), Is.InstanceOf<FooTemplate>());
			}
		}

		[TestFixture]
		public class When_creating_instance_of_template_type_without_model
		{
			[SetUp]
			public void SetUp()
			{
				_factory = new ActivatorFactory();
			}

			private ActivatorFactory _factory;

			private class FooTemplate : Template
			{
			}

			[Test]
			public void Must_create_instance()
			{
				Assert.That(_factory.CreateFromType(typeof(FooTemplate)), Is.InstanceOf<FooTemplate>());
			}
		}
	}
}
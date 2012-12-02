using System;

using Junior.Common;
using Junior.Route.ViewEngines.Razor.TemplateClassNameBuilders;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.ViewEngines.Razor.TemplateClassNameBuilders
{
	public static class RandomGuidBuilderTester
	{
		[TestFixture]
		public class When_building_template_class_name
		{
			[SetUp]
			public void SetUp()
			{
				_guidFactory = MockRepository.GenerateMock<IGuidFactory>();
				_builder = new RandomGuidBuilder(_guidFactory);
			}

			private IGuidFactory _guidFactory;
			private RandomGuidBuilder _builder;

			[Test]
			public void Must_use_guidfactory_to_generate_random_class_name()
			{
				_builder.BuildFromRandomGuid();

				_guidFactory.AssertWasCalled(arg => arg.Random());
			}

			[Test]
			public void Must_use_random_guid_as_class_name()
			{
				_guidFactory.Stub(arg => arg.Random()).Return(Guid.Parse("a6ddb300-25e2-4c03-9aad-179bd4641f29"));

				Assert.That(_builder.BuildFromRandomGuid(), Is.EqualTo("_a6ddb30025e24c039aad179bd4641f29"));
			}
		}
	}
}
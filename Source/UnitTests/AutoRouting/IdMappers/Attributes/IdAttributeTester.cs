using System;

using Junior.Route.AutoRouting.IdMappers.Attributes;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.IdMappers.Attributes
{
	public static class IdAttributeTester
	{
		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_attribute = new IdAttribute("f75b5030-5826-421d-8ee1-4f94f8cdb3d8");
			}

			private IdAttribute _attribute;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_attribute.Id, Is.EqualTo(Guid.Parse("f75b5030-5826-421d-8ee1-4f94f8cdb3d8")));
			}
		}
	}
}
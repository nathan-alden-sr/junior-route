using System;

using Junior.Route.AutoRouting.IdMappers;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.IdMappers
{
	public static class IdResultTester
	{
		[TestFixture]
		public class When_creating_mapped_instance
		{
			[SetUp]
			public void SetUp()
			{
				_id = Guid.NewGuid();
				_result = IdResult.IdMapped(_id);
			}

			private IdResult _result;
			private Guid _id;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.Id, Is.EqualTo(_id));
				Assert.That(_result.ResultType, Is.EqualTo(IdResultType.IdMapped));
			}
		}

		[TestFixture]
		public class When_creating_not_mapped_instance
		{
			[SetUp]
			public void SetUp()
			{
				_result = IdResult.IdNotMapped();
			}

			private IdResult _result;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_result.Id, Is.Null);
				Assert.That(_result.ResultType, Is.EqualTo(IdResultType.IdNotMapped));
			}
		}
	}
}
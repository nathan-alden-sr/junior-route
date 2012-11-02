using System;

using Junior.Common;
using Junior.Route.AutoRouting.IdMappers;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.IdMappers
{
	public static class RandomIdMapperTester
	{
		[TestFixture]
		public class When_mapping_ids_randomly
		{
			[SetUp]
			public void SetUp()
			{
				_guidFactory = MockRepository.GenerateMock<IGuidFactory>();
				_guidFactory.Stub(arg => arg.Random()).Return(Guid.Parse("78d15a5e-723b-4b2f-974e-58f6852b6584")).Repeat.Once();
				_guidFactory.Stub(arg => arg.Random()).Return(Guid.Parse("e16a5bdb-bc1b-4364-a03c-1d1cffaa1b13")).Repeat.Once();
				_mapper = new RandomIdMapper(_guidFactory);
			}

			private RandomIdMapper _mapper;
			private IGuidFactory _guidFactory;

			public class Endpoint
			{
				public void Method1()
				{
				}

				public void Method2()
				{
				}
			}

			[Test]
			public void Must_map_ids_from_guidfactory_instance()
			{
				IdResult result1 = _mapper.Map(typeof(Endpoint), typeof(Endpoint).GetMethod("Method1"));
				IdResult result2 = _mapper.Map(typeof(Endpoint), typeof(Endpoint).GetMethod("Method2"));

				Assert.That(result1.Id, Is.EqualTo(Guid.Parse("78d15a5e-723b-4b2f-974e-58f6852b6584")));
				Assert.That(result1.ResultType, Is.EqualTo(IdResultType.IdMapped));

				Assert.That(result2.Id, Is.EqualTo(Guid.Parse("e16a5bdb-bc1b-4364-a03c-1d1cffaa1b13")));
				Assert.That(result2.ResultType, Is.EqualTo(IdResultType.IdMapped));
			}
		}
	}
}
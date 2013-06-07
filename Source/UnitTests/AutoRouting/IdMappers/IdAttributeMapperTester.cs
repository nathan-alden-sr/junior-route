using System;

using Junior.Route.AutoRouting.IdMappers;
using Junior.Route.AutoRouting.IdMappers.Attributes;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.IdMappers
{
	public static class IdAttributeMapperTester
	{
		[TestFixture]
		public class When_mapping_ids_from_idattributes
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new IdAttributeMapper();
			}

			private IdAttributeMapper _mapper;

			public class Endpoint
			{
				[Id("617658c6-4ea6-442c-8504-add11dde02ff")]
				public void Method()
				{
				}
			}

			[Test]
			public async void Must_use_id_from_attribute()
			{
				IdResult result = await _mapper.MapAsync(typeof(Endpoint), typeof(Endpoint).GetMethod("Method"));

				Assert.That(result.Id, Is.EqualTo(Guid.Parse("617658c6-4ea6-442c-8504-add11dde02ff")));
				Assert.That(result.ResultType, Is.EqualTo(IdResultType.IdMapped));
			}
		}
	}
}
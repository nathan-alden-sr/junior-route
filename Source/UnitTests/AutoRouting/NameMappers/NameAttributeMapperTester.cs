using Junior.Route.AutoRouting.NameMappers;
using Junior.Route.AutoRouting.NameMappers.Attributes;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.NameMappers
{
	public static class NameAttributeMapperTester
	{
		[TestFixture]
		public class When_mapping_names_from_nameattributes
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new NameAttributeMapper();
			}

			private NameAttributeMapper _mapper;

			public class Endpoint
			{
				[Name("name")]
				public void Method()
				{
				}
			}

			[Test]
			public async void Must_use_name_from_attribute()
			{
				NameResult result = await _mapper.MapAsync(typeof(Endpoint), typeof(Endpoint).GetMethod("Method"));

				Assert.That(result.Name, Is.EqualTo("name"));
				Assert.That(result.ResultType, Is.EqualTo(NameResultType.NameMapped));
			}
		}
	}
}
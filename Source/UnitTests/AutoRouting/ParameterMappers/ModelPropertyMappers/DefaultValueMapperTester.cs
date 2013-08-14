using System;
using System.Reflection;
using System.Web;

using Junior.Route.AutoRouting;
using Junior.Route.AutoRouting.ParameterMappers;

using NUnit.Framework;

using Rhino.Mocks;

using DefaultValueMapper = Junior.Route.AutoRouting.ParameterMappers.ModelPropertyMappers.DefaultValueMapper;

namespace Junior.Route.UnitTests.AutoRouting.ParameterMappers.ModelPropertyMappers
{
	public static class DefaultValueMapperTester
	{
		[TestFixture]
		public class When_mapping_default_values
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new DefaultValueMapper();
				_context = MockRepository.GenerateMock<HttpContextBase>();
			}

			private DefaultValueMapper _mapper;
			private HttpContextBase _context;

			public class Model
			{
				public string S
				{
					get;
					set;
				}

				public int I
				{
					get;
					set;
				}
			}

			[Test]
			[TestCase(typeof(string))]
			[TestCase(typeof(int))]
			[TestCase(typeof(object))]
			public async void Must_map_all_types(Type propertyType)
			{
				Assert.That(await _mapper.CanMapTypeAsync(_context, propertyType), Is.True);
			}

			[Test]
			[TestCase(typeof(Model), "S", null)]
			public async void Must_map_to_default_value_of_property_type(Type type, string propertyName, object expectedValue)
			{
				PropertyInfo propertyInfo = type.GetProperty(propertyName);
				MapResult result = await _mapper.MapAsync(_context, type, propertyInfo);

				Assert.That(result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(result.Value, Is.EqualTo(propertyInfo.PropertyType.GetDefaultValue()));
			}
		}
	}
}
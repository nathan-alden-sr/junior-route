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
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private DefaultValueMapper _mapper;
			private HttpRequestBase _request;

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
			public void Must_map_all_types(Type propertyType)
			{
				Assert.That(_mapper.CanMapType(propertyType), Is.True);
			}

			[Test]
			[TestCase(typeof(Model), "S", null)]
			public void Must_map_to_default_value_of_property_type(Type type, string propertyName, object expectedValue)
			{
				PropertyInfo propertyInfo = type.GetProperty(propertyName);
				MapResult result = _mapper.Map(_request, type, propertyInfo);

				Assert.That(result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(result.Value, Is.EqualTo(propertyInfo.PropertyType.GetDefaultValue()));
			}
		}
	}
}
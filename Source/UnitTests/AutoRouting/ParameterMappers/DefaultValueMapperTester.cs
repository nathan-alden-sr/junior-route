using System;
using System.Linq;
using System.Reflection;
using System.Web;

using Junior.Route.AutoRouting;
using Junior.Route.AutoRouting.ParameterMappers;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.ParameterMappers
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
				public void Method(string s)
				{
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
			[TestCase(typeof(Model), "Method", "s", null)]
			public async void Must_map_to_default_value_of_property_type(Type type, string methodName, string parameterName, object expectedValue)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);
				ParameterInfo parameterInfo = methodInfo.GetParameters().Single(arg => arg.Name == parameterName);
				MapResult result = await _mapper.MapAsync(_context, type, methodInfo, parameterInfo);

				Assert.That(result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(result.Value, Is.EqualTo(parameterInfo.ParameterType.GetDefaultValue()));
			}
		}
	}
}
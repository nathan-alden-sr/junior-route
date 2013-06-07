using System;
using System.Reflection;
using System.Web;

using Junior.Route.AutoRouting.ParameterMappers;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.ParameterMappers
{
	public static class HttpContextBaseMapperTester
	{
		[TestFixture]
		public class When_mapping_parameter
		{
			[SetUp]
			public void SetUp()
			{
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_mapper = new HttpContextBaseMapper();

				Type type = typeof(When_mapping_parameter);
				MethodInfo methodInfo = type.GetMethod("Method", BindingFlags.NonPublic | BindingFlags.Instance);
				ParameterInfo parameterInfo = methodInfo.GetParameters()[0];

				_result = _mapper.MapAsync(_context, type, methodInfo, parameterInfo).Result;
			}

			// ReSharper disable UnusedMember.Local
			private void Method(HttpContextBase context)
				// ReSharper restore UnusedMember.Local
			{
			}

			private HttpContextBaseMapper _mapper;
			private MapResult _result;
			private HttpContextBase _context;

			[Test]
			public void Must_use_context()
			{
				Assert.That(_result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(_result.Value, Is.SameAs(_context));
			}
		}

		[TestFixture]
		public class When_testing_if_mapper_can_map_parameter_type
		{
			[SetUp]
			public void SetUp()
			{
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_mapper = new HttpContextBaseMapper();
			}

			private HttpContextBaseMapper _mapper;
			private HttpContextBase _context;

			[Test]
			public async void Must_map_httpcontextbase_type()
			{
				Assert.That(await _mapper.CanMapTypeAsync(_context, typeof(HttpContextBase)), Is.True);
			}
		}
	}
}
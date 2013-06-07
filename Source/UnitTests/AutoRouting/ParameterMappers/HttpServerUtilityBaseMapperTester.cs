using System;
using System.Reflection;
using System.Web;

using Junior.Route.AutoRouting.ParameterMappers;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.ParameterMappers
{
	public static class HttpServerUtilityBaseMapperTester
	{
		[TestFixture]
		public class When_mapping_parameter
		{
			[SetUp]
			public void SetUp()
			{
				_server = MockRepository.GenerateMock<HttpServerUtilityBase>();
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Server).Return(_server);
				_mapper = new HttpServerUtilityBaseMapper();

				Type type = typeof(When_mapping_parameter);
				MethodInfo methodInfo = type.GetMethod("Method", BindingFlags.NonPublic | BindingFlags.Instance);
				ParameterInfo parameterInfo = methodInfo.GetParameters()[0];

				_result = _mapper.MapAsync(_context, type, methodInfo, parameterInfo).Result;
			}

			// ReSharper disable UnusedMember.Local
			private void Method(HttpServerUtilityBase context)
				// ReSharper restore UnusedMember.Local
			{
			}

			private HttpServerUtilityBaseMapper _mapper;
			private MapResult _result;
			private HttpServerUtilityBase _server;
			private HttpContextBase _context;

			[Test]
			public void Must_use_server_from_context()
			{
				Assert.That(_result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(_result.Value, Is.SameAs(_server));
			}
		}

		[TestFixture]
		public class When_testing_if_mapper_can_map_parameter_type
		{
			[SetUp]
			public void SetUp()
			{
				_server = MockRepository.GenerateMock<HttpServerUtilityBase>();
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Server).Return(_server);
				_mapper = new HttpServerUtilityBaseMapper();
			}

			private HttpServerUtilityBaseMapper _mapper;
			private HttpServerUtilityBase _server;
			private HttpContextBase _context;

			[Test]
			public async void Must_map_httpserverutilitybase_type()
			{
				Assert.That(await _mapper.CanMapTypeAsync(_context, typeof(HttpServerUtilityBase)), Is.True);
			}
		}
	}
}
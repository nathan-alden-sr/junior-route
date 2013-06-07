using System;
using System.Reflection;
using System.Web;

using Junior.Route.AutoRouting.ParameterMappers;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.ParameterMappers
{
	public static class HttpRequestBaseMapperTester
	{
		[TestFixture]
		public class When_mapping_parameter
		{
			[SetUp]
			public void SetUp()
			{
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Request).Return(_request);
				_mapper = new HttpRequestBaseMapper();

				Type type = typeof(When_mapping_parameter);
				MethodInfo methodInfo = type.GetMethod("Method", BindingFlags.NonPublic | BindingFlags.Instance);
				ParameterInfo parameterInfo = methodInfo.GetParameters()[0];

				_result = _mapper.MapAsync(_context, type, methodInfo, parameterInfo).Result;
			}

			// ReSharper disable UnusedMember.Local
			private void Method(HttpRequestBase context)
				// ReSharper restore UnusedMember.Local
			{
			}

			private HttpRequestBaseMapper _mapper;
			private MapResult _result;
			private HttpRequestBase _request;
			private HttpContextBase _context;

			[Test]
			public void Must_use_request_from_context()
			{
				Assert.That(_result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(_result.Value, Is.SameAs(_request));
			}
		}

		[TestFixture]
		public class When_testing_if_mapper_can_map_parameter_type
		{
			[SetUp]
			public void SetUp()
			{
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Request).Return(_request);
				_mapper = new HttpRequestBaseMapper();
			}

			private HttpRequestBaseMapper _mapper;
			private HttpRequestBase _request;
			private HttpContextBase _context;

			[Test]
			public async void Must_map_httprequestbase_type()
			{
				Assert.That(await _mapper.CanMapTypeAsync(_context, typeof(HttpRequestBase)), Is.True);
			}
		}
	}
}
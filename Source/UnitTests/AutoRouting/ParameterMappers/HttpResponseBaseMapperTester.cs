using System;
using System.Reflection;
using System.Web;

using Junior.Route.AutoRouting.ParameterMappers;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.ParameterMappers
{
	public static class HttpResponseBaseMapperTester
	{
		[TestFixture]
		public class When_mapping_parameter
		{
			[SetUp]
			public void SetUp()
			{
				_response = MockRepository.GenerateMock<HttpResponseBase>();
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Response).Return(_response);
				_mapper = new HttpResponseBaseMapper();

				Type type = typeof(When_mapping_parameter);
				MethodInfo methodInfo = type.GetMethod("Method", BindingFlags.NonPublic | BindingFlags.Instance);
				ParameterInfo parameterInfo = methodInfo.GetParameters()[0];

				_result = _mapper.Map(_context, type, methodInfo, parameterInfo);
			}

			// ReSharper disable UnusedMember.Local
			private void Method(HttpResponseBase context)
				// ReSharper restore UnusedMember.Local
			{
			}

			private HttpResponseBaseMapper _mapper;
			private MapResult _result;
			private HttpResponseBase _response;
			private HttpContextBase _context;

			[Test]
			public void Must_use_response_from_context()
			{
				Assert.That(_result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(_result.Value, Is.SameAs(_response));
			}
		}

		[TestFixture]
		public class When_testing_if_mapper_can_map_parameter_type
		{
			[SetUp]
			public void SetUp()
			{
				_response = MockRepository.GenerateMock<HttpResponseBase>();
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Response).Return(_response);
				_mapper = new HttpResponseBaseMapper();
			}

			private HttpResponseBaseMapper _mapper;
			private HttpResponseBase _response;
			private HttpContextBase _context;

			[Test]
			public void Must_map_httpresponsebase_type()
			{
				Assert.That(_mapper.CanMapType(_context, typeof(HttpResponseBase)), Is.True);
			}
		}
	}
}
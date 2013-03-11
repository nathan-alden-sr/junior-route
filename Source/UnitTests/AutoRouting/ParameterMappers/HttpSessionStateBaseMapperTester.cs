using System;
using System.Reflection;
using System.Web;

using Junior.Route.AutoRouting.ParameterMappers;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.ParameterMappers
{
	public static class HttpSessionStateBaseMapperTester
	{
		[TestFixture]
		public class When_mapping_parameter
		{
			[SetUp]
			public void SetUp()
			{
				_session = MockRepository.GenerateMock<HttpSessionStateBase>();
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Session).Return(_session);
				_mapper = new HttpSessionStateBaseMapper();

				Type type = typeof(When_mapping_parameter);
				MethodInfo methodInfo = type.GetMethod("Method", BindingFlags.NonPublic | BindingFlags.Instance);
				ParameterInfo parameterInfo = methodInfo.GetParameters()[0];

				_result = _mapper.Map(_context, type, methodInfo, parameterInfo);
			}

			// ReSharper disable UnusedMember.Local
			private void Method(HttpSessionStateBase session)
				// ReSharper restore UnusedMember.Local
			{
			}

			private HttpSessionStateBaseMapper _mapper;
			private HttpContextBase _context;
			private MapResult _result;
			private HttpSessionStateBase _session;

			[Test]
			public void Must_use_session_from_context()
			{
				Assert.That(_result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(_result.Value, Is.SameAs(_session));
				_context.AssertWasCalled(arg => arg.Session);
			}
		}

		[TestFixture]
		public class When_testing_if_mapper_can_map_parameter_type
		{
			[SetUp]
			public void SetUp()
			{
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_mapper = new HttpSessionStateBaseMapper();
			}

			private HttpSessionStateBaseMapper _mapper;
			private HttpContextBase _context;

			[Test]
			public void Must_map_httpsessionstatebase_type()
			{
				Assert.That(_mapper.CanMapType(_context, typeof(HttpSessionStateBase)), Is.True);
			}
		}
	}
}
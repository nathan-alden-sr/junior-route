using System;
using System.Reflection;
using System.Web;

using Junior.Route.AutoRouting.ParameterMappers;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.ParameterMappers
{
	public static class HttpApplicationStateBaseMapperTester
	{
		[TestFixture]
		public class When_mapping_parameter
		{
			[SetUp]
			public void SetUp()
			{
				_application = MockRepository.GenerateMock<HttpApplicationStateBase>();
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Application).Return(_application);
				_mapper = new HttpApplicationStateBaseMapper();

				Type type = typeof(When_mapping_parameter);
				MethodInfo methodInfo = type.GetMethod("Method", BindingFlags.NonPublic | BindingFlags.Instance);
				ParameterInfo parameterInfo = methodInfo.GetParameters()[0];

				_result = _mapper.MapAsync(_context, type, methodInfo, parameterInfo).Result;
			}

			// ReSharper disable UnusedMember.Local
			private void Method(HttpApplicationStateBase application)
				// ReSharper restore UnusedMember.Local
			{
			}

			private HttpApplicationStateBaseMapper _mapper;
			private HttpContextBase _context;
			private MapResult _result;
			private HttpApplicationStateBase _application;

			[Test]
			public void Must_use_application_from_context()
			{
				Assert.That(_result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(_result.Value, Is.SameAs(_application));
				_context.AssertWasCalled(arg => arg.Application);
			}
		}

		[TestFixture]
		public class When_testing_if_mapper_can_map_parameter_type
		{
			[SetUp]
			public void SetUp()
			{
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_mapper = new HttpApplicationStateBaseMapper();
			}

			private HttpApplicationStateBaseMapper _mapper;
			private HttpContextBase _context;

			[Test]
			public async void Must_map_httpapplicationstatebase_type()
			{
				Assert.That(await _mapper.CanMapTypeAsync(_context, typeof(HttpApplicationStateBase)), Is.True);
			}
		}
	}
}
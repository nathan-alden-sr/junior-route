using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.AutoRouting.ParameterMappers;
using Junior.Route.AutoRouting.ParameterMappers.Request;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.ParameterMappers.Request
{
	public static class RequestMapperTester
	{
		private class ExceptionMapper : RequestMapper
		{
			public ExceptionMapper(bool caseSensitive = false, DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
				: base(NameValueCollectionSource.Form, caseSensitive, errorHandling)
			{
			}

			public override Task<bool> CanMapTypeAsync(HttpContextBase context, Type parameterType)
			{
				return true.AsCompletedTask();
			}

			protected override Task<MapResult> OnMapAsync(HttpContextBase context, string value, Type parameterType)
			{
				throw new ApplicationException();
			}
		}

		private class Mapper : RequestMapper
		{
			public Mapper(NameValueCollectionSource source, bool caseSensitive = false, DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
				: base(source, caseSensitive, errorHandling)
			{
			}

			public override Task<bool> CanMapTypeAsync(HttpContextBase context, Type parameterType)
			{
				return true.AsCompletedTask();
			}

			protected override Task<MapResult> OnMapAsync(HttpContextBase context, string value, Type parameterType)
			{
				return MapResult.ValueMapped(value).AsCompletedTask();
			}
		}

		[TestFixture]
		public class When_conversion_fails_and_configured_to_throw_exception
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new ExceptionMapper(errorHandling:DataConversionErrorHandling.ThrowException);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Form).Return(new NameValueCollection { { "I", "1.2" } });
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Request).Return(_request);
			}

			private RequestMapper _mapper;
			private HttpRequestBase _request;
			private HttpContextBase _context;

			public class Endpoint
			{
				public void Method(int i)
				{
				}
			}

			[Test]
			[TestCase(typeof(Endpoint), "Method", "i")]
			[ExpectedException(typeof(ApplicationException))]
#warning Update to use async Assert.That(..., Throws.InstanceOf<>) when NUnit 2.6.3 becomes available
			public async void Must_throw_exception(Type type, string methodName, string parameterName)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);
				ParameterInfo parameterInfo = methodInfo.GetParameters().Single(arg => arg.Name == parameterName);

				await _mapper.MapAsync(_context, type, methodInfo, parameterInfo);
			}
		}

		[TestFixture]
		public class When_conversion_fails_and_configured_to_use_default_value
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new ExceptionMapper();
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Form).Return(new NameValueCollection { { "I", "1.2" } });
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Request).Return(_request);
			}

			private ExceptionMapper _mapper;
			private HttpRequestBase _request;
			private HttpContextBase _context;

			public class Endpoint
			{
				public void Method(int i)
				{
				}
			}

			[Test]
			[TestCase(typeof(Endpoint), "Method", "i", 0)]
			public async void Must_map_default_value_to_parameters(Type type, string methodName, string parameterName, object expectedValue)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);
				ParameterInfo parameterInfo = methodInfo.GetParameters().Single(arg => arg.Name == parameterName);
				MapResult result = await _mapper.MapAsync(_context, type, methodInfo, parameterInfo);

				Assert.That(result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(result.Value, Is.EqualTo(expectedValue));
			}
		}

		[TestFixture]
		public class When_mapping_from_source
		{
			public class Endpoint
			{
				public void Method(int i)
				{
				}
			}

			[Test]
			[TestCase(typeof(Endpoint), NameValueCollectionSource.Form, "Method", "i", "1.2")]
			[TestCase(typeof(Endpoint), NameValueCollectionSource.QueryString, "Method", "i", "2.2")]
			public async void Must_map_to_parameters_from_specified_source(Type type, NameValueCollectionSource source, string methodName, string parameterName, object expectedValue)
			{
				var mapper = new Mapper(source);
				var request = MockRepository.GenerateMock<HttpRequestBase>();
				var context = MockRepository.GenerateMock<HttpContextBase>();

				request.Stub(arg => arg.Form).Return(new NameValueCollection { { "I", "1.2" } });
				request.Stub(arg => arg.QueryString).Return(new NameValueCollection { { "I", "2.2" } });
				context.Stub(arg => arg.Request).Return(request);

				MethodInfo methodInfo = type.GetMethod(methodName);
				ParameterInfo parameterInfo = methodInfo.GetParameters().Single(arg => arg.Name == parameterName);
				MapResult result = await mapper.MapAsync(context, type, methodInfo, parameterInfo);

				Assert.That(result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(result.Value, Is.EqualTo(expectedValue));
			}
		}

		[TestFixture]
		public class When_performing_case_insensitive_mapping
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new Mapper(NameValueCollectionSource.Form);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Form).Return(new NameValueCollection { { "D", "1.2" } });
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Request).Return(_request);
			}

			private Mapper _mapper;
			private HttpRequestBase _request;
			private HttpContextBase _context;

			public class Endpoint
			{
				public void Method(double d)
				{
				}
			}

			[Test]
			[TestCase(typeof(Endpoint), "Method", "d")]
			public async void Must_map_to_parameters_whose_names_differ_by_case(Type type, string methodName, string parameterName)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);
				ParameterInfo parameterInfo = methodInfo.GetParameters().Single(arg => arg.Name == parameterName);
				MapResult result = await _mapper.MapAsync(_context, type, methodInfo, parameterInfo);

				Assert.That(result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(result.Value, Is.EqualTo("1.2"));
			}
		}

		[TestFixture]
		public class When_performing_case_sensitive_mapping_from_form_values
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new Mapper(NameValueCollectionSource.Form, true);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.Form).Return(new NameValueCollection { { "S", "value" }, { "I", "0" } });
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Request).Return(_request);
			}

			private Mapper _mapper;
			private HttpRequestBase _request;
			private HttpContextBase _context;

			public class Endpoint1
			{
				public void Method(string S, double s, int I)
				{
				}
			}

			public class Endpoint2
			{
				public void Method(string s)
				{
				}
			}

			[Test]
			[TestCase(typeof(Endpoint1), "Method", "S", "value")]
			[TestCase(typeof(Endpoint1), "Method", "I", "0")]
			public async void Must_map_to_parameters_whose_names_have_same_case(Type type, string methodName, string parameterName, object expectedValue)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);
				ParameterInfo parameterInfo = methodInfo.GetParameters().Single(arg => arg.Name == parameterName);
				MapResult result = await _mapper.MapAsync(_context, type, methodInfo, parameterInfo);

				Assert.That(result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(result.Value, Is.EqualTo(expectedValue));
			}

			[Test]
			[TestCase(typeof(Endpoint2), "Method", "s")]
			public async void Must_not_map_to_parameters_whose_names_have_different_case(Type type, string methodName, string parameterName)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);
				ParameterInfo parameterInfo = methodInfo.GetParameters().Single(arg => arg.Name == parameterName);
				MapResult result = await _mapper.MapAsync(_context, type, methodInfo, parameterInfo);

				Assert.That(result.ResultType, Is.EqualTo(MapResultType.ValueNotMapped));
			}
		}
	}
}
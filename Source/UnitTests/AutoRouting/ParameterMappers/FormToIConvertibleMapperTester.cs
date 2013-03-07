using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web;

using Junior.Route.AutoRouting.ParameterMappers;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.ParameterMappers
{
	public static class FormToIConvertibleMapperTester
	{
		[TestFixture]
		public class When_conversion_fails_and_configured_to_throw_exception
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new FormToIConvertibleMapper(errorHandling:DataConversionErrorHandling.ThrowException);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request
					.Stub(arg => arg.Form)
					.Return(new NameValueCollection
						{
							{ "I", "1.2" }
						});
			}

			private FormToIConvertibleMapper _mapper;
			private HttpRequestBase _request;

			public class Endpoint
			{
				public void Method(int i)
				{
				}
			}

			[Test]
			[TestCase(typeof(Endpoint), "Method", "i")]
			public void Must_throw_exception(Type type, string methodName, string parameterName)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);
				ParameterInfo parameterInfo = methodInfo.GetParameters().Single(arg => arg.Name == parameterName);

				Assert.That(() => _mapper.Map(_request, type, methodInfo, parameterInfo), Throws.InstanceOf<ApplicationException>());
			}
		}

		[TestFixture]
		public class When_conversion_fails_and_configured_to_use_default_value
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new FormToIConvertibleMapper();
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request
					.Stub(arg => arg.Form)
					.Return(new NameValueCollection
						{
							{ "I", "1.2" }
						});
			}

			private FormToIConvertibleMapper _mapper;
			private HttpRequestBase _request;

			public class Endpoint
			{
				public void Method(int i)
				{
				}
			}

			[Test]
			[TestCase(typeof(Endpoint), "Method", "i", 0)]
			public void Must_map_default_value(Type type, string methodName, string parameterName, object expectedValue)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);
				ParameterInfo parameterInfo = methodInfo.GetParameters().Single(arg => arg.Name == parameterName);
				MapResult result = _mapper.Map(_request, type, methodInfo, parameterInfo);

				Assert.That(result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(result.Value, Is.EqualTo(expectedValue));
			}
		}

		[TestFixture]
		public class When_performing_case_insensitive_mapping_from_form_values
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new FormToIConvertibleMapper();
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request
					.Stub(arg => arg.Form)
					.Return(new NameValueCollection
						{
							{ "D", "1.2" }
						});
			}

			private FormToIConvertibleMapper _mapper;
			private HttpRequestBase _request;

			public class Endpoint
			{
				public void Method(double d)
				{
				}
			}

			[Test]
			[TestCase(typeof(Endpoint), "Method", "d", 1.2)]
			public void Must_map_to_properties_whose_return_values_implement_iconvertible(Type type, string methodName, string parameterName, object expectedValue)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);
				ParameterInfo parameterInfo = methodInfo.GetParameters().Single(arg => arg.Name == parameterName);
				MapResult result = _mapper.Map(_request, type, methodInfo, parameterInfo);

				Assert.That(result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(result.Value, Is.EqualTo(expectedValue));
			}
		}

		[TestFixture]
		public class When_performing_case_sensitive_mapping_from_form_values
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new FormToIConvertibleMapper(true);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request
					.Stub(arg => arg.Form)
					.Return(new NameValueCollection
						{
							{ "S", "value" },
							{ "I", "0" }
						});
			}

			private FormToIConvertibleMapper _mapper;
			private HttpRequestBase _request;

			public class Endpoint1
			{
				public void Method(string S, double s, int I)
				{
				}
			}

			public class Endpoint2
			{
				public void Method(int S)
				{
				}
			}

			[Test]
			[TestCase(typeof(Endpoint1), "Method", "S", "value")]
			[TestCase(typeof(Endpoint1), "Method", "I", 0)]
			public void Must_map_to_properties_whose_return_values_implement_iconvertible(Type type, string methodName, string parameterName, object expectedValue)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);
				ParameterInfo parameterInfo = methodInfo.GetParameters().Single(arg => arg.Name == parameterName);
				MapResult result = _mapper.Map(_request, type, methodInfo, parameterInfo);

				Assert.That(result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(result.Value, Is.EqualTo(expectedValue));
			}
		}

		[TestFixture]
		public class When_testing_if_mapper_can_map_types_implementing_iconvertible
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new FormToIConvertibleMapper();
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private FormToIConvertibleMapper _mapper;
			private HttpRequestBase _request;

			[Test]
			[TestCase(typeof(string))]
			[TestCase(typeof(int))]
			public void Must_map_types_implementing_iconvertible(Type parameterType)
			{
				Assert.That(_mapper.CanMapType(_request, parameterType), Is.True);
			}
		}

		[TestFixture]
		public class When_testing_if_mapper_can_map_types_not_implementing_iconvertible
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new FormToIConvertibleMapper();
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private FormToIConvertibleMapper _mapper;
			private HttpRequestBase _request;

			[Test]
			[TestCase(typeof(object))]
			[TestCase(typeof(HttpRequestBase))]
			public void Must_not_map_types_not_implementing_iconvertible(Type propertyType)
			{
				Assert.That(_mapper.CanMapType(_request, propertyType), Is.False);
			}
		}
	}
}
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

using Junior.Route.AutoRouting;
using Junior.Route.AutoRouting.ParameterMappers;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.ParameterMappers
{
	public static class JsonModelMapperTester
	{
		[TestFixture]
		public class When_deserialization_fails_and_configured_to_throw_exception
		{
			[SetUp]
			public void SetUp()
			{
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.ContentEncoding).Return(Encoding.ASCII);
				_request.Stub(arg => arg.ContentType).Return("application/json");
				_request.Stub(arg => arg.InputStream).Return(new MemoryStream(Encoding.ASCII.GetBytes("{")));
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Request).Return(_request);
				_mapper = new JsonModelMapper(DataConversionErrorHandling.ThrowException);
			}

			private HttpRequestBase _request;
			private JsonModelMapper _mapper;
			private HttpContextBase _context;

			public class Endpoint
			{
				public void Method(Model model)
				{
				}
			}

			public class Model
			{
			}

			[Test]
			[TestCase(typeof(Endpoint), "Method", "model")]
			public void Must_throw_exception(Type type, string methodName, string parameterName)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);
				ParameterInfo parameterInfo = methodInfo.GetParameters().Single(arg => arg.Name == parameterName);

				Assert.That(() => _mapper.Map(_context, type, methodInfo, parameterInfo), Throws.InstanceOf<ApplicationException>());
			}
		}

		[TestFixture]
		public class When_deserialization_fails_and_configured_to_use_default_value
		{
			[SetUp]
			public void SetUp()
			{
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.ContentEncoding).Return(Encoding.ASCII);
				_request.Stub(arg => arg.ContentType).Return("application/json");
				_request.Stub(arg => arg.InputStream).Return(new MemoryStream(Encoding.ASCII.GetBytes("{")));
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Request).Return(_request);
				_mapper = new JsonModelMapper();
			}

			private HttpRequestBase _request;
			private JsonModelMapper _mapper;
			private HttpContextBase _context;

			public class Endpoint
			{
				public void Method(Model model)
				{
				}
			}

			public class Model
			{
			}

			[Test]
			[TestCase(typeof(Endpoint), "Method", "model")]
			public void Must_use_default_value(Type type, string methodName, string parameterName)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);
				ParameterInfo parameterInfo = methodInfo.GetParameters().Single(arg => arg.Name == parameterName);
				MapResult result = _mapper.Map(_context, type, methodInfo, parameterInfo);

				Assert.That(result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(result.Value, Is.EqualTo(parameterInfo.ParameterType.GetDefaultValue()));
			}
		}

		[TestFixture]
		public class When_deserializing_request
		{
			[SetUp]
			public void SetUp()
			{
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.ContentEncoding).Return(Encoding.ASCII);
				_request.Stub(arg => arg.ContentType).Return("application/json");
				_request.Stub(arg => arg.InputStream).Return(new MemoryStream(Encoding.ASCII.GetBytes(@"{ ""S"" : ""value"", ""I"": 1 }")));
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Request).Return(_request);
				_mapper = new JsonModelMapper();
			}

			private HttpRequestBase _request;
			private JsonModelMapper _mapper;
			private HttpContextBase _context;

			public class Endpoint
			{
				public void Method(Model model)
				{
				}
			}

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
			[TestCase(typeof(Endpoint), "Method", "model")]
			public void Must_deserialize_model(Type type, string methodName, string parameterName)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);
				ParameterInfo parameterInfo = methodInfo.GetParameters().Single(arg => arg.Name == parameterName);
				MapResult result = _mapper.Map(_context, type, methodInfo, parameterInfo);

				Assert.That(result.ResultType, Is.EqualTo(MapResultType.ValueMapped));
				Assert.That(result.Value, Is.TypeOf<Model>());

				var value = (Model)result.Value;

				Assert.That(value.I, Is.EqualTo(1));
				Assert.That(value.S, Is.EqualTo("value"));
			}
		}

		[TestFixture]
		public class When_testing_if_mapper_can_map_parameter_type_using_custom_delegate
		{
			[SetUp]
			public void SetUp()
			{
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.ContentType).Return("application/json");
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Request).Return(_request);
				_mapper = new JsonModelMapper(type => _executed = true);
				_mapper.CanMapType(_context, typeof(object));
			}

			private bool _executed;
			private JsonModelMapper _mapper;
			private HttpRequestBase _request;
			private HttpContextBase _context;

			[Test]
			public void Must_call_delegate()
			{
				Assert.That(_executed, Is.True);
			}
		}

		[TestFixture]
		public class When_testing_if_mapper_can_map_parameter_type_using_default_comparison
		{
			[SetUp]
			public void SetUp()
			{
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_request.Stub(arg => arg.ContentType).Return("application/json");
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Request).Return(_request);
				_mapper = new JsonModelMapper();
			}

			private JsonModelMapper _mapper;
			private HttpRequestBase _request;
			private HttpContextBase _context;

			public class Model
			{
			}

			public class JsonModel
			{
			}

			public class Model2
			{
			}

			[Test]
			[TestCase(typeof(Model))]
			[TestCase(typeof(JsonModel))]
			public void Must_map_types_ending_in_model(Type type)
			{
				Assert.That(_mapper.CanMapType(_context, type), Is.True);
			}

			[Test]
			[TestCase(typeof(Model2))]
			[TestCase(typeof(object))]
			public void Must_not_map_types_not_ending_in_model(Type type)
			{
				Assert.That(_mapper.CanMapType(_context, type), Is.False);
			}
		}
	}
}
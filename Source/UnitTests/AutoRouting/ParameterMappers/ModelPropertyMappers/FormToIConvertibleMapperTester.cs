using System;
using System.Collections.Specialized;
using System.Web;

using Junior.Route.AutoRouting.ParameterMappers;

using NUnit.Framework;

using Rhino.Mocks;

using FormToIConvertibleMapper = Junior.Route.AutoRouting.ParameterMappers.ModelPropertyMappers.FormToIConvertibleMapper;

namespace Junior.Route.UnitTests.AutoRouting.ParameterMappers.ModelPropertyMappers
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

			public class Model
			{
				public int I
				{
					get;
					set;
				}
			}

			[Test]
			[TestCase(typeof(Model), "I")]
			public void Must_throw_exception(Type type, string propertyName)
			{
				Assert.Throws<ApplicationException>(() => _mapper.Map(_request, type, type.GetProperty(propertyName)));
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

			public class Model
			{
				public int I
				{
					get;
					set;
				}
			}

			[Test]
			[TestCase(typeof(Model), "I", 0)]
			public void Must_map_default_value(Type type, string propertyName, object expectedValue)
			{
				MapResult result = _mapper.Map(_request, type, type.GetProperty(propertyName));

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

			public class Model
			{
				public double d
				{
					get;
					set;
				}
			}

			[Test]
			[TestCase(typeof(Model), "d", 1.2)]
			public void Must_map_to_properties_whose_return_values_implement_iconvertible(Type type, string propertyName, object expectedValue)
			{
				MapResult result = _mapper.Map(_request, type, type.GetProperty(propertyName));

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

			public class Model1
			{
				public string S
				{
					get;
					set;
				}

				public double s
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

			public class Model2
			{
				public int S
				{
					get;
					set;
				}
			}

			[Test]
			[TestCase(typeof(Model1), "S", "value")]
			[TestCase(typeof(Model1), "I", 0)]
			public void Must_map_to_properties_whose_return_values_implement_iconvertible(Type type, string propertyName, object expectedValue)
			{
				MapResult result = _mapper.Map(_request, type, type.GetProperty(propertyName));

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
			}

			private FormToIConvertibleMapper _mapper;

			[Test]
			[TestCase(typeof(string))]
			[TestCase(typeof(int))]
			public void Must_map_types_implementing_iconvertible(Type propertyType)
			{
				Assert.That(_mapper.CanMapType(propertyType), Is.True);
			}
		}

		[TestFixture]
		public class When_testing_if_mapper_can_map_types_not_implementing_iconvertible
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new FormToIConvertibleMapper();
			}

			private FormToIConvertibleMapper _mapper;

			[Test]
			[TestCase(typeof(object))]
			[TestCase(typeof(HttpRequestBase))]
			public void Must_not_map_types_not_implementing_iconvertible(Type propertyType)
			{
				Assert.That(_mapper.CanMapType(propertyType), Is.False);
			}
		}
	}
}
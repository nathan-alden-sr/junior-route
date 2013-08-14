using System;
using System.Web;

using Junior.Route.AutoRouting.ParameterMappers;
using Junior.Route.AutoRouting.ParameterMappers.Request;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.ParameterMappers.Request
{
	public static class ConvertibleMapperTester
	{
		[TestFixture]
		public class When_testing_if_mapper_can_map_types_implementing_iconvertible
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new ConvertibleMapper(NameValueCollectionSource.Form);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Request).Return(_request);
			}

			private ConvertibleMapper _mapper;
			private HttpRequestBase _request;
			private HttpContextBase _context;

			[Test]
			[TestCase(typeof(string))]
			[TestCase(typeof(int))]
			public async void Must_map_types_implementing_iconvertible(Type parameterType)
			{
				Assert.That(await _mapper.CanMapTypeAsync(_context, parameterType), Is.True);
			}
		}

		[TestFixture]
		public class When_testing_if_mapper_can_map_types_not_implementing_iconvertible
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new ConvertibleMapper(NameValueCollectionSource.Form);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Request).Return(_request);
			}

			private ConvertibleMapper _mapper;
			private HttpRequestBase _request;
			private HttpContextBase _context;

			[Test]
			[TestCase(typeof(object))]
			[TestCase(typeof(HttpRequestBase))]
			public async void Must_not_map_types_not_implementing_iconvertible(Type parameterType)
			{
				Assert.That(await _mapper.CanMapTypeAsync(_context, parameterType), Is.False);
			}
		}
	}
}
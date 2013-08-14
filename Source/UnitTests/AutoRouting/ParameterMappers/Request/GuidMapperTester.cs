using System;
using System.Web;

using Junior.Route.AutoRouting.ParameterMappers;
using Junior.Route.AutoRouting.ParameterMappers.Request;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.ParameterMappers.Request
{
	public static class GuidMapperTester
	{
		[TestFixture]
		public class When_testing_if_mapper_can_map_non_guid_types
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new GuidMapper(NameValueCollectionSource.Form);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Request).Return(_request);
			}

			private GuidMapper _mapper;
			private HttpRequestBase _request;
			private HttpContextBase _context;

			[Test]
			[TestCase(typeof(object))]
			[TestCase(typeof(HttpRequestBase))]
			public async void Must_not_map_non_guid_types(Type propertyType)
			{
				Assert.That(await _mapper.CanMapTypeAsync(_context, propertyType), Is.False);
			}
		}

		[TestFixture]
		public class When_testing_if_mapper_can_map_types_implementing_iconvertible
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new GuidMapper(NameValueCollectionSource.Form);
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_context.Stub(arg => arg.Request).Return(_request);
			}

			private GuidMapper _mapper;
			private HttpRequestBase _request;
			private HttpContextBase _context;

			[Test]
			public async void Must_map_guid_type()
			{
				Assert.That(await _mapper.CanMapTypeAsync(_context, typeof(Guid)), Is.True);
			}
		}
	}
}
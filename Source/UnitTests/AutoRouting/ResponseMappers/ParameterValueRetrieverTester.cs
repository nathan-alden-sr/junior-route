using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

using Junior.Route.AutoRouting.ParameterMappers;
using Junior.Route.AutoRouting.ResponseMappers;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.ResponseMappers
{
	public static class ParameterValueRetrieverTester
	{
		[TestFixture]
		public class When_no_mapper_is_found_for_method_parameter
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = MockRepository.GenerateMock<IParameterMapper>();
				_mapper.Stub(arg => arg.CanMapType(Arg<HttpRequestBase>.Is.Anything, Arg<Type>.Is.Anything)).Return(false);
				_retriever = new ParameterValueRetriever(new[] { _mapper });
				_request = MockRepository.GenerateMock<HttpRequestBase>();
			}

			private ParameterValueRetriever _retriever;
			private HttpRequestBase _request;
			private IParameterMapper _mapper;

			public class Endpoint
			{
				public void Method(int value)
				{
				}
			}

			[Test]
			public void Must_throw_exception()
			{
				Assert.That(() => _retriever.GetParameterValues(_request, typeof(Endpoint), typeof(Endpoint).GetMethod("Method")), Throws.InstanceOf<ApplicationException>());
			}
		}

		[TestFixture]
		public class When_retrieving_values_for_method_parameters
		{
			[SetUp]
			public void SetUp()
			{
				_mapper1 = MockRepository.GenerateMock<IParameterMapper>();
				_mapper1.Stub(arg => arg.CanMapType(Arg<HttpRequestBase>.Is.Anything, Arg<Type>.Is.Anything)).Return(false);
				_mapper2 = MockRepository.GenerateMock<IParameterMapper>();
				_mapper2.Stub(arg => arg.CanMapType(Arg<HttpRequestBase>.Is.Anything, Arg<Type>.Is.Anything)).Return(true);
				_mapper2
					.Stub(arg => arg.Map(Arg<HttpRequestBase>.Is.Anything, Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything, Arg<ParameterInfo>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = MapResult.ValueMapped(100))
					.Return(null);
				_mapper3 = MockRepository.GenerateMock<IParameterMapper>();
				_mapper3.Stub(arg => arg.CanMapType(Arg<HttpRequestBase>.Is.Anything, Arg<Type>.Is.Anything)).Return(false);
				_retriever = new ParameterValueRetriever(new[] { _mapper1, _mapper2, _mapper3 });
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_values = _retriever.GetParameterValues(_request, typeof(Endpoint), typeof(Endpoint).GetMethod("Method"));
			}

			private IParameterMapper _mapper1;
			private IParameterMapper _mapper2;
			private IParameterMapper _mapper3;
			private ParameterValueRetriever _retriever;
			private HttpRequestBase _request;
			private IEnumerable<object> _values;

			public class Endpoint
			{
				public void Method(int value)
				{
				}
			}

			[Test]
			public void Must_use_first_mapper_that_maps_successfully()
			{
				_mapper1.AssertWasNotCalled(arg => arg.Map(Arg<HttpRequestBase>.Is.Anything, Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything, Arg<ParameterInfo>.Is.Anything));
				_mapper2.AssertWasCalled(arg => arg.Map(Arg<HttpRequestBase>.Is.Anything, Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything, Arg<ParameterInfo>.Is.Anything));
				_mapper3.AssertWasNotCalled(arg => arg.Map(Arg<HttpRequestBase>.Is.Anything, Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything, Arg<ParameterInfo>.Is.Anything));
				Assert.That(_values, Is.EquivalentTo(new[] { 100 }));
			}
		}
	}
}
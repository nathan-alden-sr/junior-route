using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

using Junior.Common;
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
				_mapper.Stub(arg => arg.CanMapTypeAsync(Arg<HttpContextBase>.Is.Anything, Arg<Type>.Is.Anything)).Return(false.AsCompletedTask());
				_retriever = new ParameterValueRetriever(new[] { _mapper });
				_context = MockRepository.GenerateMock<HttpContextBase>();
			}

			private ParameterValueRetriever _retriever;
			private HttpContextBase _context;
			private IParameterMapper _mapper;

			public class Endpoint
			{
				public void Method(int value)
				{
				}
			}

			[Test]
			[ExpectedException(typeof(ApplicationException))]
#warning Update to use async Assert.That(..., Throws.InstanceOf<>) when NUnit 2.6.3 becomes available
			public async void Must_throw_exception()
			{
				await _retriever.GetParameterValuesAsync(_context, typeof(Endpoint), typeof(Endpoint).GetMethod("Method"));
			}
		}

		[TestFixture]
		public class When_retrieving_values_for_method_parameters
		{
			[SetUp]
			public void SetUp()
			{
				_mapper1 = MockRepository.GenerateMock<IParameterMapper>();
				_mapper1.Stub(arg => arg.CanMapTypeAsync(Arg<HttpContextBase>.Is.Anything, Arg<Type>.Is.Anything)).Return(false.AsCompletedTask());
				_mapper2 = MockRepository.GenerateMock<IParameterMapper>();
				_mapper2.Stub(arg => arg.CanMapTypeAsync(Arg<HttpContextBase>.Is.Anything, Arg<Type>.Is.Anything)).Return(true.AsCompletedTask());
				_mapper2
					.Stub(arg => arg.MapAsync(Arg<HttpContextBase>.Is.Anything, Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything, Arg<ParameterInfo>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = (MapResult.ValueMapped(100)).AsCompletedTask())
					.Return(null);
				_mapper3 = MockRepository.GenerateMock<IParameterMapper>();
				_mapper3.Stub(arg => arg.CanMapTypeAsync(Arg<HttpContextBase>.Is.Anything, Arg<Type>.Is.Anything)).Return(false.AsCompletedTask());
				_retriever = new ParameterValueRetriever(new[] { _mapper1, _mapper2, _mapper3 });
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_values = _retriever.GetParameterValuesAsync(_context, typeof(Endpoint), typeof(Endpoint).GetMethod("Method")).Result;
			}

			private IParameterMapper _mapper1;
			private IParameterMapper _mapper2;
			private IParameterMapper _mapper3;
			private ParameterValueRetriever _retriever;
			private HttpContextBase _context;
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
				_mapper1.AssertWasNotCalled(arg => arg.MapAsync(Arg<HttpContextBase>.Is.Anything, Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything, Arg<ParameterInfo>.Is.Anything));
				_mapper2.AssertWasCalled(arg => arg.MapAsync(Arg<HttpContextBase>.Is.Anything, Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything, Arg<ParameterInfo>.Is.Anything));
				_mapper3.AssertWasNotCalled(arg => arg.MapAsync(Arg<HttpContextBase>.Is.Anything, Arg<Type>.Is.Anything, Arg<MethodInfo>.Is.Anything, Arg<ParameterInfo>.Is.Anything));
				Assert.That(_values, Is.EquivalentTo(new[] { 100 }));
			}
		}
	}
}
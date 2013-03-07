using System;
using System.Linq;
using System.Reflection;
using System.Web;

using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.ParameterMappers;
using Junior.Route.AutoRouting.ParameterMappers.ModelPropertyMappers;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.AutoRouting.ParameterMappers
{
	public static class ModelMapperTester
	{
		[TestFixture]
		public class When_mapping_model_class_and_supplying_container
		{
			[SetUp]
			public void SetUp()
			{
				_container = MockRepository.GenerateMock<IContainer>();
				_container.Stub(arg => arg.GetInstance(typeof(Model))).Return(new Model());
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_mapper = new ModelMapper(_container);
			}

			private IContainer _container;
			private ModelMapper _mapper;
			private HttpRequestBase _request;

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
			public void Must_use_container_to_get_instance(Type type, string methodName, string parameterName)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);
				ParameterInfo parameterInfo = methodInfo.GetParameters().Single(arg => arg.Name == parameterName);

				_mapper.Map(_request, type, methodInfo, parameterInfo);

				_container.AssertWasCalled(arg => arg.GetInstance(parameterInfo.ParameterType));
			}
		}

		[TestFixture]
		public class When_mapping_properties
		{
			[SetUp]
			public void SetUp()
			{
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_modelPropertyMapper1 = MockRepository.GenerateMock<IModelPropertyMapper>();
				_modelPropertyMapper1
					.Stub(arg => arg.CanMapType(Arg<Type>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = ((Type)arg.Arguments.First()) == typeof(string))
					.Return(false);
				_modelPropertyMapper1.Stub(arg => arg.Map(Arg<HttpRequestBase>.Is.Anything, Arg<Type>.Is.Anything, Arg<PropertyInfo>.Is.Anything)).Return(MapResult.ValueMapped("value"));
				_modelPropertyMapper2 = MockRepository.GenerateMock<IModelPropertyMapper>();
				_modelPropertyMapper2
					.Stub(arg => arg.CanMapType(Arg<Type>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = ((Type)arg.Arguments.First()) == typeof(string))
					.Return(false);
				_modelPropertyMapper2.Stub(arg => arg.Map(Arg<HttpRequestBase>.Is.Anything, Arg<Type>.Is.Anything, Arg<PropertyInfo>.Is.Anything)).Return(MapResult.ValueMapped("value"));
				_modelMapper = new ModelMapper(_modelPropertyMapper1, _modelPropertyMapper2);
			}

			private ModelMapper _modelMapper;
			private HttpRequestBase _request;
			private IModelPropertyMapper _modelPropertyMapper1;
			private IModelPropertyMapper _modelPropertyMapper2;

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

				// ReSharper disable UnusedMember.Local
				private int I
					// ReSharper restore UnusedMember.Local
				{
					get;
					set;
				}
			}

			[Test]
			[TestCase(typeof(Endpoint), "Method", "model")]
			public void Must_only_consider_public_instance_properties(Type type, string methodName, string parameterName)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);
				ParameterInfo parameterInfo = methodInfo.GetParameters().Single(arg => arg.Name == parameterName);

				Assert.That(() => _modelMapper.Map(_request, type, methodInfo, parameterInfo), Throws.Nothing);
			}

			[Test]
			[TestCase(typeof(Endpoint), "Method", "model")]
			public void Must_use_first_matching_parameter_mapper(Type type, string methodName, string parameterName)
			{
				MethodInfo methodInfo = type.GetMethod(methodName);
				ParameterInfo parameterInfo = methodInfo.GetParameters().Single(arg => arg.Name == parameterName);

				_modelMapper.Map(_request, type, methodInfo, parameterInfo);

				_modelPropertyMapper1.AssertWasCalled(arg => arg.Map(Arg<HttpRequestBase>.Is.Anything, Arg<Type>.Is.Anything, Arg<PropertyInfo>.Is.Anything));
				_modelPropertyMapper2.AssertWasNotCalled(arg => arg.Map(Arg<HttpRequestBase>.Is.Anything, Arg<Type>.Is.Anything, Arg<PropertyInfo>.Is.Anything));
			}
		}

		[TestFixture]
		public class When_testing_if_mapper_can_map_parameter_type_using_custom_delegate
		{
			[SetUp]
			public void SetUp()
			{
				_request = MockRepository.GenerateMock<HttpRequestBase>();
				_mapper = new ModelMapper(
					type =>
						{
							_executed = true;
							return true;
						});
			}

			public class Endpoint
			{
				public void Method(Model model)
				{
				}
			}

			public class Model
			{
			}

			private ModelMapper _mapper;
			private HttpRequestBase _request;
			private bool _executed;

			[Test]
			public void Must_use_container_to_get_instance()
			{
				_mapper.CanMapType(_request, typeof(object));

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
				_mapper = new ModelMapper();
			}

			public class Endpoint
			{
				public void Method(Model model)
				{
				}
			}

			public class Model
			{
			}

			public class AnotherModel
			{
			}

			public class Model2
			{
			}

			private ModelMapper _mapper;
			private HttpRequestBase _request;

			[Test]
			[TestCase(typeof(Model))]
			[TestCase(typeof(AnotherModel))]
			public void Must_map_types_ending_in_model(Type type)
			{
				Assert.That(_mapper.CanMapType(_request, type), Is.True);
			}

			[Test]
			[TestCase(typeof(Model2))]
			[TestCase(typeof(object))]
			public void Must_not_map_types_not_ending_in_model(Type type)
			{
				Assert.That(_mapper.CanMapType(_request, type), Is.False);
			}
		}
	}
}
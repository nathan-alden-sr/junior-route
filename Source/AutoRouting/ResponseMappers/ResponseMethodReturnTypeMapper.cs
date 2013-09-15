using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.ParameterMappers;
using Junior.Route.AutoRouting.ParameterMappers.ModelPropertyMappers;
using Junior.Route.Common;
using Junior.Route.Routing.Responses;

using Newtonsoft.Json;

namespace Junior.Route.AutoRouting.ResponseMappers
{
	public class ResponseMethodReturnTypeMapper : IResponseMapper
	{
		private readonly IEnumerable<IMappedDelegateContextFactory> _contextFactories;
		private readonly HashSet<IParameterMapper> _parameterMappers = new HashSet<IParameterMapper>();

		public ResponseMethodReturnTypeMapper(IEnumerable<IParameterMapper> mappers, IEnumerable<IMappedDelegateContextFactory> contextFactories)
		{
			mappers.ThrowIfNull("mappers");
			contextFactories.ThrowIfNull("contextFactories");

			_parameterMappers.AddRange(mappers);
			_contextFactories = contextFactories;
		}

		public Task MapAsync(Func<IContainer> container, Type type, MethodInfo method, Routing.Route route)
		{
			container.ThrowIfNull("container");
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			route.ThrowIfNull("route");

			bool methodReturnTypeImplementsIResponse = method.ReturnType.ImplementsInterface<IResponse>();
			bool methodReturnTypeIsTaskT = method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);
			bool methodReturnTypeIsVoid = method.ReturnType == typeof(void);

			if (methodReturnTypeImplementsIResponse)
			{
				ParameterInfo[] parameterInfos = method.GetParameters();
				ParameterExpression instanceParameterExpression = Expression.Parameter(typeof(object), "instance");
				ParameterExpression parametersParameterExpression = Expression.Parameter(typeof(object[]), "parameters");
				UnaryExpression unaryExpression =
					Expression.Convert(
						Expression.Call(
							Expression.Convert(instanceParameterExpression, type),
							method,
							parameterInfos.Select((arg, index) => Expression.Convert(
								Expression.ArrayIndex(parametersParameterExpression, Expression.Constant(index)),
								arg.ParameterType))),
						typeof(IResponse));
				Func<object, object[], IResponse> @delegate = Expression.Lambda<Func<object, object[], IResponse>>(unaryExpression, instanceParameterExpression, parametersParameterExpression).Compile();

				route.RespondWith(
					async context =>
					{
						object instance;

						try
						{
							instance = container().GetInstance(type);
						}
						catch (Exception exception)
						{
							throw new ApplicationException(String.Format("Unable to resolve instance of type {0}.", type.FullName), exception);
						}
						if (instance == null)
						{
							throw new ApplicationException(String.Format("Unable to resolve instance of type {0}.", type.FullName));
						}

						var parameterValueRetriever = new ParameterValueRetriever(_parameterMappers);
						object[] parameterValues = (await parameterValueRetriever.GetParameterValuesAsync(context, type, method)).ToArray();
						var disposableContexts = new List<IDisposable>();

						try
						{
							disposableContexts.AddRange(_contextFactories.Select(arg => arg.CreateContext(context, type, method)).Where(arg => arg != null));

							return @delegate(instance, parameterValues);
						}
						finally
						{
							foreach (IDisposable disposableContext in disposableContexts)
							{
								disposableContext.Dispose();
							}
						}
					},
					method.ReturnType);
			}
			else if (methodReturnTypeIsTaskT)
			{
				ParameterInfo[] parameterInfos = method.GetParameters();
				ParameterExpression instanceParameterExpression = Expression.Parameter(typeof(object), "instance");
				ParameterExpression parametersParameterExpression = Expression.Parameter(typeof(object[]), "parameters");
				Type methodGenericArgumentType = method.ReturnType.GetGenericArguments()[0];
				MethodInfo upcastMethodInfo = typeof(TaskExtensions)
					.GetMethod("Upcast", BindingFlags.Static | BindingFlags.Public)
					.MakeGenericMethod(methodGenericArgumentType, typeof(IResponse));
				UnaryExpression unaryExpression =
					Expression.Convert(
						Expression.Call(
							upcastMethodInfo,
							Expression.Call(
								Expression.Convert(instanceParameterExpression, type),
								method,
								parameterInfos.Select((arg, index) => Expression.Convert(
									Expression.ArrayIndex(parametersParameterExpression, Expression.Constant(index)),
									arg.ParameterType)))),
						upcastMethodInfo.ReturnType);
				Func<object, object[], Task<IResponse>> @delegate = Expression.Lambda<Func<object, object[], Task<IResponse>>>(unaryExpression, instanceParameterExpression, parametersParameterExpression).Compile();

				route.RespondWith(
					async context =>
					{
						object instance;

						try
						{
							instance = container().GetInstance(type);
						}
						catch (Exception exception)
						{
							throw new ApplicationException(String.Format("Unable to resolve instance of type {0}.", type.FullName), exception);
						}
						if (instance == null)
						{
							throw new ApplicationException(String.Format("Unable to resolve instance of type {0}.", type.FullName));
						}

						var parameterValueRetriever = new ParameterValueRetriever(_parameterMappers);
						object[] parameterValues = (await parameterValueRetriever.GetParameterValuesAsync(context, type, method)).ToArray();
						var disposableContexts = new List<IDisposable>();

						try
						{
							disposableContexts.AddRange(_contextFactories.Select(arg => arg.CreateContext(context, type, method)).Where(arg => arg != null));

							return await @delegate(instance, parameterValues);
						}
						finally
						{
							foreach (IDisposable disposableContext in disposableContexts)
							{
								disposableContext.Dispose();
							}
						}
					},
					methodGenericArgumentType);
			}
			else if (methodReturnTypeIsVoid)
			{
				ParameterInfo[] parameterInfos = method.GetParameters();
				ParameterExpression instanceParameterExpression = Expression.Parameter(typeof(object), "instance");
				ParameterExpression parametersParameterExpression = Expression.Parameter(typeof(object[]), "parameters");
				MethodCallExpression methodCallExpression =
					Expression.Call(
						Expression.Convert(instanceParameterExpression, type),
						method,
						parameterInfos.Select((arg, index) => Expression.Convert(
							Expression.ArrayIndex(parametersParameterExpression, Expression.Constant(index)),
							arg.ParameterType)));
				Action<object, object[]> @delegate = Expression.Lambda<Action<object, object[]>>(methodCallExpression, instanceParameterExpression, parametersParameterExpression).Compile();

				route.RespondWithNoContent(
					async context =>
					{
						object instance;

						try
						{
							instance = container().GetInstance(type);
						}
						catch (Exception exception)
						{
							throw new ApplicationException(String.Format("Unable to resolve instance of type {0}.", type.FullName), exception);
						}
						if (instance == null)
						{
							throw new ApplicationException(String.Format("Unable to resolve instance of type {0}.", type.FullName));
						}

						var parameterValueRetriever = new ParameterValueRetriever(_parameterMappers);
						object[] parameterValues = (await parameterValueRetriever.GetParameterValuesAsync(context, type, method)).ToArray();
						var disposableContexts = new List<IDisposable>();

						try
						{
							disposableContexts.AddRange(_contextFactories.Select(arg => arg.CreateContext(context, type, method)).Where(arg => arg != null));

							@delegate(instance, parameterValues);
						}
						finally
						{
							foreach (IDisposable disposableContext in disposableContexts)
							{
								disposableContext.Dispose();
							}
						}
					});
			}
			else
			{
				throw new ApplicationException(String.Format("The return type of {0}.{1} must implement {2} or be a {3} whose generic type argument implements {2}.", type.FullName, method.Name, typeof(IResponse).Name, typeof(Task<>)));
			}

			return Task.Factory.Empty();
		}

		public ResponseMethodReturnTypeMapper JsonModelMapper(
			Func<Type, bool> parameterTypeMatchDelegate,
			JsonSerializerSettings serializerSettings,
			DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
		{
			_parameterMappers.Add(new JsonModelMapper(parameterTypeMatchDelegate, serializerSettings, errorHandling));

			return this;
		}

		public ResponseMethodReturnTypeMapper JsonModelMapper(Func<Type, bool> parameterTypeMatchDelegate, DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
		{
			_parameterMappers.Add(new JsonModelMapper(parameterTypeMatchDelegate, errorHandling));

			return this;
		}

		public ResponseMethodReturnTypeMapper JsonModelMapper(JsonSerializerSettings serializerSettings, DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
		{
			_parameterMappers.Add(new JsonModelMapper(serializerSettings, errorHandling));

			return this;
		}

		public ResponseMethodReturnTypeMapper JsonModelMapper(DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
		{
			_parameterMappers.Add(new JsonModelMapper(errorHandling));

			return this;
		}

		public ResponseMethodReturnTypeMapper ModelMapper(IContainer container, Func<Type, bool> parameterTypeMatchDelegate, IEnumerable<IModelPropertyMapper> propertyMappers)
		{
			_parameterMappers.Add(new ModelMapper(container, parameterTypeMatchDelegate, propertyMappers));

			return this;
		}

		public ResponseMethodReturnTypeMapper ModelMapper(IContainer container, Func<Type, bool> parameterTypeMatchDelegate, params IModelPropertyMapper[] propertyMappers)
		{
			_parameterMappers.Add(new ModelMapper(container, parameterTypeMatchDelegate, propertyMappers));

			return this;
		}

		public ResponseMethodReturnTypeMapper ModelMapper(Func<Type, bool> parameterTypeMatchDelegate, IEnumerable<IModelPropertyMapper> propertyMappers)
		{
			_parameterMappers.Add(new ModelMapper(parameterTypeMatchDelegate, propertyMappers));

			return this;
		}

		public ResponseMethodReturnTypeMapper ModelMapper(Func<Type, bool> parameterTypeMatchDelegate, params IModelPropertyMapper[] propertyMappers)
		{
			_parameterMappers.Add(new ModelMapper(parameterTypeMatchDelegate, propertyMappers));

			return this;
		}

		public ResponseMethodReturnTypeMapper ModelMapper(IContainer container, IEnumerable<IModelPropertyMapper> propertyMappers)
		{
			_parameterMappers.Add(new ModelMapper(container, propertyMappers));

			return this;
		}

		public ResponseMethodReturnTypeMapper ModelMapper(IContainer container, params IModelPropertyMapper[] propertyMappers)
		{
			_parameterMappers.Add(new ModelMapper(container, propertyMappers));

			return this;
		}

		public ResponseMethodReturnTypeMapper ModelMapper(IEnumerable<IModelPropertyMapper> propertyMappers)
		{
			_parameterMappers.Add(new ModelMapper(propertyMappers));

			return this;
		}

		public ResponseMethodReturnTypeMapper ModelMapper(params IModelPropertyMapper[] propertyMappers)
		{
			_parameterMappers.Add(new ModelMapper(propertyMappers));

			return this;
		}
	}
}
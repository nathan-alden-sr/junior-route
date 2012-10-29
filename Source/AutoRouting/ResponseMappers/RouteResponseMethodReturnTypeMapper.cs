using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.ParameterMappers;
using Junior.Route.AutoRouting.ParameterMappers.ModelPropertyMappers;
using Junior.Route.Common;
using Junior.Route.Routing.Responses;

using Newtonsoft.Json;

namespace Junior.Route.AutoRouting.ResponseMappers
{
	public class RouteResponseMethodReturnTypeMapper : IRouteResponseMapper
	{
		private readonly HashSet<IParameterMapper> _parameterMappers = new HashSet<IParameterMapper>();

		public RouteResponseMethodReturnTypeMapper(IEnumerable<IParameterMapper> mappers)
		{
			mappers.ThrowIfNull("mappers");

			_parameterMappers.AddRange(mappers);
		}

		public RouteResponseMethodReturnTypeMapper(params IParameterMapper[] mappers)
			: this((IEnumerable<IParameterMapper>)mappers)
		{
		}

		public void Map(Func<IContainer> container, Type type, MethodInfo method, Routing.Route route)
		{
			container.ThrowIfNull("container");
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			route.ThrowIfNull("route");

			if (method.ReturnType == typeof(void))
			{
				route.RespondWithNoContent();
				return;
			}
			if (!method.ReturnType.ImplementsInterface<IResponse>())
			{
				throw new ApplicationException(String.Format("The return type of '{0}.{1}' does not implement '{2}'.", type.FullName, method.Name, typeof(IResponse).Name));
			}

			route.RespondWith(
				request =>
					{
						object instance;

						try
						{
							instance = container().GetInstance(type);
						}
						catch (Exception exception)
						{
							throw new ApplicationException(String.Format("Unable to resolve instance of type '{0}'.", type.FullName), exception);
						}
						if (instance == null)
						{
							throw new ApplicationException(String.Format("Unable to resolve instance of type '{0}'.", type.FullName));
						}

						var parameterValueRetriever = new ParameterValueRetriever(_parameterMappers);
						IEnumerable<object> parameterValues = parameterValueRetriever.GetParameterValues(request, type, method);
						var routeResponse = (IResponse)method.Invoke(instance, parameterValues.ToArray());

						return routeResponse;
					},
				method.ReturnType);
		}

		public RouteResponseMethodReturnTypeMapper JsonModelMapper(
			Func<Type, bool> parameterTypeMatchDelegate,
			JsonSerializerSettings serializerSettings,
			DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
		{
			_parameterMappers.Add(new JsonModelMapper(parameterTypeMatchDelegate, serializerSettings, errorHandling));

			return this;
		}

		public RouteResponseMethodReturnTypeMapper JsonModelMapper(Func<Type, bool> parameterTypeMatchDelegate, DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
		{
			_parameterMappers.Add(new JsonModelMapper(parameterTypeMatchDelegate, errorHandling));

			return this;
		}

		public RouteResponseMethodReturnTypeMapper JsonModelMapper(JsonSerializerSettings serializerSettings, DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
		{
			_parameterMappers.Add(new JsonModelMapper(serializerSettings, errorHandling));

			return this;
		}

		public RouteResponseMethodReturnTypeMapper JsonModelMapper(DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
		{
			_parameterMappers.Add(new JsonModelMapper(errorHandling));

			return this;
		}

		public RouteResponseMethodReturnTypeMapper ModelMapper(IContainer container, Func<Type, bool> parameterTypeMatchDelegate, IEnumerable<IModelPropertyMapper> propertyMappers)
		{
			_parameterMappers.Add(new ModelMapper(container, parameterTypeMatchDelegate, propertyMappers));

			return this;
		}

		public RouteResponseMethodReturnTypeMapper ModelMapper(IContainer container, Func<Type, bool> parameterTypeMatchDelegate, params IModelPropertyMapper[] propertyMappers)
		{
			_parameterMappers.Add(new ModelMapper(container, parameterTypeMatchDelegate, propertyMappers));

			return this;
		}

		public RouteResponseMethodReturnTypeMapper ModelMapper(Func<Type, bool> parameterTypeMatchDelegate, IEnumerable<IModelPropertyMapper> propertyMappers)
		{
			_parameterMappers.Add(new ModelMapper(parameterTypeMatchDelegate, propertyMappers));

			return this;
		}

		public RouteResponseMethodReturnTypeMapper ModelMapper(Func<Type, bool> parameterTypeMatchDelegate, params IModelPropertyMapper[] propertyMappers)
		{
			_parameterMappers.Add(new ModelMapper(parameterTypeMatchDelegate, propertyMappers));

			return this;
		}

		public RouteResponseMethodReturnTypeMapper ModelMapper(IContainer container, IEnumerable<IModelPropertyMapper> propertyMappers)
		{
			_parameterMappers.Add(new ModelMapper(container, propertyMappers));

			return this;
		}

		public RouteResponseMethodReturnTypeMapper ModelMapper(IContainer container, params IModelPropertyMapper[] propertyMappers)
		{
			_parameterMappers.Add(new ModelMapper(container, propertyMappers));

			return this;
		}

		public RouteResponseMethodReturnTypeMapper ModelMapper(IEnumerable<IModelPropertyMapper> propertyMappers)
		{
			_parameterMappers.Add(new ModelMapper(propertyMappers));

			return this;
		}

		public RouteResponseMethodReturnTypeMapper ModelMapper(params IModelPropertyMapper[] propertyMappers)
		{
			_parameterMappers.Add(new ModelMapper(propertyMappers));

			return this;
		}
	}
}
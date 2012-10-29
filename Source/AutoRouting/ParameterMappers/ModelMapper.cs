using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.ParameterMappers.ModelPropertyMappers;

namespace Junior.Route.AutoRouting.ParameterMappers
{
	public class ModelMapper : IParameterMapper
	{
		private readonly IContainer _container;
		private readonly IEnumerable<IModelPropertyMapper> _modelPropertyMappers = Enumerable.Empty<IModelPropertyMapper>();
		private readonly Func<Type, bool> _parameterTypeMatchDelegate;

		public ModelMapper(IContainer container, Func<Type, bool> parameterTypeMatchDelegate, IEnumerable<IModelPropertyMapper> propertyMappers)
		{
			container.ThrowIfNull("container");
			parameterTypeMatchDelegate.ThrowIfNull("parameterTypeMatchDelegate");
			propertyMappers.ThrowIfNull("propertyMappers");

			_container = container;
			_parameterTypeMatchDelegate = parameterTypeMatchDelegate;
			_modelPropertyMappers = propertyMappers;
		}

		public ModelMapper(IContainer container, Func<Type, bool> parameterTypeMatchDelegate, params IModelPropertyMapper[] propertyMappers)
			: this(container, parameterTypeMatchDelegate, (IEnumerable<IModelPropertyMapper>)propertyMappers)
		{
		}

		public ModelMapper(Func<Type, bool> parameterTypeMatchDelegate, IEnumerable<IModelPropertyMapper> propertyMappers)
		{
			parameterTypeMatchDelegate.ThrowIfNull("parameterTypeMatchDelegate");
			propertyMappers.ThrowIfNull("propertyMappers");

			_parameterTypeMatchDelegate = parameterTypeMatchDelegate;
			_modelPropertyMappers = propertyMappers;
		}

		public ModelMapper(Func<Type, bool> parameterTypeMatchDelegate, params IModelPropertyMapper[] propertyMappers)
			: this(parameterTypeMatchDelegate, (IEnumerable<IModelPropertyMapper>)propertyMappers)
		{
		}

		public ModelMapper(IContainer container, IEnumerable<IModelPropertyMapper> propertyMappers)
			: this(container, type => type.Name.EndsWith("Model"), propertyMappers)
		{
		}

		public ModelMapper(IContainer container, params IModelPropertyMapper[] propertyMappers)
			: this(container, type => type.Name.EndsWith("Model"), (IEnumerable<IModelPropertyMapper>)propertyMappers)
		{
		}

		public ModelMapper(IEnumerable<IModelPropertyMapper> propertyMappers)
			: this(type => type.Name.EndsWith("Model"), propertyMappers)
		{
		}

		public ModelMapper(params IModelPropertyMapper[] propertyMappers)
			: this(type => type.Name.EndsWith("Model"), (IEnumerable<IModelPropertyMapper>)propertyMappers)
		{
		}

		public bool CanMapType(HttpRequestBase request, Type parameterType)
		{
			request.ThrowIfNull("request");
			parameterType.ThrowIfNull("parameterType");

			return _parameterTypeMatchDelegate(parameterType);
		}

		public MapResult Map(HttpRequestBase request, Type type, MethodInfo method, ParameterInfo parameter)
		{
			request.ThrowIfNull("request");
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			parameter.ThrowIfNull("parameter");

			Type parameterType = parameter.ParameterType;
			object model = _container != null ? _container.GetInstance(parameterType) : Activator.CreateInstance(parameterType);
			Type modelType = model.GetType();

			foreach (PropertyInfo property in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				PropertyInfo closureProperty = property;
				var mappedValue = _modelPropertyMappers
					.Select(arg => new { Mapper = arg, MapResult = arg.Map(request, modelType, closureProperty) })
					.FirstOrDefault(arg => arg.MapResult.ResultType == MapResultType.ValueMapped);

				if (mappedValue == null)
				{
					throw new ApplicationException(String.Format("Unable to map property '{0} {1}' of type '{2}'.", property.PropertyType.FullName, property.Name, modelType.FullName));
				}

				property.SetValue(model, mappedValue.MapResult.Value, null);
			}

			return MapResult.ValueMapped(model);
		}
	}
}
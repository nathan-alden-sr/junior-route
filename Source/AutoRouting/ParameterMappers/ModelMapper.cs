using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.ParameterMappers.ModelPropertyMappers;

namespace Junior.Route.AutoRouting.ParameterMappers
{
	public class ModelMapper : IParameterMapper
	{
		private readonly IContainer _container;
		private readonly IModelPropertyMapper[] _modelPropertyMappers;
		private readonly Func<Type, bool> _parameterTypeMatchDelegate;

		public ModelMapper(IContainer container, Func<Type, bool> parameterTypeMatchDelegate, IEnumerable<IModelPropertyMapper> propertyMappers)
		{
			container.ThrowIfNull("container");
			parameterTypeMatchDelegate.ThrowIfNull("parameterTypeMatchDelegate");
			propertyMappers.ThrowIfNull("propertyMappers");

			_container = container;
			_parameterTypeMatchDelegate = parameterTypeMatchDelegate;
			_modelPropertyMappers = propertyMappers.ToArray();
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
			_modelPropertyMappers = propertyMappers.ToArray();
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

		public async Task<bool> CanMapTypeAsync(HttpContextBase context, Type parameterType)
		{
			context.ThrowIfNull("context");
			parameterType.ThrowIfNull("parameterType");

			return await Task.Run(() => _parameterTypeMatchDelegate(parameterType));
		}

		public async Task<MapResult> MapAsync(HttpContextBase context, Type type, MethodInfo method, ParameterInfo parameter)
		{
			context.ThrowIfNull("context");
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			parameter.ThrowIfNull("parameter");

			Type parameterType = parameter.ParameterType;
			object model = _container != null ? _container.GetInstance(parameterType) : Activator.CreateInstance(parameterType);
			Type modelType = model.GetType();

			foreach (PropertyInfo property in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				object mappedValue = await GetMappedValueAsync(context, modelType, property);

				if (mappedValue == null)
				{
					throw new ApplicationException(String.Format("Unable to map property '{0} {1}' of type '{2}'.", property.PropertyType.FullName, property.Name, modelType.FullName));
				}

				property.SetValue(model, mappedValue, null);
			}

			return MapResult.ValueMapped(model);
		}

		private async Task<object> GetMappedValueAsync(HttpContextBase context, Type modelType, PropertyInfo property)
		{
			object mappedValue = null;

			foreach (IModelPropertyMapper modelPropertyMapper in _modelPropertyMappers)
			{
				MapResult result = await modelPropertyMapper.MapAsync(context, modelType, property);

				if (result.ResultType == MapResultType.ValueMapped)
				{
					mappedValue = result.Value;
					break;
				}
			}
			return mappedValue;
		}
	}
}
using System;
using System.Reflection;
using System.Web;

using Junior.Common;

namespace Junior.Route.AutoRouting.ParameterMappers.ModelPropertyMappers
{
	public class DefaultValueMapper : IModelPropertyMapper
	{
		public bool CanMapType(Type propertyType)
		{
			propertyType.ThrowIfNull("propertyType");

			return true;
		}

		public MapResult Map(HttpRequestBase request, Type modelType, PropertyInfo property)
		{
			request.ThrowIfNull("request");
			modelType.ThrowIfNull("modelType");
			property.ThrowIfNull("property");

			return MapResult.ValueMapped(property.PropertyType.GetDefaultValue());
		}
	}
}
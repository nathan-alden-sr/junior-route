using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;

namespace Junior.Route.AutoRouting.ParameterMappers.ModelPropertyMappers
{
	public class DefaultValueMapper : IModelPropertyMapper
	{
		public Task<bool> CanMapTypeAsync(Type propertyType)
		{
			propertyType.ThrowIfNull("propertyType");

			return true.AsCompletedTask();
		}

		public Task<MapResult> MapAsync(HttpRequestBase request, Type modelType, PropertyInfo property)
		{
			request.ThrowIfNull("request");
			modelType.ThrowIfNull("modelType");
			property.ThrowIfNull("property");

			return MapResult.ValueMapped(property.PropertyType.GetDefaultValue()).AsCompletedTask();
		}
	}
}
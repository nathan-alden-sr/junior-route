using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;

namespace Junior.Route.AutoRouting.ParameterMappers.ModelPropertyMappers
{
	public class DefaultValueMapper : IModelPropertyMapper
	{
		public Task<bool> CanMapTypeAsync(HttpContextBase context, Type propertyType)
		{
			context.ThrowIfNull("context");
			propertyType.ThrowIfNull("propertyType");

			return true.AsCompletedTask();
		}

		public Task<MapResult> MapAsync(HttpContextBase context, Type modelType, PropertyInfo property)
		{
			context.ThrowIfNull("context");
			modelType.ThrowIfNull("modelType");
			property.ThrowIfNull("property");

			return MapResult.ValueMapped(property.PropertyType.GetDefaultValue()).AsCompletedTask();
		}
	}
}
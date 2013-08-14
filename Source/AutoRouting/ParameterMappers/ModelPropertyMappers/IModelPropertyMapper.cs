using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace Junior.Route.AutoRouting.ParameterMappers.ModelPropertyMappers
{
	public interface IModelPropertyMapper
	{
		Task<bool> CanMapTypeAsync(HttpContextBase context, Type propertyType);
		Task<MapResult> MapAsync(HttpContextBase context, Type modelType, PropertyInfo property);
	}
}
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace Junior.Route.AutoRouting.ParameterMappers.ModelPropertyMappers
{
	public interface IModelPropertyMapper
	{
		Task<bool> CanMapTypeAsync(Type propertyType);
		Task<MapResult> MapAsync(HttpRequestBase request, Type modelType, PropertyInfo property);
	}
}
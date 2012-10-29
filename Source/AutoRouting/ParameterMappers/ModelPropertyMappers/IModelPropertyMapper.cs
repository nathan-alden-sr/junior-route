using System;
using System.Reflection;
using System.Web;

namespace Junior.Route.AutoRouting.ParameterMappers.ModelPropertyMappers
{
	public interface IModelPropertyMapper
	{
		bool CanMapType(Type propertyType);
		MapResult Map(HttpRequestBase request, Type modelType, PropertyInfo property);
	}
}
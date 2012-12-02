using System;
using System.Dynamic;
using System.Runtime.CompilerServices;

using Junior.Common;

namespace Junior.Route.ViewEngines.Razor
{
	public static class TypeExtensions
	{
		public static bool IsDynamicType(this Type type)
		{
			type.ThrowIfNull("type");

			return typeof(DynamicObject).IsAssignableFrom(type) || typeof(ExpandoObject).IsAssignableFrom(type);
		}

		public static bool IsAnonymousType(this Type type)
		{
			type.ThrowIfNull("type");

			return type.IsClass && type.IsSealed && type.BaseType == typeof(object) && type.Name.StartsWith("<>") && type.IsDefined(typeof(CompilerGeneratedAttribute), true);
		}
	}
}
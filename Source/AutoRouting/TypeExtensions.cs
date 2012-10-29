using System;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.AutoRouting
{
	public static class TypeExtensions
	{
		public static bool NamespaceStartsWith(this Type type, string @namespace)
		{
			type.ThrowIfNull("type");
			@namespace.ThrowIfNull("namespace");

			return type.Namespace != null && Regex.IsMatch(type.Namespace, Regex.Escape(@namespace) + @"(\.|$)");
		}

		public static object GetDefaultValue(this Type type)
		{
			type.ThrowIfNull("type");

			return type.IsValueType ? Activator.CreateInstance(type) : null;
		}
	}
}
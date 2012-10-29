using System;

using Junior.Common;

namespace Junior.Route.AutoRouting.ClassFilters
{
	public class InNamespaceFilter : IClassFilter
	{
		private readonly string _namespace;

		public InNamespaceFilter(string @namespace)
		{
			@namespace.ThrowIfNull("@namespace");

			_namespace = @namespace;
		}

		public bool Matches(Type type)
		{
			type.ThrowIfNull("type");

			return type.Namespace == _namespace;
		}
	}
}
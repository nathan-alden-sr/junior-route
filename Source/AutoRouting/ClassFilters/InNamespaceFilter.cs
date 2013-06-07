using System;
using System.Threading.Tasks;

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

		public Task<bool> MatchesAsync(Type type)
		{
			type.ThrowIfNull("type");

			return (type.Namespace == _namespace).AsCompletedTask();
		}
	}
}
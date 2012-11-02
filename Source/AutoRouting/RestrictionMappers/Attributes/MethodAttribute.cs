using System;
using System.Collections.Generic;
using System.Linq;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.Routing;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class MethodAttribute : RestrictionAttribute
	{
		private readonly IEnumerable<string> _methods;

		public MethodAttribute(params string[] methods)
		{
			methods.ThrowIfNull("methods");

			_methods = methods;
		}

		public MethodAttribute(params HttpMethod[] methods)
		{
			methods.ThrowIfNull("methods");

			_methods = methods.Select(arg => arg.ToString().ToUpperInvariant());
		}

		public override void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			route.RestrictByMethods(_methods);
		}
	}
}
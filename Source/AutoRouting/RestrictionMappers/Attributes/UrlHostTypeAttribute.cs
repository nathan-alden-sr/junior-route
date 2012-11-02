using System;
using System.Collections.Generic;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class UrlHostTypeAttribute : RestrictionAttribute
	{
		private readonly IEnumerable<UriHostNameType> _hostTypes;

		public UrlHostTypeAttribute(params UriHostNameType[] hostTypes)
		{
			hostTypes.ThrowIfNull("hostTypes");

			_hostTypes = hostTypes;
		}

		public override void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			route.RestrictByUrlHostTypes(_hostTypes);
		}
	}
}
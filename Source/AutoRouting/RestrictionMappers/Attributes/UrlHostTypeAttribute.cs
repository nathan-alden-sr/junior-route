using System;
using System.Collections.Generic;

using Junior.Common;

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

		public override void Map(Routing.Route route)
		{
			route.ThrowIfNull("route");

			route.RestrictByUrlHostTypes(_hostTypes);
		}
	}
}
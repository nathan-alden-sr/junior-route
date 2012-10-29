using System;
using System.Collections.Generic;

using Junior.Common;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class UrlSchemeAttribute : RestrictionAttribute
	{
		private readonly IEnumerable<string> _schemes;

		public UrlSchemeAttribute(params string[] schemes)
		{
			schemes.ThrowIfNull("schemes");

			_schemes = schemes;
		}

		public override void Map(Routing.Route route)
		{
			route.ThrowIfNull("route");

			route.RestrictByUrlSchemes(_schemes);
		}
	}
}
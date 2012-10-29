using System;
using System.Collections.Generic;

using Junior.Common;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class RefererUrlAuthorityAttribute : RestrictionAttribute
	{
		private readonly IEnumerable<string> _authorities;

		public RefererUrlAuthorityAttribute(params string[] authorities)
		{
			authorities.ThrowIfNull("authorities");

			_authorities = authorities;
		}

		public override void Map(Routing.Route route)
		{
			route.ThrowIfNull("route");

			route.RestrictByRefererUrlAuthorities(_authorities);
		}
	}
}
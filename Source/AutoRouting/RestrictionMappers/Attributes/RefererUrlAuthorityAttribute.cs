using System;
using System.Collections.Generic;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class RefererUrlAuthorityAttribute : RestrictionAttribute
	{
		private readonly IEnumerable<string> _authorities;
		private readonly RequestValueComparer? _comparer;

		public RefererUrlAuthorityAttribute(params string[] authorities)
		{
			authorities.ThrowIfNull("authorities");

			_authorities = authorities;
		}

		public RefererUrlAuthorityAttribute(string authority, RequestValueComparer comparer)
		{
			authority.ThrowIfNull("host");

			_comparer = comparer;
			_authorities = authority.ToEnumerable();
		}

		public override void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			if (_comparer != null)
			{
				route.RestrictByRefererUrlAuthorities(_authorities, GetComparer(_comparer.Value));
			}
			else
			{
				route.RestrictByRefererUrlAuthorities(_authorities);
			}
		}
	}
}
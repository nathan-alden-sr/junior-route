using System;
using System.Collections.Generic;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class UrlAuthorityAttribute : RestrictionAttribute
	{
		private readonly IEnumerable<string> _authorities;
		private readonly RequestValueComparer? _comparer;

		public UrlAuthorityAttribute(params string[] authorities)
		{
			authorities.ThrowIfNull("authorities");

			_authorities = authorities;
		}

		public UrlAuthorityAttribute(string authority, RequestValueComparer comparer)
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
				route.RestrictByUrlAuthorities(_authorities, GetComparer(_comparer.Value));
			}
			else
			{
				route.RestrictByUrlAuthorities(_authorities);
			}
		}
	}
}
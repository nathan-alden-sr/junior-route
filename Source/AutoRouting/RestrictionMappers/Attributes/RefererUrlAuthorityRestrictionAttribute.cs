using System;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class RefererUrlAuthorityRestrictionAttribute : RestrictionAttribute
	{
		private readonly string[] _authorities;
		private readonly RequestValueComparer? _comparer;

		public RefererUrlAuthorityRestrictionAttribute(string authority, RequestValueComparer comparer)
		{
			authority.ThrowIfNull("authority");

			_comparer = comparer;
			_authorities = new[] { authority };
		}

		public RefererUrlAuthorityRestrictionAttribute(params string[] authorities)
		{
			authorities.ThrowIfNull("authorities");

			_authorities = authorities;
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
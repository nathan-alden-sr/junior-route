using System;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class RefererUrlSchemeRestrictionAttribute : RestrictionAttribute
	{
		private readonly RequestValueComparer? _comparer;
		private readonly string[] _schemes;

		public RefererUrlSchemeRestrictionAttribute(string scheme, RequestValueComparer comparer)
		{
			scheme.ThrowIfNull("scheme");

			_comparer = comparer;
			_schemes = new[] { scheme };
		}

		public RefererUrlSchemeRestrictionAttribute(params string[] schemes)
		{
			schemes.ThrowIfNull("schemes");

			_schemes = schemes;
		}

		public override void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			if (_comparer != null)
			{
				route.RestrictByRefererUrlSchemes(_schemes, GetComparer(_comparer.Value));
			}
			else
			{
				route.RestrictByRefererUrlSchemes(_schemes);
			}
		}
	}
}
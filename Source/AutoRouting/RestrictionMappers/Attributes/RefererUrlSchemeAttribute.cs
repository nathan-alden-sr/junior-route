using System;
using System.Collections.Generic;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class RefererUrlSchemeAttribute : RestrictionAttribute
	{
		private readonly RequestValueComparer? _comparer;
		private readonly IEnumerable<string> _schemes;

		public RefererUrlSchemeAttribute(params string[] schemes)
		{
			schemes.ThrowIfNull("schemes");

			_schemes = schemes;
		}

		public RefererUrlSchemeAttribute(string scheme, RequestValueComparer comparer)
		{
			scheme.ThrowIfNull("scheme");

			_comparer = comparer;
			_schemes = scheme.ToEnumerable();
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
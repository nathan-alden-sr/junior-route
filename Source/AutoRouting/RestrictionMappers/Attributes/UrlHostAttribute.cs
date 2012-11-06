using System;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class UrlHostAttribute : RestrictionAttribute
	{
		private readonly RequestValueComparer? _comparer;
		private readonly string[] _hosts;

		public UrlHostAttribute(string host, RequestValueComparer comparer)
		{
			host.ThrowIfNull("host");

			_comparer = comparer;
			_hosts = new[] { host };
		}

		public UrlHostAttribute(params string[] hosts)
		{
			hosts.ThrowIfNull("hosts");

			_hosts = hosts;
		}

		public override void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			if (_comparer != null)
			{
				route.RestrictByUrlHosts(_hosts, GetComparer(_comparer.Value));
			}
			else
			{
				route.RestrictByUrlHosts(_hosts);
			}
		}
	}
}
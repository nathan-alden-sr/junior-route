using System;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class RefererUrlHostAttribute : RestrictionAttribute
	{
		private readonly RequestValueComparer? _comparer;
		private readonly string[] _hosts;

		public RefererUrlHostAttribute(string host, RequestValueComparer comparer)
		{
			host.ThrowIfNull("host");

			_comparer = comparer;
			_hosts = new[] { host };
		}

		public RefererUrlHostAttribute(params string[] hosts)
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
				route.RestrictByRefererUrlHosts(_hosts, GetComparer(_comparer.Value));
			}
			else
			{
				route.RestrictByRefererUrlHosts(_hosts);
			}
		}
	}
}
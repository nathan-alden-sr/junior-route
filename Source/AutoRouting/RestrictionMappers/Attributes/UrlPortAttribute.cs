using System;
using System.Collections.Generic;

using Junior.Common;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class UrlPortAttribute : RestrictionAttribute
	{
		private readonly IEnumerable<ushort> _ports;

		public UrlPortAttribute(params ushort[] ports)
		{
			ports.ThrowIfNull("portTypes");

			_ports = ports;
		}

		public override void Map(Routing.Route route)
		{
			route.ThrowIfNull("route");

			route.RestrictByUrlPorts(_ports);
		}
	}
}
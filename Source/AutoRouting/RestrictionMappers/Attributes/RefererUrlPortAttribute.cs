using System;
using System.Collections.Generic;

using Junior.Common;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class RefererUrlPortAttribute : RestrictionAttribute
	{
		private readonly IEnumerable<ushort> _ports;

		public RefererUrlPortAttribute(params ushort[] ports)
		{
			ports.ThrowIfNull("portTypes");

			_ports = ports;
		}

		public override void Map(Routing.Route route)
		{
			route.ThrowIfNull("route");

			route.RestrictByRefererUrlPorts(_ports);
		}
	}
}
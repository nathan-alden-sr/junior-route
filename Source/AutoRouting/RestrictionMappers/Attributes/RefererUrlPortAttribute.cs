using System;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class RefererUrlPortAttribute : RestrictionAttribute
	{
		private readonly ushort[] _ports;

		public RefererUrlPortAttribute(params ushort[] ports)
		{
			ports.ThrowIfNull("portTypes");

			_ports = ports;
		}

		public override void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			route.RestrictByRefererUrlPorts(_ports);
		}
	}
}
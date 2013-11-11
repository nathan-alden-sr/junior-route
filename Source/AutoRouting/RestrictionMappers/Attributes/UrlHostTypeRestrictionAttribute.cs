using System;
using System.Threading.Tasks;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class UrlHostTypeRestrictionAttribute : RestrictionAttribute
	{
		private readonly UriHostNameType[] _hostTypes;

		public UrlHostTypeRestrictionAttribute(params UriHostNameType[] hostTypes)
		{
			hostTypes.ThrowIfNull("hostTypes");

			_hostTypes = hostTypes;
		}

		public override void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			route.RestrictByUrlHostTypes(_hostTypes);
		}
	}
}
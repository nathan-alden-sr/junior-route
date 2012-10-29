using System;

namespace Junior.Route.Common
{
	public interface IUrlResolver
	{
		string Absolute(string relativeUrl);
		string Route(string routeName);
		string Route(Guid routeId);
	}
}
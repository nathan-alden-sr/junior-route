using System;

namespace Junior.Route.Common
{
	public interface IUrlResolver
	{
		string Absolute(string relativeUrl, params object[] args);
		string Route(string routeName, params object[] args);
		string Route(Guid routeId, params object[] args);
	}
}
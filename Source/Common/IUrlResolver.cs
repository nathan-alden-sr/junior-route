using System;

namespace Junior.Route.Common
{
	public interface IUrlResolver
	{
		string Absolute(Scheme scheme, string relativeUrl, params object[] args);
		string Absolute(string relativeUrl, params object[] args);
		string Route(Scheme scheme, string routeName, params object[] args);
		string Route(string routeName, params object[] args);
		string Route(Scheme scheme, Guid routeId, params object[] args);
		string Route(Guid routeId, params object[] args);
	}
}
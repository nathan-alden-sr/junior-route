using Junior.Common;

using NathanAlden.JuniorRouting.Core;

namespace NathanAlden.JuniorRouting.Diagnostics
{
	public static class HttpRoutesExtensions
	{
		public static HttpRoutes AddDiagnostics(this HttpRoutes routes, string baseUrl = "_diagnostics")
		{
			routes.ThrowIfNull("routes");

			DiagnosticRouteHelper.Instance.AddRoutes(routes, baseUrl);

			return routes;
		}
	}
}
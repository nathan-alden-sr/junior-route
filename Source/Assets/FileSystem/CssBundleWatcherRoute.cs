using System;
using System.Web;

using Junior.Common;
using Junior.Route.Routing;
using Junior.Route.Routing.Responses;
using Junior.Route.Routing.Responses.Text;

namespace Junior.Route.Assets.FileSystem
{
	public class CssBundleWatcherRoute : BundleWatcherRoute<CssResponse>
	{
		private readonly ISystemClock _systemClock;

		public CssBundleWatcherRoute(string name, IGuidFactory guidFactory, string relativePath, BundleWatcher watcher, IHttpRuntime httpRuntime, ISystemClock systemClock)
			: base(name, guidFactory, relativePath, watcher, httpRuntime)
		{
			systemClock.ThrowIfNull("systemClock");

			_systemClock = systemClock;
		}

		public CssBundleWatcherRoute(string name, Guid id, string relativePath, BundleWatcher watcher, IHttpRuntime httpRuntime, ISystemClock systemClock)
			: base(name, id, relativePath, watcher, httpRuntime)
		{
			systemClock.ThrowIfNull("systemClock");

			_systemClock = systemClock;
		}

		protected override CssResponse GetResponse(HttpRequestBase request, string bundleContents)
		{
			request.ThrowIfNull("request");
			bundleContents.ThrowIfNull("bundleContents");

			return new CssResponse(bundleContents, ConfigureResponse);
		}

		private void ConfigureResponse(Response response)
		{
			DateTime expires = _systemClock.UtcDateTime.AddYears(1);

			response.CachePolicy
				.ServerCaching()
				.PublicClientCaching(expires)
				.ETag(Id.ToString("N"));
		}
	}
}
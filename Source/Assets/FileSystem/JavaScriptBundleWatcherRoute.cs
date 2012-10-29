using System;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.Responses;
using Junior.Route.Routing.Responses.Application;

namespace Junior.Route.Assets.FileSystem
{
	public class JavaScriptBundleWatcherRoute : BundleWatcherRoute<JavaScriptResponse>
	{
		private readonly ISystemClock _systemClock;

		public JavaScriptBundleWatcherRoute(string name, IGuidFactory guidFactory, string relativeUrl, BundleWatcher watcher, ISystemClock systemClock)
			: base(name, guidFactory, relativeUrl, watcher)
		{
			systemClock.ThrowIfNull("systemClock");

			_systemClock = systemClock;
		}

		public JavaScriptBundleWatcherRoute(string name, Guid id, string relativeUrl, BundleWatcher watcher, ISystemClock systemClock)
			: base(name, id, relativeUrl, watcher)
		{
			systemClock.ThrowIfNull("systemClock");

			_systemClock = systemClock;
		}

		protected override JavaScriptResponse GetResponse(HttpRequestBase request, string bundleContents)
		{
			request.ThrowIfNull("request");
			bundleContents.ThrowIfNull("bundleContents");

			return new JavaScriptResponse(bundleContents, ConfigureResponse);
		}

		private void ConfigureResponse(Response response)
		{
			DateTime expires = _systemClock.UtcDateTime.AddYears(1);

			response
				.CacheInPublicClientCacheAndServerCache(expires)
				.ETag(Id.ToString("N"));
		}
	}
}
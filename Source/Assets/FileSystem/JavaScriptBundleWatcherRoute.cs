using System;
using System.Web;

using Junior.Common;
using Junior.Route.Routing;
using Junior.Route.Routing.Responses;
using Junior.Route.Routing.Responses.Application;

namespace Junior.Route.Assets.FileSystem
{
	public class JavaScriptBundleWatcherRoute : BundleWatcherRoute<JavaScriptResponse>
	{
		private readonly ISystemClock _systemClock;

		public JavaScriptBundleWatcherRoute(string name, IGuidFactory guidFactory, string relativePath, BundleWatcher watcher, IHttpRuntime httpRuntime, ISystemClock systemClock)
			: base(name, guidFactory, relativePath, watcher, httpRuntime)
		{
			systemClock.ThrowIfNull("systemClock");

			_systemClock = systemClock;
		}

		public JavaScriptBundleWatcherRoute(string name, Guid id, string relativePath, BundleWatcher watcher, IHttpRuntime httpRuntime, ISystemClock systemClock)
			: base(name, id, relativePath, watcher, httpRuntime)
		{
			systemClock.ThrowIfNull("systemClock");

			_systemClock = systemClock;
		}

		protected override JavaScriptResponse GetResponse(HttpContextBase context, string bundleContents)
		{
			context.ThrowIfNull("context");
			bundleContents.ThrowIfNull("bundleContents");

			return new JavaScriptResponse(bundleContents, ConfigureResponse);
		}

		private void ConfigureResponse(Response response)
		{
			DateTime expirationUtcTimestamp = _systemClock.UtcDateTime.AddYears(1);

			response.CachePolicy
				.ServerCaching(expirationUtcTimestamp)
				.PublicClientCaching(expirationUtcTimestamp)
				.ETag(Id.ToString("N"));
		}
	}
}
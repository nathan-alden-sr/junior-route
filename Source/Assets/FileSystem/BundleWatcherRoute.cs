using System;
using System.Web;

using Junior.Common;
using Junior.Route.Common;
using Junior.Route.Routing;
using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Responses;

namespace Junior.Route.Assets.FileSystem
{
	public abstract class BundleWatcherRoute<T> : Routing.Route
		where T : class, IResponse
	{
		private readonly IHttpRuntime _httpRuntime;
		private readonly object _lockObject = new object();
		private readonly string _relativePath;
		private readonly BundleWatcher _watcher;

		protected BundleWatcherRoute(string name, Guid id, Scheme scheme, string relativePath, BundleWatcher watcher, IHttpRuntime httpRuntime)
			: base(name, id, scheme)
		{
			relativePath.ThrowIfNull("relativePath");
			watcher.ThrowIfNull("watcher");
			httpRuntime.ThrowIfNull("httpRuntime");

			_relativePath = relativePath;
			_watcher = watcher;
			_httpRuntime = httpRuntime;
			_watcher.BundleChanged += WatcherBundleChanged;
			ConfigureRoute();
		}

		protected abstract T GetResponse(HttpContextBase context, string bundleContents);

		private void ConfigureRoute()
		{
			lock (_lockObject)
			{
				ResolvedRelativeUrl = String.Format("{0}?v={1}", _relativePath, _watcher.Hash);
				ClearRestrictions();
				RestrictByMethods(HttpMethod.Get);
				RestrictByUrlRelativePath(_relativePath, CaseInsensitivePlainComparer.Instance, _httpRuntime);
				RestrictByUrlQuery(@"^(?:\?(?i:v=[a-f0-9]{32}))?$", CaseInsensitiveRegexComparer.Instance);
				RespondWith(context => GetResponse(context, _watcher.Contents));
			}
		}

		private void WatcherBundleChanged()
		{
			ConfigureRoute();
		}
	}
}
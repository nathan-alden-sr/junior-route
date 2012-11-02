using System;
using System.Web;

using Junior.Common;
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

		protected BundleWatcherRoute(string name, IGuidFactory guidFactory, string relativePath, BundleWatcher watcher, IHttpRuntime httpRuntime)
			: base(name, guidFactory)
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

		protected BundleWatcherRoute(string name, Guid id, string relativePath, BundleWatcher watcher, IHttpRuntime httpRuntime)
			: base(name, id)
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

		protected abstract T GetResponse(HttpRequestBase request, string bundleContents);

		private void ConfigureRoute()
		{
			lock (_lockObject)
			{
				ResolvedRelativeUrl = String.Format("{0}?v={1}", _relativePath, _watcher.Hash);
				ClearRestrictions();
				RestrictByMethods(HttpMethod.Get);
				RestrictByRelativePath(_relativePath, CaseInsensitivePlainRequestValueComparer.Instance, _httpRuntime);
				RestrictByUrlQuery(@"^(?:\?(?i:v=[a-f0-9]{32}))?$", CaseInsensitiveRegexRequestValueComparer.Instance);
				RespondWith(request => GetResponse(request, _watcher.Contents));
			}
		}

		private void WatcherBundleChanged()
		{
			ConfigureRoute();
		}
	}
}
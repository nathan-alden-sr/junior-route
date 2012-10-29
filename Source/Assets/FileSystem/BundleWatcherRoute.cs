using System;
using System.Text.RegularExpressions;
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
		private readonly object _lockObject = new object();
		private readonly string _relativeUrl;
		private readonly BundleWatcher _watcher;

		protected BundleWatcherRoute(string name, IGuidFactory guidFactory, string relativeUrl, BundleWatcher watcher)
			: base(name, guidFactory)
		{
			relativeUrl.ThrowIfNull("relativeUrl");
			watcher.ThrowIfNull("watcher");

			_relativeUrl = relativeUrl;
			_watcher = watcher;
			_watcher.BundleChanged += WatcherBundleChanged;
			ConfigureRoute();
		}

		protected BundleWatcherRoute(string name, Guid id, string relativeUrl, BundleWatcher watcher)
			: base(name, id)
		{
			relativeUrl.ThrowIfNull("relativeUrl");
			watcher.ThrowIfNull("watcher");

			_relativeUrl = relativeUrl;
			_watcher = watcher;
			_watcher.BundleChanged += WatcherBundleChanged;
			ConfigureRoute();
		}

		protected abstract T GetResponse(HttpRequestBase request, string bundleContents);

		private void ConfigureRoute()
		{
			lock (_lockObject)
			{
				ResolvedRelativeUrl = String.Format("{0}?v={1}", _relativeUrl, _watcher.Hash);
				ClearRestrictions();
				RestrictByMethods(HttpMethod.Get);
				RestrictByRelativeUrl(String.Format(@"^{0}(?:$|\?(?i:v=[a-f0-9]{{32}}))", Regex.Escape(_relativeUrl)), CaseInsensitiveRegexRequestValueComparer.Instance);
				RespondWith(request => GetResponse(request, _watcher.Contents));
			}
		}

		private void WatcherBundleChanged()
		{
			ConfigureRoute();
		}
	}
}
using System;
using System.IO;

using NathanAlden.JuniorRouting.Core;
using NathanAlden.JuniorRouting.Core.Responses;
using NathanAlden.JuniorRouting.Diagnostics.Web;

using Spark;
using Spark.FileSystem;

namespace NathanAlden.JuniorRouting.Diagnostics
{
	public class DiagnosticRouteHelper
	{
		public static readonly DiagnosticRouteHelper Instance = new DiagnosticRouteHelper();

		private DiagnosticRouteHelper()
		{
		}

		public void AddRoutes(HttpRoutes routes, string baseUrl)
		{
			baseUrl = baseUrl.TrimEnd('/');

			routes.Add(GetViewRoute<DiagnosticsView>(baseUrl, ResponseResources.Diagnostics, view => view.Populate(baseUrl)));
			routes.Add(GetViewRoute<RouteTableView>(baseUrl + "/route_table", ResponseResources.RouteTable, view => view.Populate(routes, baseUrl)));
			routes.Add(GetStylesheetRoute(baseUrl + "/stylesheets/reset", ResponseResources.reset));
			routes.Add(GetStylesheetRoute(baseUrl + "/stylesheets/common", ResponseResources.common));
			routes.Add(GetStylesheetRoute(baseUrl + "/stylesheets/route-table-view", ResponseResources.route_table_view));
		}

		private static HttpRoute GetViewRoute<T>(string url, byte[] viewMarkup, Action<T> populateViewDelegate = null)
			where T : View
		{
			return HttpRoute.Create()
				.Get()
				.RelativeUrl(url)
				.Response(() => GetViewResponse(viewMarkup, populateViewDelegate));
		}

		private static HttpRoute GetStylesheetRoute(string url, string stylesheet)
		{
			return HttpRoute.Create()
				.Get()
				.RelativeUrl(url)
				.Response(GetCssResponse(stylesheet));
		}

		private static ContentResponse GetViewResponse<T>(byte[] viewMarkup, Action<T> populateViewDelegate = null)
			where T : View
		{
			return ContentResponse
				.OK()
				.TextHtml()
				.Content(() =>
					{
						Type viewType = typeof(T);
						SparkSettings settings = new SparkSettings()
							.SetPageBaseType(viewType)
							.SetDebug(false)
							.AddNamespace("System")
							.AddNamespace("System.Linq")
							.AddNamespace("System.Web")
							.AddNamespace("NathanAlden.JuniorRouting.Core")
							.AddNamespace("NathanAlden.JuniorRouting.Core.RequestValueComparers")
							.AddNamespace("NathanAlden.JuniorRouting.Core.Responses")
							.AddNamespace("NathanAlden.JuniorRouting.Core.Restrictions");
						var container = new SparkServiceContainer(settings);
						var viewFolder = new InMemoryViewFolder();

						container.SetServiceBuilder<IViewFolder>(arg => viewFolder);
						var viewEngine = container.GetService<ISparkViewEngine>();
						string viewKey = viewType.Name.Replace("View", "") + ".spark";
						const string applicationKey = @"Layouts\application.spark";

						viewFolder.AddLayoutsPath("Layouts");
						viewFolder.Add(viewKey, viewMarkup);
						viewFolder.Add(applicationKey, ResponseResources.Application);

						SparkViewDescriptor descriptor = new SparkViewDescriptor()
							.AddTemplate(viewKey)
							.AddTemplate(applicationKey);
						var view = (T)viewEngine.CreateInstance(descriptor);

						try
						{
							if (populateViewDelegate != null)
							{
								populateViewDelegate(view);
							}

							var writer = new StringWriter();

							view.RenderView(writer);

							return writer.ToString();
						}
						finally
						{
							viewEngine.ReleaseInstance(view);
						}
					});
		}

		private static ContentResponse GetCssResponse(string stylesheet)
		{
			return ContentResponse
				.OK()
				.TextCss()
				.Content(stylesheet);
		}
	}
}
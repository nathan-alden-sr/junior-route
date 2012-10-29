using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Junior.Common;
using Junior.Route.Diagnostics.Web;
using Junior.Route.Routing;
using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Responses.Application;
using Junior.Route.Routing.Responses.Text;

using Spark;
using Spark.FileSystem;

namespace Junior.Route.Diagnostics
{
	public class DiagnosticRouteHelper
	{
		public static readonly DiagnosticRouteHelper Instance = new DiagnosticRouteHelper();

		private DiagnosticRouteHelper()
		{
		}

		public Routing.Route GetViewRoute<T>(string name, IGuidFactory guidFactory, string relativeUrl, byte[] viewTemplate, IEnumerable<string> namespaces, Action<T> populateView = null)
			where T : AbstractSparkView
		{
			guidFactory.ThrowIfNull("guidFactory");
			relativeUrl.ThrowIfNull("relativeUrl");
			viewTemplate.ThrowIfNull("viewTemplate");
			namespaces.ThrowIfNull("namespaces");

			return new Routing.Route(name, guidFactory.Random(), relativeUrl)
				.RestrictByMethods(HttpMethod.Get)
				.RestrictByRelativeUrls(relativeUrl)
				.RespondWith(request => GetViewResponse(viewTemplate, namespaces, populateView));
		}

		public Routing.Route GetStylesheetRoute(string name, IGuidFactory guidFactory, string relativeUrl, string stylesheet)
		{
			guidFactory.ThrowIfNull("guidFactory");
			relativeUrl.ThrowIfNull("relativeUrl");
			stylesheet.ThrowIfNull("stylesheet");

			return new Routing.Route(name, guidFactory.Random(), relativeUrl)
				.RestrictByMethods(HttpMethod.Get)
				.RestrictByRelativeUrls(relativeUrl)
				.RespondWith(request => new CssResponse(stylesheet));
		}

		public Routing.Route GetJavaScriptRoute(string name, IGuidFactory guidFactory, string relativeUrl, string javaScript)
		{
			guidFactory.ThrowIfNull("guidFactory");
			relativeUrl.ThrowIfNull("relativeUrl");
			javaScript.ThrowIfNull("javaScript");

			return new Routing.Route(name, guidFactory.Random(), relativeUrl)
				.RestrictByMethods(HttpMethod.Get)
				.RestrictByRelativeUrls(relativeUrl)
				.RespondWith(request => new JavaScriptResponse(javaScript));
		}

		private static HtmlResponse GetViewResponse<T>(byte[] viewTemplate, IEnumerable<string> namespaces, Action<T> populateView = null)
			where T : AbstractSparkView
		{
			return new HtmlResponse(() =>
				{
					Type viewType = typeof(T);
					SparkSettings settings = new SparkSettings()
						.SetPageBaseType(viewType)
						.SetDebug(false);

					foreach (string @namespace in namespaces.Distinct())
					{
						settings.AddNamespace(@namespace);
					}

					var container = new SparkServiceContainer(settings);
					var viewFolder = new InMemoryViewFolder();

					container.SetServiceBuilder<IViewFolder>(arg => viewFolder);
					var viewEngine = container.GetService<ISparkViewEngine>();
					string viewKey = viewType.Name.Replace("View", "") + ".spark";
					const string applicationKey = @"Layouts\application.spark";

					viewFolder.AddLayoutsPath("Layouts");
					viewFolder.Add(viewKey, viewTemplate);
					viewFolder.Add(applicationKey, ResponseResources.Application);

					SparkViewDescriptor descriptor = new SparkViewDescriptor()
						.AddTemplate(viewKey)
						.AddTemplate(applicationKey);
					var view = (T)viewEngine.CreateInstance(descriptor);

					try
					{
						if (populateView != null)
						{
							populateView(view);
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
	}
}
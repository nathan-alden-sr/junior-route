using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.AspNetIntegration.RequestFilters;

namespace Junior.Route.AspNetIntegration
{
	public static class JuniorRouteApplication
	{
		private static JuniorRouteApplicationConfiguration _configuration;

		public static void RegisterConfiguration<T>()
			where T : JuniorRouteApplicationConfiguration, new()
		{
			_configuration = new T();
		}

		public static void RegisterConfiguration(JuniorRouteApplicationConfiguration configuration)
		{
			configuration.ThrowIfNull("configuration");

			_configuration = configuration;
		}

		public static void AttachToHttpApplication(HttpApplication application)
		{
			application.ThrowIfNull("application");

			if (_configuration == null)
			{
				throw new InvalidOperationException("No configuration was provided.");
			}

			var postAuthenticateRequestHandler = new EventHandlerTaskAsyncHelper(OnApplicationOnPostAuthenticateRequest);

			application.AddOnPostAuthenticateRequestAsync(postAuthenticateRequestHandler.BeginEventHandler, postAuthenticateRequestHandler.EndEventHandler);
		}

		private static async Task OnApplicationOnPostAuthenticateRequest(object sender, EventArgs e)
		{
			var context = new HttpContextWrapper(HttpContext.Current);
			IRequestFilter[] requestFilters = _configuration.RequestFilters.ToArray();

			if (!requestFilters.Any())
			{
				context.RemapHandler(_configuration.HttpHandler);
				return;
			}

			foreach (IRequestFilter requestFilter in _configuration.RequestFilters)
			{
				if ((await requestFilter.FilterAsync(context)).ResultType != FilterResultType.UseJuniorRouteHandler)
				{
					continue;
				}

				context.RemapHandler(_configuration.HttpHandler);
				return;
			}
		}
	}
}
using System;
using System.Web;

using Junior.Common;

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

			application.PostAuthenticateRequest += (sender, args) => HttpContext.Current.RemapHandler(_configuration.Handler);
		}
	}
}
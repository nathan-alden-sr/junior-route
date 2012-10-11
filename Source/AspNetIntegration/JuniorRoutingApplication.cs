using System;
using System.Collections.Generic;

using Junior.Common;

using NathanAlden.JuniorRouting.AspNetIntegration.AspNet;
using NathanAlden.JuniorRouting.Core;

namespace NathanAlden.JuniorRouting.AspNetIntegration
{
	public class JuniorRoutingApplication
	{
		private readonly JuniorRoutingApplicationConfiguration _configuration;
		private readonly Func<IEnumerable<HttpRoute>, IHttpApplication> _httpApplicationFactoryDelegate;
		private IHttpApplication _httpApplication;

		private JuniorRoutingApplication(JuniorRoutingApplicationConfiguration configuration, Func<IEnumerable<HttpRoute>, IHttpApplication> httpApplicationFactoryDelegate)
		{
			configuration.ThrowIfNull("configuration");
			httpApplicationFactoryDelegate.ThrowIfNull("httpApplicationFactoryDelegate");

			_configuration = configuration;
			_httpApplicationFactoryDelegate = httpApplicationFactoryDelegate;
		}

		public void Initialize()
		{
			_httpApplication = _httpApplicationFactoryDelegate(_configuration.Routes);
		}

		public static JuniorRoutingApplication CreateWithHttpApplicationFactory(JuniorRoutingApplicationConfiguration configuration, IHttpApplicationFactory httpApplicationFactory)
		{
			configuration.ThrowIfNull("configuration");
			httpApplicationFactory.ThrowIfNull("httpApplicationFactory");

			return new JuniorRoutingApplication(configuration, httpApplicationFactory.Create);
		}

		public static JuniorRoutingApplication CreateWithHttpApplicationFactory<T>(IHttpApplicationFactory httpApplicationFactory)
			where T : JuniorRoutingApplicationConfiguration, new()
		{
			httpApplicationFactory.ThrowIfNull("httpApplicationFactory");

			var configuration = new T();

			return new JuniorRoutingApplication(configuration, httpApplicationFactory.Create);
		}

		public static JuniorRoutingApplication CreateWithHttpApplicationFactory(JuniorRoutingApplicationConfiguration configuration, Func<IEnumerable<HttpRoute>, IHttpApplication> httpApplicationFactoryDelegate)
		{
			configuration.ThrowIfNull("configuration");
			httpApplicationFactoryDelegate.ThrowIfNull("httpApplicationFactoryDelegate");

			return new JuniorRoutingApplication(configuration, httpApplicationFactoryDelegate);
		}

		public static JuniorRoutingApplication CreateWithHttpApplicationFactory<T>(Func<IEnumerable<HttpRoute>, IHttpApplication> httpApplicationFactoryDelegate)
			where T : JuniorRoutingApplicationConfiguration, new()
		{
			httpApplicationFactoryDelegate.ThrowIfNull("httpApplicationFactoryDelegate");

			var configuration = new T();

			return new JuniorRoutingApplication(configuration, httpApplicationFactoryDelegate);
		}

		public static JuniorRoutingApplication CreateWithDefaultHttpApplicationFactory(JuniorRoutingApplicationConfiguration configuration)
		{
			configuration.ThrowIfNull("configuration");

			return new JuniorRoutingApplication(configuration, new AspNetHttpApplicationFactory().Create);
		}

		public static JuniorRoutingApplication CreateWithDefaultHttpApplicationFactory<T>()
			where T : JuniorRoutingApplicationConfiguration, new()
		{
			var configuration = new T();

			return new JuniorRoutingApplication(configuration, new AspNetHttpApplicationFactory().Create);
		}
	}
}
using System.Collections.Generic;

using NathanAlden.JuniorRouting.Core;

namespace NathanAlden.JuniorRouting.AspNetIntegration.AspNet
{
	public class AspNetHttpApplicationFactory : IHttpApplicationFactory
	{
		public IHttpApplication Create(IEnumerable<HttpRoute> routes)
		{
			return new AspNetHttpApplication(routes);
		}
	}
}
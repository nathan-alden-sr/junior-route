using System.Collections.Generic;

using NathanAlden.JuniorRouting.Core;

namespace NathanAlden.JuniorRouting.AspNetIntegration.AspNet
{
	public interface IHttpApplicationFactory
	{
		IHttpApplication Create(IEnumerable<HttpRoute> routes);
	}
}
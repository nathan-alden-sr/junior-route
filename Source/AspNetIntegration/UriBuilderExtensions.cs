using System;

using Junior.Common;

namespace Junior.Route.AspNetIntegration
{
	public static class UriBuilderExtensions
	{
		public static Uri GetUriWithoutOptionalPort(this UriBuilder uriBuilder)
		{
			uriBuilder.ThrowIfNull("uriBuilder");

			string scheme = uriBuilder.Scheme.ToLowerInvariant();

			return (scheme == "http" && uriBuilder.Port == 80) || (scheme == "https" && uriBuilder.Port == 443)
				? new Uri(uriBuilder.Uri.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.Port, UriFormat.Unescaped))
				: new Uri(uriBuilder.Uri.GetComponents(UriComponents.AbsoluteUri, UriFormat.Unescaped));
		}
	}
}
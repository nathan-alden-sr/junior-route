using System;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class RefererHeader
	{
		private readonly Uri _url;

		private RefererHeader(Uri url)
		{
			url.ThrowIfNull("url");

			_url = url;
		}

		public Uri Url
		{
			get
			{
				return _url;
			}
		}

		public static RefererHeader Parse(string headerValue)
		{
			if (headerValue == null)
			{
				return null;
			}

			Uri url;

			return Uri.TryCreate(headerValue, UriKind.RelativeOrAbsolute, out url) ? new RefererHeader(url) : null;
		}
	}
}
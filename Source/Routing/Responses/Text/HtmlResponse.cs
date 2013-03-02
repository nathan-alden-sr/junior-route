using System;
using System.Text;

using Junior.Common;
using Junior.Route.Routing.AntiCsrf;

namespace Junior.Route.Routing.Responses.Text
{
	public class HtmlResponse : ImmutableResponse
	{
		public HtmlResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextHtml().Content(content), configurationDelegate)
		{
		}

		public HtmlResponse(Func<byte[]> content, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextHtml().Content(content), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		public HtmlResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public HtmlResponse(Func<byte[]> content, Encoding encoding, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextHtml().ContentEncoding(encoding).Content(content), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		public HtmlResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextHtml().Content(content), configurationDelegate)
		{
		}

		public HtmlResponse(Func<string> content, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextHtml().Content(content), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		public HtmlResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public HtmlResponse(Func<string> content, Encoding encoding, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextHtml().ContentEncoding(encoding).Content(content), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		public HtmlResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextHtml().Content(content), configurationDelegate)
		{
		}

		public HtmlResponse(byte[] content, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextHtml().Content(content), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		public HtmlResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public HtmlResponse(byte[] content, Encoding encoding, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextHtml().ContentEncoding(encoding).Content(content), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		public HtmlResponse(string content, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextHtml().Content(content), configurationDelegate)
		{
		}

		public HtmlResponse(string content, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextHtml().Content(content), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		public HtmlResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public HtmlResponse(string content, Encoding encoding, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextHtml().ContentEncoding(encoding).Content(content), response => AntiCsrf(response, antiCsrfData, configurationDelegate))
		{
		}

		private static Response AntiCsrf(Response response, AntiCsrfData antiCsrfData, Action<Response> configurationDelegate)
		{
			antiCsrfData.ThrowIfNull("antiCsrfData");

			response.Cookie(antiCsrfData.Cookie);

			if (configurationDelegate != null)
			{
				configurationDelegate(response);
			}

			return response;
		}
	}
}
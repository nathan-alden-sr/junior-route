using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Text
{
	public class HtmlResponse : ImmutableResponse
	{
		public HtmlResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().Content(content), configurationDelegate)
		{
		}

		public HtmlResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public HtmlResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().Content(content), configurationDelegate)
		{
		}

		public HtmlResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public HtmlResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().Content(content), configurationDelegate)
		{
		}

		public HtmlResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public HtmlResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().Content(content), configurationDelegate)
		{
		}

		public HtmlResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
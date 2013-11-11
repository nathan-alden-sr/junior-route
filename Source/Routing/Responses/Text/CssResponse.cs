using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Text
{
	public class CssResponse : ImmutableResponse
	{
		public CssResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().TextCss().Content(content), configurationDelegate)
		{
		}

		public CssResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextCss().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public CssResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().TextCss().Content(content), configurationDelegate)
		{
		}

		public CssResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextCss().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public CssResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().TextCss().Content(content), configurationDelegate)
		{
		}

		public CssResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextCss().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public CssResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().TextCss().Content(content), configurationDelegate)
		{
		}

		public CssResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextCss().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public CssResponse(Action<Response> configurationDelegate = null)
			: base(new Response().TextCss(), configurationDelegate)
		{
		}

		public CssResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextCss().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
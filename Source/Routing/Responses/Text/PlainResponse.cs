using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Text
{
	public class PlainResponse : ImmutableResponse
	{
		public PlainResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().Content(content), configurationDelegate)
		{
		}

		public PlainResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public PlainResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().Content(content), configurationDelegate)
		{
		}

		public PlainResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public PlainResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().Content(content), configurationDelegate)
		{
		}

		public PlainResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public PlainResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().Content(content), configurationDelegate)
		{
		}

		public PlainResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public PlainResponse(Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain(), configurationDelegate)
		{
		}

		public PlainResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
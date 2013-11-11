using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Text
{
	public class VCardResponse : ImmutableResponse
	{
		public VCardResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().TextVCard().Content(content), configurationDelegate)
		{
		}

		public VCardResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextVCard().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public VCardResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().TextVCard().Content(content), configurationDelegate)
		{
		}

		public VCardResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextVCard().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public VCardResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().TextVCard().Content(content), configurationDelegate)
		{
		}

		public VCardResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextVCard().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public VCardResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().TextVCard().Content(content), configurationDelegate)
		{
		}

		public VCardResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextVCard().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public VCardResponse(Action<Response> configurationDelegate = null)
			: base(new Response().TextVCard(), configurationDelegate)
		{
		}

		public VCardResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextVCard().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
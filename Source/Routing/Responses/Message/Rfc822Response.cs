using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Message
{
	public class Rfc822Response : ImmutableResponse
	{
		public Rfc822Response(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().MessageRfc822().Content(content), configurationDelegate)
		{
		}

		public Rfc822Response(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MessageRfc822().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public Rfc822Response(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().MessageRfc822().Content(content), configurationDelegate)
		{
		}

		public Rfc822Response(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MessageRfc822().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public Rfc822Response(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().MessageRfc822().Content(content), configurationDelegate)
		{
		}

		public Rfc822Response(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MessageRfc822().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public Rfc822Response(string content, Action<Response> configurationDelegate = null)
			: base(new Response().MessageRfc822().Content(content), configurationDelegate)
		{
		}

		public Rfc822Response(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MessageRfc822().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public Rfc822Response(Action<Response> configurationDelegate = null)
			: base(new Response().MessageRfc822(), configurationDelegate)
		{
		}

		public Rfc822Response(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MessageRfc822().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
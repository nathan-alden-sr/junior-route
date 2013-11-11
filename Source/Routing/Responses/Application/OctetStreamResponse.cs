using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Application
{
	public class OctetStreamResponse : ImmutableResponse
	{
		public OctetStreamResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationOctetStream().Content(content), configurationDelegate)
		{
		}

		public OctetStreamResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationOctetStream().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public OctetStreamResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationOctetStream().Content(content), configurationDelegate)
		{
		}

		public OctetStreamResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationOctetStream().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public OctetStreamResponse(Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationOctetStream(), configurationDelegate)
		{
		}

		public OctetStreamResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationOctetStream().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
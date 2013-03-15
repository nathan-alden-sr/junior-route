using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Audio
{
	public class WebMResponse : ImmutableResponse
	{
		public WebMResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().AudioWebM().Content(content), configurationDelegate)
		{
		}

		public WebMResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioWebM().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public WebMResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().AudioWebM().Content(content), configurationDelegate)
		{
		}

		public WebMResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioWebM().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
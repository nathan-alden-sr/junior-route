using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Audio
{
	public class WavResponse : ImmutableResponse
	{
		public WavResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().AudioWav().Content(content), configurationDelegate)
		{
		}

		public WavResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioWav().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public WavResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().AudioWav().Content(content), configurationDelegate)
		{
		}

		public WavResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioWav().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
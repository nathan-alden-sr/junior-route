using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Audio
{
	public class L24Response : ImmutableResponse
	{
		public L24Response(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().AudioL24().Content(content), configurationDelegate)
		{
		}

		public L24Response(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioL24().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public L24Response(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().AudioL24().Content(content), configurationDelegate)
		{
		}

		public L24Response(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioL24().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public L24Response(Action<Response> configurationDelegate = null)
			: base(new Response().AudioL24(), configurationDelegate)
		{
		}

		public L24Response(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioL24().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
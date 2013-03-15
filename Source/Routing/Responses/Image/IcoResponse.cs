using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Image
{
	public class IcoResponse : ImmutableResponse
	{
		public IcoResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ImageIco().Content(content), configurationDelegate)
		{
		}

		public IcoResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ImageIco().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public IcoResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ImageIco().Content(content), configurationDelegate)
		{
		}

		public IcoResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ImageIco().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
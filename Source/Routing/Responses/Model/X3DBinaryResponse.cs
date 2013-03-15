using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Model
{
	public class X3DBinaryResponse : ImmutableResponse
	{
		public X3DBinaryResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DBinary().Content(content), configurationDelegate)
		{
		}

		public X3DBinaryResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DBinary().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public X3DBinaryResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DBinary().Content(content), configurationDelegate)
		{
		}

		public X3DBinaryResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DBinary().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
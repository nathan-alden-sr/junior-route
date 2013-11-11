using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Audio
{
	public class BasicResponse : ImmutableResponse
	{
		public BasicResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().AudioBasic().Content(content), configurationDelegate)
		{
		}

		public BasicResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioBasic().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public BasicResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().AudioBasic().Content(content), configurationDelegate)
		{
		}

		public BasicResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioBasic().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public BasicResponse(Action<Response> configurationDelegate = null)
			: base(new Response().AudioBasic(), configurationDelegate)
		{
		}

		public BasicResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioBasic().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
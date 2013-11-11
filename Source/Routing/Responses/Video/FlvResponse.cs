using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Video
{
	public class FlvResponse : ImmutableResponse
	{
		public FlvResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().VideoFlv().Content(content), configurationDelegate)
		{
		}

		public FlvResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().VideoFlv().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public FlvResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().VideoFlv().Content(content), configurationDelegate)
		{
		}

		public FlvResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().VideoFlv().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public FlvResponse(Action<Response> configurationDelegate = null)
			: base(new Response().VideoFlv(), configurationDelegate)
		{
		}

		public FlvResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().VideoFlv().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
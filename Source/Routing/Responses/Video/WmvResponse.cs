using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Video
{
	public class WmvResponse : ImmutableResponse
	{
		public WmvResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().VideoWmv().Content(content), configurationDelegate)
		{
		}

		public WmvResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().VideoWmv().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public WmvResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().VideoWmv().Content(content), configurationDelegate)
		{
		}

		public WmvResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().VideoWmv().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
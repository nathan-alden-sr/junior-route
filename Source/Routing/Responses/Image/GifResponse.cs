using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Image
{
	public class GifResponse : ImmutableResponse
	{
		public GifResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageGif().Content(content), configurationDelegate)
		{
		}

		public GifResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageGif().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public GifResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageGif().Content(content), configurationDelegate)
		{
		}

		public GifResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageGif().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
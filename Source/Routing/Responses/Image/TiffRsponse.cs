using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Image
{
	public class TiffResponse : ImmutableResponse
	{
		public TiffResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ImageTiff().Content(content), configurationDelegate)
		{
		}

		public TiffResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ImageTiff().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public TiffResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ImageTiff().Content(content), configurationDelegate)
		{
		}

		public TiffResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ImageTiff().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public TiffResponse(Action<Response> configurationDelegate = null)
			: base(new Response().ImageTiff(), configurationDelegate)
		{
		}

		public TiffResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ImageTiff().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
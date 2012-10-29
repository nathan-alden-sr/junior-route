using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Image
{
	public class TiffResponse : ImmutableResponse
	{
		public TiffResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageTiff().Content(content), configurationDelegate)
		{
		}

		public TiffResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageTiff().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public TiffResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageTiff().Content(content), configurationDelegate)
		{
		}

		public TiffResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageTiff().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
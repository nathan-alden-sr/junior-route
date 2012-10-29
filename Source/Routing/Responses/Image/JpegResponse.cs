using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Image
{
	public class JpegResponse : ImmutableResponse
	{
		public JpegResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageJpeg().Content(content), configurationDelegate)
		{
		}

		public JpegResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageJpeg().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public JpegResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageJpeg().Content(content), configurationDelegate)
		{
		}

		public JpegResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImageJpeg().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
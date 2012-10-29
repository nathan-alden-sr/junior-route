using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Image
{
	public class PJpegResponse : ImmutableResponse
	{
		public PJpegResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImagePJpeg().Content(content), configurationDelegate)
		{
		}

		public PJpegResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImagePJpeg().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public PJpegResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImagePJpeg().Content(content), configurationDelegate)
		{
		}

		public PJpegResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ImagePJpeg().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
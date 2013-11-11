using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Image
{
	public class JpegResponse : ImmutableResponse
	{
		public JpegResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ImageJpeg().Content(content), configurationDelegate)
		{
		}

		public JpegResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ImageJpeg().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public JpegResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ImageJpeg().Content(content), configurationDelegate)
		{
		}

		public JpegResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ImageJpeg().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public JpegResponse(Action<Response> configurationDelegate = null)
			: base(new Response().ImageJpeg(), configurationDelegate)
		{
		}

		public JpegResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ImageJpeg().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
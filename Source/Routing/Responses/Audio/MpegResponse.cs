using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Audio
{
	public class MpegResponse : ImmutableResponse
	{
		public MpegResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().AudioMpeg().Content(content), configurationDelegate)
		{
		}

		public MpegResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioMpeg().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public MpegResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().AudioMpeg().Content(content), configurationDelegate)
		{
		}

		public MpegResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioMpeg().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public MpegResponse(Action<Response> configurationDelegate = null)
			: base(new Response().AudioMpeg(), configurationDelegate)
		{
		}

		public MpegResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioMpeg().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
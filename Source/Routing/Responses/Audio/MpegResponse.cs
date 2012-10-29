using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Audio
{
	public class MpegResponse : ImmutableResponse
	{
		public MpegResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().AudioMpeg().Content(content), configurationDelegate)
		{
		}

		public MpegResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().AudioMpeg().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public MpegResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().AudioMpeg().Content(content), configurationDelegate)
		{
		}

		public MpegResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().AudioMpeg().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
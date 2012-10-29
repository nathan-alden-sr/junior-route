using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Audio
{
	public class VorbisResponse : ImmutableResponse
	{
		public VorbisResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().AudioVorbis().Content(content), configurationDelegate)
		{
		}

		public VorbisResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().AudioVorbis().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public VorbisResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().AudioVorbis().Content(content), configurationDelegate)
		{
		}

		public VorbisResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().AudioVorbis().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
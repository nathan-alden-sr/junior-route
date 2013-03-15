using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Video
{
	public class OggResponse : ImmutableResponse
	{
		public OggResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().VideoOgg().Content(content), configurationDelegate)
		{
		}

		public OggResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().VideoOgg().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public OggResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().VideoOgg().Content(content), configurationDelegate)
		{
		}

		public OggResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().VideoOgg().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
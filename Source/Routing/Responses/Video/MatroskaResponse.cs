using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Video
{
	public class MatroskaResponse : ImmutableResponse
	{
		public MatroskaResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().VideoMatroska().Content(content), configurationDelegate)
		{
		}

		public MatroskaResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().VideoMatroska().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public MatroskaResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().VideoMatroska().Content(content), configurationDelegate)
		{
		}

		public MatroskaResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().VideoMatroska().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Video
{
	public class MatroskaResponse : ImmutableResponse
	{
		public MatroskaResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().VideoMatroska().Content(content), configurationDelegate)
		{
		}

		public MatroskaResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().VideoMatroska().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public MatroskaResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().VideoMatroska().Content(content), configurationDelegate)
		{
		}

		public MatroskaResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().VideoMatroska().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
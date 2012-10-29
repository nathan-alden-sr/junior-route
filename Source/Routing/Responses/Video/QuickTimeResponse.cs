using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Video
{
	public class QuickTimeResponse : ImmutableResponse
	{
		public QuickTimeResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().VideoQuickTime().Content(content), configurationDelegate)
		{
		}

		public QuickTimeResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().VideoQuickTime().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public QuickTimeResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().VideoQuickTime().Content(content), configurationDelegate)
		{
		}

		public QuickTimeResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().VideoQuickTime().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Video
{
	public class QuickTimeResponse : ImmutableResponse
	{
		public QuickTimeResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().VideoQuickTime().Content(content), configurationDelegate)
		{
		}

		public QuickTimeResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().VideoQuickTime().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public QuickTimeResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().VideoQuickTime().Content(content), configurationDelegate)
		{
		}

		public QuickTimeResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().VideoQuickTime().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public QuickTimeResponse(Action<Response> configurationDelegate = null)
			: base(new Response().VideoQuickTime(), configurationDelegate)
		{
		}

		public QuickTimeResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().VideoQuickTime().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
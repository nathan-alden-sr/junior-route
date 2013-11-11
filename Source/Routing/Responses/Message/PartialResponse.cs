using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Message
{
	public class PartialResponse : ImmutableResponse
	{
		public PartialResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().MessagePartial().Content(content), configurationDelegate)
		{
		}

		public PartialResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MessagePartial().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public PartialResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().MessagePartial().Content(content), configurationDelegate)
		{
		}

		public PartialResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MessagePartial().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public PartialResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().MessagePartial().Content(content), configurationDelegate)
		{
		}

		public PartialResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MessagePartial().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public PartialResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().MessagePartial().Content(content), configurationDelegate)
		{
		}

		public PartialResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MessagePartial().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public PartialResponse(Action<Response> configurationDelegate = null)
			: base(new Response().MessagePartial(), configurationDelegate)
		{
		}

		public PartialResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MessagePartial().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Application
{
	public class PostscriptResponse : ImmutableResponse
	{
		public PostscriptResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationPostscript().Content(content), configurationDelegate)
		{
		}

		public PostscriptResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationPostscript().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public PostscriptResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationPostscript().Content(content), configurationDelegate)
		{
		}

		public PostscriptResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationPostscript().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public PostscriptResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationPostscript().Content(content), configurationDelegate)
		{
		}

		public PostscriptResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationPostscript().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public PostscriptResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationPostscript().Content(content), configurationDelegate)
		{
		}

		public PostscriptResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationPostscript().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
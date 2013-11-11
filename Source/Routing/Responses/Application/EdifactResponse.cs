using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Application
{
	public class EdifactResponse : ImmutableResponse
	{
		public EdifactResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdifact().Content(content), configurationDelegate)
		{
		}

		public EdifactResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdifact().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EdifactResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdifact().Content(content), configurationDelegate)
		{
		}

		public EdifactResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdifact().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EdifactResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdifact().Content(content), configurationDelegate)
		{
		}

		public EdifactResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdifact().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EdifactResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdifact().Content(content), configurationDelegate)
		{
		}

		public EdifactResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdifact().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EdifactResponse(Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdifact(), configurationDelegate)
		{
		}

		public EdifactResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdifact().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
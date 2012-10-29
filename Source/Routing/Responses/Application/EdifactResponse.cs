using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Application
{
	public class EdifactResponse : ImmutableResponse
	{
		public EdifactResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationEdifact().Content(content), configurationDelegate)
		{
		}

		public EdifactResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationEdifact().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EdifactResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationEdifact().Content(content), configurationDelegate)
		{
		}

		public EdifactResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationEdifact().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EdifactResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationEdifact().Content(content), configurationDelegate)
		{
		}

		public EdifactResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationEdifact().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EdifactResponse(string content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationEdifact().Content(content), configurationDelegate)
		{
		}

		public EdifactResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationEdifact().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
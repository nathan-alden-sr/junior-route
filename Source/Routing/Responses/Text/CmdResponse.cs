using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Text
{
	public class CmdResponse : ImmutableResponse
	{
		public CmdResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextCmd().Content(content), configurationDelegate)
		{
		}

		public CmdResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextCmd().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public CmdResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextCmd().Content(content), configurationDelegate)
		{
		}

		public CmdResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextCmd().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public CmdResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextCmd().Content(content), configurationDelegate)
		{
		}

		public CmdResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextCmd().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public CmdResponse(string content, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextCmd().Content(content), configurationDelegate)
		{
		}

		public CmdResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextCmd().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
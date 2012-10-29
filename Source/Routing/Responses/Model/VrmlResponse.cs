using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Model
{
	public class VrmlResponse : ImmutableResponse
	{
		public VrmlResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ModelVrml().Content(content), configurationDelegate)
		{
		}

		public VrmlResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ModelVrml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public VrmlResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ModelVrml().Content(content), configurationDelegate)
		{
		}

		public VrmlResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ModelVrml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public VrmlResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ModelVrml().Content(content), configurationDelegate)
		{
		}

		public VrmlResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ModelVrml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public VrmlResponse(string content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ModelVrml().Content(content), configurationDelegate)
		{
		}

		public VrmlResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ModelVrml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
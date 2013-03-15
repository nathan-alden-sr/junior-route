using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Model
{
	public class X3DVrmlResponse : ImmutableResponse
	{
		public X3DVrmlResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DVrml().Content(content), configurationDelegate)
		{
		}

		public X3DVrmlResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DVrml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public X3DVrmlResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DVrml().Content(content), configurationDelegate)
		{
		}

		public X3DVrmlResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DVrml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public X3DVrmlResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DVrml().Content(content), configurationDelegate)
		{
		}

		public X3DVrmlResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DVrml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public X3DVrmlResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DVrml().Content(content), configurationDelegate)
		{
		}

		public X3DVrmlResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelX3DVrml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
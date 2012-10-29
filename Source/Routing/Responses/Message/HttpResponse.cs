using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Message
{
	public class HttpResponse : ImmutableResponse
	{
		public HttpResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().MessageHttp().Content(content), configurationDelegate)
		{
		}

		public HttpResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().MessageHttp().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public HttpResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().MessageHttp().Content(content), configurationDelegate)
		{
		}

		public HttpResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().MessageHttp().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Application
{
	public class ZipResponse : ImmutableResponse
	{
		public ZipResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationZip().Content(content), configurationDelegate)
		{
		}

		public ZipResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationZip().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public ZipResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationZip().Content(content), configurationDelegate)
		{
		}

		public ZipResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationZip().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public ZipResponse(Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationZip(), configurationDelegate)
		{
		}

		public ZipResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationZip().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
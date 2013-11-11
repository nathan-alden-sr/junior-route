using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Model
{
	public class IgesResponse : ImmutableResponse
	{
		public IgesResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelIges().Content(content), configurationDelegate)
		{
		}

		public IgesResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelIges().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public IgesResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelIges().Content(content), configurationDelegate)
		{
		}

		public IgesResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelIges().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public IgesResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelIges().Content(content), configurationDelegate)
		{
		}

		public IgesResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelIges().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public IgesResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelIges().Content(content), configurationDelegate)
		{
		}

		public IgesResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelIges().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public IgesResponse(Action<Response> configurationDelegate = null)
			: base(new Response().ModelIges(), configurationDelegate)
		{
		}

		public IgesResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelIges().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
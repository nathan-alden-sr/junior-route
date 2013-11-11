using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Application
{
	public class EdiX12Response : ImmutableResponse
	{
		public EdiX12Response(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdiX12().Content(content), configurationDelegate)
		{
		}

		public EdiX12Response(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdiX12().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EdiX12Response(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdiX12().Content(content), configurationDelegate)
		{
		}

		public EdiX12Response(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdiX12().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EdiX12Response(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdiX12().Content(content), configurationDelegate)
		{
		}

		public EdiX12Response(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdiX12().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EdiX12Response(string content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdiX12().Content(content), configurationDelegate)
		{
		}

		public EdiX12Response(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdiX12().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EdiX12Response(Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdiX12(), configurationDelegate)
		{
		}

		public EdiX12Response(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEdiX12().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
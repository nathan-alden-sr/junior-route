using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Application
{
	public class DtdResponse : ImmutableResponse
	{
		public DtdResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationDtd().Content(content), configurationDelegate)
		{
		}

		public DtdResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationDtd().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public DtdResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationDtd().Content(content), configurationDelegate)
		{
		}

		public DtdResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationDtd().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public DtdResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationDtd().Content(content), configurationDelegate)
		{
		}

		public DtdResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationDtd().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public DtdResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationDtd().Content(content), configurationDelegate)
		{
		}

		public DtdResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationDtd().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public DtdResponse(Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationDtd(), configurationDelegate)
		{
		}

		public DtdResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationDtd().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
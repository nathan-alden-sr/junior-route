using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Application
{
	public class PdfResponse : ImmutableResponse
	{
		public PdfResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationPdf().Content(content), configurationDelegate)
		{
		}

		public PdfResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationPdf().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public PdfResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationPdf().Content(content), configurationDelegate)
		{
		}

		public PdfResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ApplicationPdf().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
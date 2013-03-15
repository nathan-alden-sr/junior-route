using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Multipart
{
	public class AlternativeResponse : ImmutableResponse
	{
		public AlternativeResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartAlternative().Content(content), configurationDelegate)
		{
		}

		public AlternativeResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartAlternative().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public AlternativeResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartAlternative().Content(content), configurationDelegate)
		{
		}

		public AlternativeResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartAlternative().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public AlternativeResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartAlternative().Content(content), configurationDelegate)
		{
		}

		public AlternativeResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartAlternative().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public AlternativeResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartAlternative().Content(content), configurationDelegate)
		{
		}

		public AlternativeResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartAlternative().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
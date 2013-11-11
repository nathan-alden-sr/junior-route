using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Multipart
{
	public class SignedResponse : ImmutableResponse
	{
		public SignedResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartSigned().Content(content), configurationDelegate)
		{
		}

		public SignedResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartSigned().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public SignedResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartSigned().Content(content), configurationDelegate)
		{
		}

		public SignedResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartSigned().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public SignedResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartSigned().Content(content), configurationDelegate)
		{
		}

		public SignedResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartSigned().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public SignedResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartSigned().Content(content), configurationDelegate)
		{
		}

		public SignedResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartSigned().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public SignedResponse(Action<Response> configurationDelegate = null)
			: base(new Response().MultipartSigned(), configurationDelegate)
		{
		}

		public SignedResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartSigned().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
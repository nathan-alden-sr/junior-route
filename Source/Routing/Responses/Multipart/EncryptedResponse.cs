using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Multipart
{
	public class EncryptedResponse : ImmutableResponse
	{
		public EncryptedResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartEncrypted().Content(content), configurationDelegate)
		{
		}

		public EncryptedResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartEncrypted().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EncryptedResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartEncrypted().Content(content), configurationDelegate)
		{
		}

		public EncryptedResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartEncrypted().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EncryptedResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartEncrypted().Content(content), configurationDelegate)
		{
		}

		public EncryptedResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartEncrypted().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EncryptedResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartEncrypted().Content(content), configurationDelegate)
		{
		}

		public EncryptedResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartEncrypted().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EncryptedResponse(Action<Response> configurationDelegate = null)
			: base(new Response().MultipartEncrypted(), configurationDelegate)
		{
		}

		public EncryptedResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartEncrypted().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
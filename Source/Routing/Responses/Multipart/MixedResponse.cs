using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Multipart
{
	public class MixedResponse : ImmutableResponse
	{
		public MixedResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartMixed().Content(content), configurationDelegate)
		{
		}

		public MixedResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartMixed().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public MixedResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartMixed().Content(content), configurationDelegate)
		{
		}

		public MixedResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartMixed().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public MixedResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartMixed().Content(content), configurationDelegate)
		{
		}

		public MixedResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartMixed().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public MixedResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartMixed().Content(content), configurationDelegate)
		{
		}

		public MixedResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartMixed().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public MixedResponse(Action<Response> configurationDelegate = null)
			: base(new Response().MultipartMixed(), configurationDelegate)
		{
		}

		public MixedResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartMixed().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
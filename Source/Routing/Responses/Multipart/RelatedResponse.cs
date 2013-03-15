using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Multipart
{
	public class RelatedResponse : ImmutableResponse
	{
		public RelatedResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartRelated().Content(content), configurationDelegate)
		{
		}

		public RelatedResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartRelated().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public RelatedResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartRelated().Content(content), configurationDelegate)
		{
		}

		public RelatedResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartRelated().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public RelatedResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartRelated().Content(content), configurationDelegate)
		{
		}

		public RelatedResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartRelated().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public RelatedResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartRelated().Content(content), configurationDelegate)
		{
		}

		public RelatedResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().MultipartRelated().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
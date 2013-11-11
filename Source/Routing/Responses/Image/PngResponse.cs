using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Image
{
	public class PngResponse : ImmutableResponse
	{
		public PngResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ImagePng().Content(content), configurationDelegate)
		{
		}

		public PngResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ImagePng().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public PngResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ImagePng().Content(content), configurationDelegate)
		{
		}

		public PngResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ImagePng().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public PngResponse(Action<Response> configurationDelegate = null)
			: base(new Response().ImagePng(), configurationDelegate)
		{
		}

		public PngResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ImagePng().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
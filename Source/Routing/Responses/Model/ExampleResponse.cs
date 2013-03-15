using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Model
{
	public class ExampleResponse : ImmutableResponse
	{
		public ExampleResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelExample().Content(content), configurationDelegate)
		{
		}

		public ExampleResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelExample().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public ExampleResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelExample().Content(content), configurationDelegate)
		{
		}

		public ExampleResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelExample().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public ExampleResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelExample().Content(content), configurationDelegate)
		{
		}

		public ExampleResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelExample().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public ExampleResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().ModelExample().Content(content), configurationDelegate)
		{
		}

		public ExampleResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ModelExample().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
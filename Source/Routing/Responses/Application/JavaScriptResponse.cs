using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Application
{
	public class JavaScriptResponse : ImmutableResponse
	{
		public JavaScriptResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationJavaScript().Content(content), configurationDelegate)
		{
		}

		public JavaScriptResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationJavaScript().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public JavaScriptResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationJavaScript().Content(content), configurationDelegate)
		{
		}

		public JavaScriptResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationJavaScript().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public JavaScriptResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationJavaScript().Content(content), configurationDelegate)
		{
		}

		public JavaScriptResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationJavaScript().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public JavaScriptResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationJavaScript().Content(content), configurationDelegate)
		{
		}

		public JavaScriptResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationJavaScript().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
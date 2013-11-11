using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Application
{
	public class EcmaScriptResponse : ImmutableResponse
	{
		public EcmaScriptResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEcmaScript().Content(content), configurationDelegate)
		{
		}

		public EcmaScriptResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEcmaScript().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EcmaScriptResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEcmaScript().Content(content), configurationDelegate)
		{
		}

		public EcmaScriptResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEcmaScript().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EcmaScriptResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEcmaScript().Content(content), configurationDelegate)
		{
		}

		public EcmaScriptResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEcmaScript().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EcmaScriptResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEcmaScript().Content(content), configurationDelegate)
		{
		}

		public EcmaScriptResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEcmaScript().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public EcmaScriptResponse(Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEcmaScript(), configurationDelegate)
		{
		}

		public EcmaScriptResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationEcmaScript().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
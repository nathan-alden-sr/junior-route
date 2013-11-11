using System;
using System.Text;

using Newtonsoft.Json;

namespace Junior.Route.Routing.Responses.Text
{
	public class JsonHtmlResponse : ImmutableResponse
	{
		public JsonHtmlResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().Content(content), configurationDelegate)
		{
		}

		public JsonHtmlResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public JsonHtmlResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().Content(content), configurationDelegate)
		{
		}

		public JsonHtmlResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public JsonHtmlResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().Content(content), configurationDelegate)
		{
		}

		public JsonHtmlResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public JsonHtmlResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().Content(content), configurationDelegate)
		{
		}

		public JsonHtmlResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public JsonHtmlResponse(object content, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().Content(() => JsonConvert.SerializeObject(content)), configurationDelegate)
		{
		}

		public JsonHtmlResponse(object content, JsonSerializerSettings serializerSettings)
			: base(new Response().TextHtml().Content(() => JsonConvert.SerializeObject(content, serializerSettings)))
		{
		}

		public JsonHtmlResponse(object content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().ContentEncoding(encoding).Content(() => JsonConvert.SerializeObject(content)), configurationDelegate)
		{
		}

		public JsonHtmlResponse(object content, Encoding encoding, JsonSerializerSettings serializerSettings)
			: base(new Response().TextHtml().ContentEncoding(encoding).Content(() => JsonConvert.SerializeObject(content, serializerSettings)))
		{
		}

		public JsonHtmlResponse(Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml(), configurationDelegate)
		{
		}

		public JsonHtmlResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextHtml().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
using System;
using System.Text;

using Newtonsoft.Json;

namespace Junior.Route.Routing.Responses.Text
{
	public class JsonPlainResponse : ImmutableResponse
	{
		public JsonPlainResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().Content(content), configurationDelegate)
		{
		}

		public JsonPlainResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public JsonPlainResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().Content(content), configurationDelegate)
		{
		}

		public JsonPlainResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public JsonPlainResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().Content(content), configurationDelegate)
		{
		}

		public JsonPlainResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public JsonPlainResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().Content(content), configurationDelegate)
		{
		}

		public JsonPlainResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public JsonPlainResponse(object content, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().Content(() => JsonConvert.SerializeObject(content)), configurationDelegate)
		{
		}

		public JsonPlainResponse(object content, JsonSerializerSettings serializerSettings)
			: base(new Response().TextPlain().Content(() => JsonConvert.SerializeObject(content, serializerSettings)))
		{
		}

		public JsonPlainResponse(object content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().ContentEncoding(encoding).Content(() => JsonConvert.SerializeObject(content)), configurationDelegate)
		{
		}

		public JsonPlainResponse(object content, Encoding encoding, JsonSerializerSettings serializerSettings)
			: base(new Response().TextPlain().ContentEncoding(encoding).Content(() => JsonConvert.SerializeObject(content, serializerSettings)))
		{
		}

		public JsonPlainResponse(Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain(), configurationDelegate)
		{
		}

		public JsonPlainResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().TextPlain().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}
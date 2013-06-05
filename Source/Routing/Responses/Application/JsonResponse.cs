using System;
using System.Text;

using Newtonsoft.Json;

namespace Junior.Route.Routing.Responses.Application
{
	public class JsonResponse : ImmutableResponse
	{
		public JsonResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationJson().Content(content), configurationDelegate)
		{
		}

		public JsonResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationJson().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public JsonResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationJson().Content(content), configurationDelegate)
		{
		}

		public JsonResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationJson().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public JsonResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationJson().Content(content), configurationDelegate)
		{
		}

		public JsonResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationJson().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public JsonResponse(string content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationJson().Content(content), configurationDelegate)
		{
		}

		public JsonResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationJson().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public JsonResponse(object content, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationJson().Content(() => JsonConvert.SerializeObject(content)), configurationDelegate)
		{
		}

		public JsonResponse(object content, JsonSerializerSettings serializerSettings)
			: base(new Response().ApplicationJson().Content(() => JsonConvert.SerializeObject(content, serializerSettings)))
		{
		}

		public JsonResponse(object content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().ApplicationJson().ContentEncoding(encoding).Content(() => JsonConvert.SerializeObject(content)), configurationDelegate)
		{
		}

		public JsonResponse(object content, Encoding encoding, JsonSerializerSettings serializerSettings)
			: base(new Response().ApplicationJson().ContentEncoding(encoding).Content(() => JsonConvert.SerializeObject(content, serializerSettings)))
		{
		}
	}
}
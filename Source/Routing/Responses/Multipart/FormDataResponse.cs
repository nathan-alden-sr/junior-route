using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Multipart
{
	public class FormDataResponse : ImmutableResponse
	{
		public FormDataResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().MultipartFormData().Content(content), configurationDelegate)
		{
		}

		public FormDataResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().MultipartFormData().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public FormDataResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().MultipartFormData().Content(content), configurationDelegate)
		{
		}

		public FormDataResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().MultipartFormData().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public FormDataResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().MultipartFormData().Content(content), configurationDelegate)
		{
		}

		public FormDataResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().MultipartFormData().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public FormDataResponse(string content, Action<Response> configurationDelegate = null)
			: base(Response.OK().MultipartFormData().Content(content), configurationDelegate)
		{
		}

		public FormDataResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().MultipartFormData().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
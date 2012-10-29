using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Text
{
	public class CsvResponse : ImmutableResponse
	{
		public CsvResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextCsv().Content(content), configurationDelegate)
		{
		}

		public CsvResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextCsv().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public CsvResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextCsv().Content(content), configurationDelegate)
		{
		}

		public CsvResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextCsv().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public CsvResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextCsv().Content(content), configurationDelegate)
		{
		}

		public CsvResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextCsv().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public CsvResponse(string content, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextCsv().Content(content), configurationDelegate)
		{
		}

		public CsvResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().TextCsv().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}
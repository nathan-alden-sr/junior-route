using System.Collections.Generic;
using System.Net;
using System.Text;

using Junior.Route.Routing.Caching;

namespace Junior.Route.Routing.Responses
{
	public interface IResponse
	{
		int StatusCode
		{
			get;
		}
		HttpStatusCode? ParsedStatusCode
		{
			get;
		}
		int SubStatusCode
		{
			get;
		}
		string ContentType
		{
			get;
		}
		string Charset
		{
			get;
		}
		Encoding ContentEncoding
		{
			get;
		}
		IEnumerable<Header> Headers
		{
			get;
		}
		Encoding HeaderEncoding
		{
			get;
		}
		IEnumerable<Cookie> Cookies
		{
			get;
		}
		ICachePolicy CachePolicy
		{
			get;
		}

		byte[] GetContent();
	}
}
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Junior.Route.Routing.Caching;

namespace Junior.Route.Routing.Responses
{
	public interface IResponse
	{
		StatusAndSubStatusCode StatusCode
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

		bool SkipIisCustomErrors
		{
			get;
		}

		Task<byte[]> GetContentAsync();
	}
}
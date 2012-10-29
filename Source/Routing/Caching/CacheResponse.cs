using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.Responses;

using Cookie = Junior.Route.Routing.Responses.Cookie;

namespace Junior.Route.Routing.Caching
{
	public class CacheResponse
	{
		private const string DefaultCharset = "utf-8";
		private static readonly Encoding _defaultContentEncoding = Encoding.UTF8;
		private static readonly Encoding _defaultHeaderEncoding = Encoding.UTF8;
		private readonly ICachePolicy _cachePolicy;
		private readonly string _charset;
		private readonly byte[] _content;
		private readonly Encoding _contentEncoding;
		private readonly string _contentType;
		private readonly IEnumerable<Cookie> _cookies;
		private readonly Encoding _headerEncoding;
		private readonly IEnumerable<Header> _headers;
		private readonly HttpStatusCode? _parsedStatusCode;
		private readonly int _statusCode;
		private readonly int _subStatusCode;

		public CacheResponse(IResponse routeResponse)
		{
			routeResponse.ThrowIfNull("routeResponse");

			_statusCode = routeResponse.StatusCode;
			_parsedStatusCode = routeResponse.ParsedStatusCode;
			_subStatusCode = routeResponse.SubStatusCode;
			_contentType = routeResponse.ContentType;
			_charset = routeResponse.Charset ?? DefaultCharset;
			_contentEncoding = routeResponse.ContentEncoding ?? _defaultContentEncoding;
			_headers = routeResponse.Headers.Select(arg => arg.Clone());
			_headerEncoding = routeResponse.HeaderEncoding ?? _defaultHeaderEncoding;
			_cookies = routeResponse.Cookies.Select(arg => arg.Clone());
			_cachePolicy = routeResponse.CachePolicy.IfNotNull(arg => arg.Clone());
			_content = routeResponse.GetContent();
		}

		public int StatusCode
		{
			get
			{
				return _statusCode;
			}
		}

		public HttpStatusCode? ParsedStatusCode
		{
			get
			{
				return _parsedStatusCode;
			}
		}

		public int SubStatusCode
		{
			get
			{
				return _subStatusCode;
			}
		}

		public string ContentType
		{
			get
			{
				return _contentType;
			}
		}

		public string Charset
		{
			get
			{
				return _charset;
			}
		}

		public Encoding ContentEncoding
		{
			get
			{
				return _contentEncoding;
			}
		}

		public IEnumerable<Header> Headers
		{
			get
			{
				return _headers;
			}
		}

		public Encoding HeaderEncoding
		{
			get
			{
				return _headerEncoding;
			}
		}

		public IEnumerable<Cookie> Cookies
		{
			get
			{
				return _cookies;
			}
		}

		public ICachePolicy CachePolicy
		{
			get
			{
				return _cachePolicy;
			}
		}

		public byte[] Content
		{
			get
			{
				return _content;
			}
		}

		public void WriteResponse(HttpResponseBase response, bool applyCachePolicy = true)
		{
			response.ThrowIfNull("response");

			response.StatusCode = StatusCode;
			response.SubStatusCode = SubStatusCode;
			response.ContentType = ContentType;
			response.Charset = Charset;
			response.ContentEncoding = ContentEncoding;
			foreach (Header header in Headers)
			{
				response.Headers.Add(header.Field, header.Value);
			}
			response.HeaderEncoding = HeaderEncoding;
			foreach (Cookie cookie in Cookies)
			{
				response.Cookies.Add(cookie.GetHttpCookie());
			}
			if (applyCachePolicy && _cachePolicy != null)
			{
				_cachePolicy.Apply(response.Cache);
			}

			response.OutputStream.Write(_content, 0, _content.Length);
		}
	}
}
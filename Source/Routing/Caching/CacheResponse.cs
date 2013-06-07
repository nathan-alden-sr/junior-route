using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.Responses;

namespace Junior.Route.Routing.Caching
{
	public class CacheResponse
	{
		private const string DefaultCharset = "utf-8";
		private static readonly Encoding _defaultContentEncoding = Encoding.UTF8;
		private static readonly Encoding _defaultHeaderEncoding = Encoding.UTF8;
		private readonly ICachePolicy _cachePolicy;
		private readonly string _charset;
		private readonly AsyncLazy<byte[]> _content;
		private readonly Encoding _contentEncoding;
		private readonly string _contentType;
		private readonly Cookie[] _cookies;
		private readonly Encoding _headerEncoding;
		private readonly Header[] _headers;
		private readonly bool _skipIisCustomErrors;
		private readonly StatusAndSubStatusCode _statusCode;

		public CacheResponse(IResponse response)
		{
			response.ThrowIfNull("response");

			_statusCode = response.StatusCode;
			_contentType = response.ContentType;
			_charset = response.Charset ?? DefaultCharset;
			_contentEncoding = response.ContentEncoding ?? _defaultContentEncoding;
			_headers = response.Headers.Select(arg => arg.Clone()).ToArray();
			_headerEncoding = response.HeaderEncoding ?? _defaultHeaderEncoding;
			_cookies = response.Cookies.Select(arg => arg.Clone()).ToArray();
			_cachePolicy = response.CachePolicy.Clone();
			_skipIisCustomErrors = response.SkipIisCustomErrors;
			_content = new AsyncLazy<byte[]>(() => response.GetContentAsync());
		}

		public StatusAndSubStatusCode StatusCode
		{
			get
			{
				return _statusCode;
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

		public bool SkipIisCustomErrors
		{
			get
			{
				return _skipIisCustomErrors;
			}
		}

		public Task<byte[]> Content
		{
			get
			{
				return _content.Value;
			}
		}

		public async Task WriteResponseAsync(HttpResponseBase response)
		{
			response.ThrowIfNull("response");

			response.StatusCode = _statusCode.StatusCode;
			response.SubStatusCode = _statusCode.SubStatusCode;
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
			_cachePolicy.Apply(response.Cache);
			response.TrySkipIisCustomErrors = _skipIisCustomErrors;

			response.BinaryWrite(await _content.Value);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Junior.Common;
using Junior.Route.Routing.Caching;

namespace Junior.Route.Routing.Responses
{
	public abstract class ImmutableResponse : IResponse
	{
		private readonly IResponse _response;

		protected ImmutableResponse(Response response, Action<Response> configurationDelegate = null)
		{
			response.ThrowIfNull("response");

			if (configurationDelegate != null)
			{
				configurationDelegate(response);
			}
			_response = response;
		}

		public StatusAndSubStatusCode StatusCode
		{
			get
			{
				return _response.StatusCode;
			}
		}

		public string ContentType
		{
			get
			{
				return _response.ContentType;
			}
		}

		public string Charset
		{
			get
			{
				return _response.Charset;
			}
		}

		public Encoding ContentEncoding
		{
			get
			{
				return _response.ContentEncoding;
			}
		}

		public Encoding HeaderEncoding
		{
			get
			{
				return _response.HeaderEncoding;
			}
		}

		public IEnumerable<Header> Headers
		{
			get
			{
				return _response.Headers;
			}
		}

		public IEnumerable<Cookie> Cookies
		{
			get
			{
				return _response.Cookies;
			}
		}

		public ICachePolicy CachePolicy
		{
			get
			{
				return _response.CachePolicy;
			}
		}

		public bool SkipIisCustomErrors
		{
			get
			{
				return _response.SkipIisCustomErrors;
			}
		}

		public Task<byte[]> GetContentAsync()
		{
			return _response.GetContentAsync();
		}
	}
}
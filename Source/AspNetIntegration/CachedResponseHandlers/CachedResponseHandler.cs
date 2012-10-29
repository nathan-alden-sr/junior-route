using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

using Junior.Common;
using Junior.Route.Http;
using Junior.Route.Http.RequestHeaders;
using Junior.Route.Routing.Caching;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.CachedResponseHandlers
{
	public class CachedResponseHandler : ICachedResponseHandler
	{
		private static readonly IEnumerable<int> _cacheableStatusCodes = new[]
			{
				(int)HttpStatusCode.OK,
				(int)HttpStatusCode.NonAuthoritativeInformation,
				(int)HttpStatusCode.MultipleChoices,
				(int)HttpStatusCode.MovedPermanently,
				(int)HttpStatusCode.Gone
			};
		private readonly ISystemClock _systemClock;

		public CachedResponseHandler(ISystemClock systemClock)
		{
			systemClock.ThrowIfNull("systemClock");

			_systemClock = systemClock;
		}

		public CachedResponseHandlerResult HandleResponse(HttpRequestBase httpRequest, HttpResponseBase httpResponse, IResponse response, ICache cache, string cacheKey)
		{
			httpRequest.ThrowIfNull("request");
			httpResponse.ThrowIfNull("httpResponse");
			response.ThrowIfNull("response");
			cache.ThrowIfNull("cache");
			cacheKey.ThrowIfNull("cacheKey");

			CacheItem cacheItem = cache.Get(cacheKey);
			string responseETag = response.CachePolicy.IfNotNull(arg => arg.ETag);

			#region If-Match precondition header

			IfMatchHeader[] ifMatchHeaders = IfMatchHeader.ParseMany(httpRequest.Headers["If-Match"]).ToArray();

			// Only consider If-Match headers if response status code is 2xx or 412
			if (ifMatchHeaders.Any() && ((response.StatusCode >= 200 && response.StatusCode <= 299) || response.StatusCode == 412))
			{
				// Return 412 if no If-Match header matches the response ETag
				// Return 412 if an "If-Match: *" header is present and the response has no ETag
				if (ifMatchHeaders.All(arg => arg.EntityTag.Value != responseETag) ||
				    (responseETag == null && ifMatchHeaders.Any(arg => arg.EntityTag.Value == "*")))
				{
					return WriteResponse(httpResponse, Response.PreconditionFailed());
				}
			}

			#endregion

			#region If-None-Match precondition header

			IfNoneMatchHeader[] ifNoneMatchHeaders = IfNoneMatchHeader.ParseMany(httpRequest.Headers["If-None-Match"]).ToArray();

			if (ifNoneMatchHeaders.Any())
			{
				// Return 304 if an If-None-Match header matches the response ETag and the request method was GET or HEAD
				// Return 304 if an "If-None-Match: *" header is present, the response has an ETag and the request method was GET or HEAD
				// Return 412 if an "If-None-Match: *" header is present, the response has an ETag and the request method was not GET or HEAD
				if (ifNoneMatchHeaders.Any(arg => arg.EntityTag.Value == responseETag) ||
				    (ifNoneMatchHeaders.Any(arg => arg.EntityTag.Value == "*") && responseETag != null))
				{
					if (String.Equals(httpRequest.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase) || String.Equals(httpRequest.HttpMethod, "HEAD", StringComparison.OrdinalIgnoreCase))
					{
						if (cacheItem != null)
						{
							cacheItem.Response.CachePolicy.Apply(httpResponse.Cache);
						}
						else
						{
							response.CachePolicy.Apply(httpResponse.Cache);
						}

						return WriteResponse(httpResponse, Response.NotModified(), false);
					}

					return WriteResponse(httpResponse, Response.PreconditionFailed());
				}
			}

			#endregion

			#region If-Modified-Since precondition header

			IfModifiedSinceHeader ifModifiedSinceHeader = IfModifiedSinceHeader.Parse(httpRequest.Headers["If-Modified-Since"]);
			bool validIfModifiedSinceHttpDate = ifModifiedSinceHeader != null && ifModifiedSinceHeader.HttpDate <= _systemClock.UtcDateTime;

			// Only consider an If-Modified-Since header if response status code is 200 and the HTTP-date is valid
			if (response.ParsedStatusCode == HttpStatusCode.OK && validIfModifiedSinceHttpDate)
			{
				// Return 304 if the response was cached before the HTTP-date
				if (cacheItem != null && cacheItem.CachedUtcTimestamp < ifModifiedSinceHeader.HttpDate)
				{
					return WriteResponse(httpResponse, Response.NotModified());
				}
			}

			#endregion

			#region If-Unmodified-Since precondition header

			IfUnmodifiedSinceHeader ifUnmodifiedSinceHeader = IfUnmodifiedSinceHeader.Parse(httpRequest.Headers["If-Unmodified-Since"]);
			bool validIfUnmodifiedSinceHttpDate = ifUnmodifiedSinceHeader != null && ifUnmodifiedSinceHeader.HttpDate <= _systemClock.UtcDateTime;

			// Only consider an If-Unmodified-Since header if response status code is 2xx or 412 and the HTTP-date is valid
			if (((response.StatusCode >= 200 && response.StatusCode <= 299) || response.StatusCode == 412) && validIfUnmodifiedSinceHttpDate)
			{
				// Return 412 if the previous response was removed from the cache or was cached again at a later time
				if (cacheItem == null || cacheItem.CachedUtcTimestamp >= ifUnmodifiedSinceHeader.HttpDate)
				{
					return WriteResponse(httpResponse, Response.PreconditionFailed());
				}
			}

			#endregion

			#region No server caching

			// Do not cache the response when the response sends a non-cacheable status code, or when an Authorization header is present
			if (!_cacheableStatusCodes.Contains(response.StatusCode) || httpRequest.Headers["Authorization"] != null)
			{
				return WriteResponse(httpResponse, response);
			}

			CacheControlHeader cacheControlHeader = CacheControlHeader.Parse(httpRequest.Headers["Cache-Control"]);

			// Do not cache the response if a "Cache-Control: no-cache" or "Cache-Control: no-store" header is present
			if (cacheControlHeader != null && (cacheControlHeader.NoCache || cacheControlHeader.NoStore))
			{
				return WriteResponse(httpResponse, response);
			}

			IEnumerable<PragmaHeader> pragmaHeader = PragmaHeader.ParseMany(httpRequest.Headers["Pragma"]);

			// Do not cache the response if a "Pragma: no-cache" header is present
			if (pragmaHeader.Any(arg => String.Equals(arg.Name, "no-cache", StringComparison.OrdinalIgnoreCase)))
			{
				return WriteResponse(httpResponse, response);
			}

			#endregion

			// Return 504 if the response has not been cached but the client is requesting to receive only a cached response
			if (cacheItem == null && cacheControlHeader != null && cacheControlHeader.OnlyIfCached)
			{
				return WriteResponse(httpResponse, Response.GatewayTimeout());
			}

			if (cacheItem != null)
			{
				// Write the cached response if no Cache-Control header is present
				// Write the cached response if a "Cache-Control: max-age" header is validated
				// Write the cached response if a "Cache-Control: max-stale" header is validated
				// Write the cached response if a "Cache-Control: min-fresh" header is validated
				if (cacheControlHeader == null ||
				    _systemClock.UtcDateTime - cacheItem.CachedUtcTimestamp <= cacheControlHeader.MaxAge ||
					cacheControlHeader.OnlyIfCached ||
				    cacheItem.ExpiresUtcTimestamp == null ||
				    _systemClock.UtcDateTime - cacheItem.ExpiresUtcTimestamp.Value <= cacheControlHeader.MaxStale ||
				    cacheItem.ExpiresUtcTimestamp.Value - _systemClock.UtcDateTime < cacheControlHeader.MinFresh)
				{
					return WriteResponseInCache(httpResponse, cacheItem);
				}
			}

			ICachePolicy cachePolicy = response.CachePolicy;
			bool cacheOnServer = cachePolicy != null && cachePolicy.AllowsServerCaching;
			var cacheResponse = new CacheResponse(response);

			if (cacheOnServer)
			{
				DateTime expirationUtcTimestamp = response.CachePolicy.Expires != null ? response.CachePolicy.Expires.Value : _systemClock.UtcDateTime + response.CachePolicy.MaxAge.Value;

				cache.Add(cacheKey, cacheResponse, expirationUtcTimestamp);
			}

			return WriteResponse(httpResponse, cacheResponse);
		}

		private static CachedResponseHandlerResult WriteResponse(HttpResponseBase httpResponse, IResponse response, bool applyCachePolicy = true)
		{
			var cacheResponse = new CacheResponse(response);

			return WriteResponse(httpResponse, cacheResponse, applyCachePolicy);
		}

		private static CachedResponseHandlerResult WriteResponse(HttpResponseBase httpResponse, CacheResponse cacheResponse, bool applyCachePolicy = true)
		{
			cacheResponse.WriteResponse(httpResponse);

			return CachedResponseHandlerResult.ResponseHandledWithResponse(cacheResponse);
		}

		private static CachedResponseHandlerResult WriteResponseInCache(HttpResponseBase httpResponse, CacheItem cacheItem)
		{
			cacheItem.Response.WriteResponse(httpResponse);

			httpResponse.Headers.Set("Last-Modified", cacheItem.CachedUtcTimestamp.ToHttpDate());

			return CachedResponseHandlerResult.ResponseHandledWithResponseInCache();
		}
	}
}
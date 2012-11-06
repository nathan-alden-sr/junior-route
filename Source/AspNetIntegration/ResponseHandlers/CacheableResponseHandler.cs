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

namespace Junior.Route.AspNetIntegration.ResponseHandlers
{
	public class CacheableResponseHandler : IResponseHandler
	{
		private static readonly StatusAndSubStatusCode[] _cacheableStatusCodes = new[]
			{
				new StatusAndSubStatusCode(HttpStatusCode.OK),
				new StatusAndSubStatusCode(HttpStatusCode.NonAuthoritativeInformation),
				new StatusAndSubStatusCode(HttpStatusCode.MultipleChoices),
				new StatusAndSubStatusCode(HttpStatusCode.MovedPermanently),
				new StatusAndSubStatusCode(HttpStatusCode.Gone)
			};
		private readonly ISystemClock _systemClock;

		public CacheableResponseHandler(ISystemClock systemClock)
		{
			systemClock.ThrowIfNull("systemClock");

			_systemClock = systemClock;
		}

		public ResponseHandlerResult HandleResponse(HttpRequestBase httpRequest, HttpResponseBase httpResponse, IResponse suggestedResponse, ICache cache, string cacheKey)
		{
			httpRequest.ThrowIfNull("request");
			httpResponse.ThrowIfNull("httpResponse");
			suggestedResponse.ThrowIfNull("suggestedResponse");

			if (!suggestedResponse.CachePolicy.HasPolicy || cache == null || cacheKey == null)
			{
				return ResponseHandlerResult.ResponseNotHandled();
			}

			CacheItem cacheItem = cache.Get(cacheKey);
			string responseETag = suggestedResponse.CachePolicy.ETag;

			#region If-Match precondition header

			IfMatchHeader[] ifMatchHeaders = IfMatchHeader.ParseMany(httpRequest.Headers["If-Match"]).ToArray();

			// Only consider If-Match headers if response status code is 2xx or 412
			if (ifMatchHeaders.Any() && ((suggestedResponse.StatusCode.StatusCode >= 200 && suggestedResponse.StatusCode.StatusCode <= 299) || suggestedResponse.StatusCode.StatusCode == 412))
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
							suggestedResponse.CachePolicy.Apply(httpResponse.Cache);
						}

						return WriteResponse(httpResponse, Response.NotModified());
					}

					return WriteResponse(httpResponse, Response.PreconditionFailed());
				}
			}

			#endregion

			#region If-Modified-Since precondition header

			IfModifiedSinceHeader ifModifiedSinceHeader = IfModifiedSinceHeader.Parse(httpRequest.Headers["If-Modified-Since"]);
			bool validIfModifiedSinceHttpDate = ifModifiedSinceHeader != null && ifModifiedSinceHeader.HttpDate <= _systemClock.UtcDateTime;

			// Only consider an If-Modified-Since header if response status code is 200 and the HTTP-date is valid
			if (suggestedResponse.StatusCode.ParsedStatusCode == HttpStatusCode.OK && validIfModifiedSinceHttpDate)
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
			if (((suggestedResponse.StatusCode.StatusCode >= 200 && suggestedResponse.StatusCode.StatusCode <= 299) || suggestedResponse.StatusCode.StatusCode == 412) && validIfUnmodifiedSinceHttpDate)
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
			if (!_cacheableStatusCodes.Contains(suggestedResponse.StatusCode) || httpRequest.Headers["Authorization"] != null)
			{
				return WriteResponse(httpResponse, suggestedResponse);
			}

			CacheControlHeader cacheControlHeader = CacheControlHeader.Parse(httpRequest.Headers["Cache-Control"]);

			// Do not cache the response if a "Cache-Control: no-cache" or "Cache-Control: no-store" header is present
			if (cacheControlHeader != null && (cacheControlHeader.NoCache || cacheControlHeader.NoStore))
			{
				return WriteResponse(httpResponse, suggestedResponse);
			}

			IEnumerable<PragmaHeader> pragmaHeader = PragmaHeader.ParseMany(httpRequest.Headers["Pragma"]);

			// Do not cache the response if a "Pragma: no-cache" header is present
			if (pragmaHeader.Any(arg => String.Equals(arg.Name, "no-cache", StringComparison.OrdinalIgnoreCase)))
			{
				return WriteResponse(httpResponse, suggestedResponse);
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

			bool cacheOnServer = suggestedResponse.CachePolicy.AllowsServerCaching;
			var cacheResponse = new CacheResponse(suggestedResponse);

			if (cacheOnServer)
			{
				DateTime expirationUtcTimestamp = suggestedResponse.CachePolicy.ServerCacheExpirationUtcTimestamp != null
					                                  ? suggestedResponse.CachePolicy.ServerCacheExpirationUtcTimestamp.Value
					                                  : _systemClock.UtcDateTime + suggestedResponse.CachePolicy.ServerCacheMaxAge.Value;

				cache.Add(cacheKey, cacheResponse, expirationUtcTimestamp);
			}

			return WriteResponse(httpResponse, cacheResponse);
		}

		private static ResponseHandlerResult WriteResponse(HttpResponseBase httpResponse, IResponse response)
		{
			return WriteResponse(httpResponse, new CacheResponse(response));
		}

		private static ResponseHandlerResult WriteResponse(HttpResponseBase httpResponse, CacheResponse cacheResponse)
		{
			cacheResponse.WriteResponse(httpResponse);

			return ResponseHandlerResult.ResponseWritten();
		}

		private static ResponseHandlerResult WriteResponseInCache(HttpResponseBase httpResponse, CacheItem cacheItem)
		{
			cacheItem.Response.WriteResponse(httpResponse);

			httpResponse.Headers.Set("Last-Modified", cacheItem.CachedUtcTimestamp.ToHttpDate());

			return ResponseHandlerResult.ResponseWritten();
		}
	}
}
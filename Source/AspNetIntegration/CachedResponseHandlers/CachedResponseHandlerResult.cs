using Junior.Common;
using Junior.Route.Routing.Caching;

namespace Junior.Route.AspNetIntegration.CachedResponseHandlers
{
	public class CachedResponseHandlerResult
	{
		private readonly CacheResponse _cacheResponse;
		private readonly CachedResponseHandlerResultType _resultType;

		private CachedResponseHandlerResult(CachedResponseHandlerResultType resultType, CacheResponse cacheResponse)
		{
			_resultType = resultType;
			_cacheResponse = cacheResponse;
		}

		public CachedResponseHandlerResultType ResultType
		{
			get
			{
				return _resultType;
			}
		}

		public CacheResponse CacheResponse
		{
			get
			{
				return _cacheResponse;
			}
		}

		public static CachedResponseHandlerResult ResponseHandledWithResponseInCache()
		{
			return new CachedResponseHandlerResult(CachedResponseHandlerResultType.ResponseHandledWithResponseInCache, null);
		}

		public static CachedResponseHandlerResult ResponseHandledWithResponse(CacheResponse cacheResponse)
		{
			cacheResponse.ThrowIfNull("cacheResponse");

			return new CachedResponseHandlerResult(CachedResponseHandlerResultType.ResponseHandledWithResponse, cacheResponse);
		}

		public static CachedResponseHandlerResult ResponseNotHandled()
		{
			return new CachedResponseHandlerResult(CachedResponseHandlerResultType.ResponseNotHandled, null);
		}
	}
}
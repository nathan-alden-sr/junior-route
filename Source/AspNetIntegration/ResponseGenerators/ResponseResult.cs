using Junior.Common;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.ResponseGenerators
{
	public class ResponseResult
	{
		private readonly string _cacheKey;
		private readonly IResponse _response;
		private readonly ResponseResultType _resultType;

		private ResponseResult(ResponseResultType resultType, IResponse response, string cacheKey)
		{
			_resultType = resultType;
			_response = response;
			_cacheKey = cacheKey;
		}

		public ResponseResultType ResultType
		{
			get
			{
				return _resultType;
			}
		}

		public IResponse Response
		{
			get
			{
				return _response;
			}
		}

		public string CacheKey
		{
			get
			{
				return _cacheKey;
			}
		}

		public static ResponseResult CachedResponse(IResponse response, string cacheKey)
		{
			cacheKey.ThrowIfNull("cacheKey");

			return new ResponseResult(ResponseResultType.CachedResponse, response, cacheKey);
		}

		public static ResponseResult NonCachedResponse(IResponse response)
		{
			return new ResponseResult(ResponseResultType.NonCachedResponse, response, null);
		}

		public static ResponseResult NoResponse()
		{
			return new ResponseResult(ResponseResultType.NoResponse, null, null);
		}
	}
}
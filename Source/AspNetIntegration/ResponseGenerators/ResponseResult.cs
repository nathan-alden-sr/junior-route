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

		public static ResponseResult ResponseGenerated(IResponse response, string cacheKey = null)
		{
			return new ResponseResult(ResponseResultType.ResponseGenerated, response, cacheKey);
		}

		public static ResponseResult ResponseNotGenerated()
		{
			return new ResponseResult(ResponseResultType.ResponseNotGenerated, null, null);
		}
	}
}
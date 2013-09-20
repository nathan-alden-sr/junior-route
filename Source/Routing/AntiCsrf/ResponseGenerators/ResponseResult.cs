using Junior.Route.Routing.Responses;

namespace Junior.Route.Routing.AntiCsrf.ResponseGenerators
{
	public class ResponseResult
	{
		private readonly IResponse _response;
		private readonly ResponseResultType _resultType;

		private ResponseResult(ResponseResultType resultType, IResponse response = null)
		{
			_resultType = resultType;
			_response = response;
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

		public static ResponseResult ResponseGenerated(IResponse response)
		{
			return new ResponseResult(ResponseResultType.ResponseGenerated, response);
		}

		public static ResponseResult ResponseNotGenerated()
		{
			return new ResponseResult(ResponseResultType.ResponseNotGenerated);
		}
	}
}
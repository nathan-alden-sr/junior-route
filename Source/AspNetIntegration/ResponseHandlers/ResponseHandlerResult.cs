using Junior.Common;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.ResponseHandlers
{
	public class ResponseHandlerResult
	{
		private readonly ResponseHandlerResultType _resultType;
		private readonly IResponse _suggestedResponse;

		private ResponseHandlerResult(ResponseHandlerResultType resultType, IResponse suggestedResponse)
		{
			_resultType = resultType;
			_suggestedResponse = suggestedResponse;
		}

		public ResponseHandlerResultType ResultType
		{
			get
			{
				return _resultType;
			}
		}

		public IResponse SuggestedResponse
		{
			get
			{
				return _suggestedResponse;
			}
		}

		public static ResponseHandlerResult ResponseWritten()
		{
			return new ResponseHandlerResult(ResponseHandlerResultType.ResponseWritten, null);
		}

		public static ResponseHandlerResult ResponseSuggested(IResponse suggestedResponse)
		{
			suggestedResponse.ThrowIfNull("suggestedResponse");

			return new ResponseHandlerResult(ResponseHandlerResultType.ResponseSuggested, suggestedResponse);
		}

		public static ResponseHandlerResult ResponseNotHandled()
		{
			return new ResponseHandlerResult(ResponseHandlerResultType.ResponseNotHandled, null);
		}
	}
}
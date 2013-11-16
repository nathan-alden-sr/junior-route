using Junior.Common;
using Junior.Route.Routing.Responses;

namespace Junior.Route.Routing.RequestValidators
{
	public class ValidateResult
	{
		private readonly IResponse _response;
		private readonly ValidateResultType _resultType;

		private ValidateResult(ValidateResultType resultType, IResponse response = null)
		{
			_resultType = resultType;
			_response = response;
		}

		public ValidateResultType ResultType
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

		public static ValidateResult RequestValidated()
		{
			return new ValidateResult(ValidateResultType.RequestValidated);
		}

		public static ValidateResult ResponseGenerated(IResponse response)
		{
			response.ThrowIfNull("response");

			return new ValidateResult(ValidateResultType.ResponseGenerated, response);
		}
	}
}
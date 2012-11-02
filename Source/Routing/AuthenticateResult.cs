using Junior.Common;
using Junior.Route.Routing.Responses;

namespace Junior.Route.Routing
{
	public class AuthenticateResult
	{
		private readonly IResponse _failedResponse;
		private readonly AuthenticateResultType _resultType;

		private AuthenticateResult(AuthenticateResultType resultType, IResponse failedResponse)
		{
			_resultType = resultType;
			_failedResponse = failedResponse;
		}

		public AuthenticateResultType ResultType
		{
			get
			{
				return _resultType;
			}
		}

		public IResponse FailedResponse
		{
			get
			{
				return _failedResponse;
			}
		}

		public static AuthenticateResult NoAuthenticationPerformed()
		{
			return new AuthenticateResult(AuthenticateResultType.NoAuthenticationPerformed, null);
		}

		public static AuthenticateResult AuthenticationSucceeded()
		{
			return new AuthenticateResult(AuthenticateResultType.AuthenticationSucceeded, null);
		}

		public static AuthenticateResult AuthenticationFailed(IResponse failedResponse)
		{
			failedResponse.ThrowIfNull("failedResponse");

			return new AuthenticateResult(AuthenticateResultType.AuthenticationFailed, failedResponse);
		}
	}
}
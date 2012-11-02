using Junior.Route.Routing;
using Junior.Route.Routing.Responses;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Routing
{
	public static class AuthenticateResultTester
	{
		[TestFixture]
		public class When_creating_instance_with_authentication_failed_result_type
		{
			[SetUp]
			public void SetUp()
			{
				_failedResponse = Response.OK();
				_authenticateResult = AuthenticateResult.AuthenticationFailed(_failedResponse);
			}

			private Response _failedResponse;
			private AuthenticateResult _authenticateResult;

			[Test]
			public void Must_set_failed_response()
			{
				Assert.That(_authenticateResult.FailedResponse, Is.SameAs(_failedResponse));
			}

			[Test]
			public void Must_set_matching_result_type()
			{
				Assert.That(_authenticateResult.ResultType, Is.EqualTo(AuthenticateResultType.AuthenticationFailed));
			}
		}

		[TestFixture]
		public class When_creating_instance_with_no_authentication_performed_result_type
		{
			[SetUp]
			public void SetUp()
			{
				_authenticateResult = AuthenticateResult.NoAuthenticationPerformed();
			}

			private AuthenticateResult _authenticateResult;

			[Test]
			public void Must_not_set_failed_response()
			{
				Assert.That(_authenticateResult.FailedResponse, Is.Null);
			}

			[Test]
			public void Must_set_matching_result_type()
			{
				Assert.That(_authenticateResult.ResultType, Is.EqualTo(AuthenticateResultType.NoAuthenticationPerformed));
			}
		}
	}
}
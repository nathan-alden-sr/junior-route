using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class BasicAuthorizationHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_authorization_header
		{
			[Test]
			[TestCase("basic")]
			[TestCase("basic x")]
			[TestCase("basic ,;")]
			[TestCase("basic2 dXNlcmlkOnBhc3N3b3Jk")]
			[TestCase("basic dXNlOnJpZDpwYXNzd29yZA==")]
			[TestCase("basic dXNlcmlkcGFzc3dvcmQ=")]
			[TestCase("BASIC dXNlcmlkOnBhc3N3b3Jk")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(BasicAuthorizationHeader.Parse(headerValue), Is.Null);
			}
		}

		[TestFixture]
		public class When_parsing_valid_authorization_header
		{
			[Test]
			[TestCase("basic dXNlcmlkOnBhc3N3b3Jk", "userid", "password")]
			[TestCase("Basic dXNlcjpwYXNzMQ==", "user", "pass1")]
			public void Must_parse_correctly(string headerValue, string expectedUserid, string expectedPassword)
			{
				BasicAuthorizationHeader header = BasicAuthorizationHeader.Parse(headerValue);

				Assert.That(header, Is.Not.Null);
				Assert.That(header.AuthScheme, Is.StringMatching("[Bb]asic"));
				Assert.That(header.Userid, Is.EqualTo(expectedUserid));
				Assert.That(header.Password, Is.EqualTo(expectedPassword));
			}
		}
	}
}
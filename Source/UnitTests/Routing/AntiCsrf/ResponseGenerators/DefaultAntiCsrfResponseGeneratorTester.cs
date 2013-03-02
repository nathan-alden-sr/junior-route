using System.Net;

using Junior.Route.Routing.AntiCsrf.ResponseGenerators;
using Junior.Route.Routing.AntiCsrf.Validators;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Routing.AntiCsrf.ResponseGenerators
{
	public static class DefaultAntiCsrfResponseGeneratorTester
	{
		[TestFixture]
		public class When_handling_validation_results_that_are_failures
		{
			[SetUp]
			public void SetUp()
			{
				_responseGenerator = new DefaultAntiCsrfResponseGenerator();
			}

			private DefaultAntiCsrfResponseGenerator _responseGenerator;

			[Test]
			[TestCase(ValidationResult.CookieMissing)]
			[TestCase(ValidationResult.FormFieldMissing)]
			[TestCase(ValidationResult.TokensDoNotMatch)]
			public void Must_generate_unauthorized_response(ValidationResult validationResult)
			{
				ResponseResult responseResult = _responseGenerator.GetResponse(validationResult);

				Assert.That(responseResult.Response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
			}
		}

		[TestFixture]
		public class When_handling_validation_results_that_are_not_failures
		{
			[SetUp]
			public void SetUp()
			{
				_responseGenerator = new DefaultAntiCsrfResponseGenerator();
			}

			private DefaultAntiCsrfResponseGenerator _responseGenerator;

			[Test]
			[TestCase(ValidationResult.TokensMatch)]
			[TestCase(ValidationResult.ValidationDisabled)]
			[TestCase(ValidationResult.ValidationSkipped)]
			public void Must_not_generate_response(ValidationResult result)
			{
				Assert.That(_responseGenerator.GetResponse(result).ResultType, Is.EqualTo(ResponseResultType.ResponseNotGenerated));
			}
		}
	}
}
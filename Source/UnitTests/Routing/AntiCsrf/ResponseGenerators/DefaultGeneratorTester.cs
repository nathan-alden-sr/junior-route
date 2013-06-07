using System.Net;

using Junior.Route.Routing.AntiCsrf.NonceValidators;
using Junior.Route.Routing.AntiCsrf.ResponseGenerators;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Routing.AntiCsrf.ResponseGenerators
{
	public static class DefaultGeneratorTester
	{
		[TestFixture]
		public class When_handling_validation_results_that_are_failures
		{
			[SetUp]
			public void SetUp()
			{
				_generator = new DefaultGenerator();
			}

			private DefaultGenerator _generator;

			[Test]
			[TestCase(ValidationResult.CookieInvalid)]
			[TestCase(ValidationResult.CookieMissing)]
			[TestCase(ValidationResult.FormFieldMissing)]
			[TestCase(ValidationResult.FormFieldInvalid)]
			[TestCase(ValidationResult.NonceInvalid)]
			public async void Must_generate_unauthorized_response(ValidationResult validationResult)
			{
				ResponseResult responseResult = await _generator.GetResponseAsync(validationResult);

				Assert.That(responseResult.Response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
			}
		}

		[TestFixture]
		public class When_handling_validation_results_that_are_not_failures
		{
			[SetUp]
			public void SetUp()
			{
				_generator = new DefaultGenerator();
			}

			private DefaultGenerator _generator;

			[Test]
			[TestCase(ValidationResult.NonceValid)]
			[TestCase(ValidationResult.ValidationDisabled)]
			[TestCase(ValidationResult.ValidationSkipped)]
			public async void Must_not_generate_response(ValidationResult result)
			{
				Assert.That((await _generator.GetResponseAsync(result)).ResultType, Is.EqualTo(ResponseResultType.ResponseNotGenerated));
			}
		}
	}
}
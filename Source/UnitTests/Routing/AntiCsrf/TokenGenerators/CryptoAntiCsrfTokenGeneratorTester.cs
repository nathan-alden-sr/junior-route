using System;

using Junior.Route.Routing.AntiCsrf.TokenGenerators;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.AntiCsrf.TokenGenerators
{
	public static class CryptoAntiCsrfTokenGeneratorTester
	{
		[TestFixture]
		public class When_generating_random_token
		{
			[SetUp]
			public void SetUp()
			{
				_configuration = MockRepository.GenerateMock<ICryptoAntiCsrfTokenGeneratorConfiguration>();
				_configuration.Stub(arg => arg.LengthInBytes).Return(LengthInBytes);
				_tokenGenerator = new CryptoAntiCsrfTokenGenerator(_configuration);
			}

			private const int LengthInBytes = 256;

			private ICryptoAntiCsrfTokenGeneratorConfiguration _configuration;
			private CryptoAntiCsrfTokenGenerator _tokenGenerator;

			[Test]
			public void Must_generate_base64_string()
			{
				string token = _tokenGenerator.Generate();

				Assert.DoesNotThrow(() => Convert.FromBase64String(token));
			}

			[Test]
			public void Must_generate_token_of_the_correct_length()
			{
				string token = _tokenGenerator.Generate();
				byte[] bytes = Convert.FromBase64String(token);

				Assert.That(bytes, Has.Length.EqualTo(LengthInBytes));
			}

			[Test]
			public void Must_use_configured_length()
			{
				_configuration.AssertWasCalled(arg => arg.LengthInBytes);
			}
		}
	}
}
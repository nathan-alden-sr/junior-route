using System;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class DigestAuthorizationHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_authorization_header
		{
			[Test]
			[TestCase("digest")]
			[TestCase("digest username=\"Username\", realm=\"realm@host.com\", nonce=\"dcd98b7102dd2f0e8b11d0f600bfb0c093\", uri=\"/home\"")]
			[TestCase("Digest username=\"Username\", realm=\"realm@host.com\", nonce=\"dcd98b7102dd2f0e8b11d0f600bfb0c093\", response=\"6629fae49393a05397450978507c4ef1\"")]
			[TestCase("digest username=\"Username\", realm=\"realm@host.com\", uri=\"/home\", response=\"6629fae49393a05397450978507c4ef1\"")]
			[TestCase("Digest username=\"Username\", nonce=\"dcd98b7102dd2f0e8b11d0f600bfb0c093\", uri=\"/home\", response=\"6629fae49393a05397450978507c4ef1\"")]
			[TestCase("digest realm=\"realm@host.com\", nonce=\"dcd98b7102dd2f0e8b11d0f600bfb0c093\", uri=\"/home\", response=\"6629fae49393a05397450978507c4ef1\"")]
			[TestCase("DIGEST username=\"Username\", realm=\"realm@host.com\", nonce=\"dcd98b7102dd2f0e8b11d0f600bfb0c093\", uri=\"/home\", response=\"6629fae49393a05397450978507c4ef1\"")]
			[TestCase("digest username=\"Username\", realm=\"realm@host.com\", nonce=\"dcd98b7102dd2f0e8b11d0f600bfb0c093\", uri=\"/home\", response=\"6629fae49393a05397450978507c4ef1")]
			[TestCase("digest username=\"Username\", realm=\"realm@host.com\", nonce=\"dcd98b7102dd2f0e8b11d0f600bfb0c093\", uri=\"/home\", response=6629fae49393a05397450978507c4ef1\"")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(DigestAuthorizationHeader.Parse(headerValue), Is.Null);
			}
		}

		[TestFixture]
		public class When_parsing_valid_authorization_header
		{
			[Test]
			[TestCase(
				"digest username=\"Username\", realm=\"realm@host.com\", nonce=\"dcd98b7102dd2f0e8b11d0f600bfb0c093\", uri=\"/home\", response=\"6629fae49393a05397450978507c4ef1\", algorithm=MD5, cnonce=\"abcd3456\", opaque=\"5ccc069c403ebaf9f0171e9517f40e41\", qop=auth, nc=12345678",
				"Username",
				"realm@host.com",
				"dcd98b7102dd2f0e8b11d0f600bfb0c093",
				"/home",
				"6629fae49393a05397450978507c4ef1",
				"MD5",
				"abcd3456",
				"5ccc069c403ebaf9f0171e9517f40e41",
				"auth",
				"12345678")]
			[TestCase(
				"digest username=\"Username\",\r\n realm=\"realm@host.com\",\r\n nonce=\"dcd98b7102dd2f0e8b11d0f600bfb0c093\",\r\n uri=\"/home\",\r\n response=\"6629fae49393a05397450978507c4ef1\",\r\n algorithm=MD5,\r\n cnonce=\"abcd3456\",\r\n opaque=\"5ccc069c403ebaf9f0171e9517f40e41\",\r\n qop=auth,\r\n nc=12345678",
				"Username",
				"realm@host.com",
				"dcd98b7102dd2f0e8b11d0f600bfb0c093",
				"/home",
				"6629fae49393a05397450978507c4ef1",
				"MD5",
				"abcd3456",
				"5ccc069c403ebaf9f0171e9517f40e41",
				"auth",
				"12345678")]
			public void Must_parse_correctly(string headerValue, string username, string realm, string nonce, string digestUri, string response, string algorithm, string cnonce, string opaque, string messageQop, string nonceCount)
			{
				DigestAuthorizationHeader header = DigestAuthorizationHeader.Parse(headerValue);

				Assert.That(header, Is.Not.Null);
				Assert.That(header.AuthScheme, Is.StringMatching("[Dd]igest"));
				Assert.That(header.Username, Is.EqualTo(username));
				Assert.That(header.Realm, Is.EqualTo(realm));
				Assert.That(header.Nonce, Is.EqualTo(nonce));
				Assert.That(header.DigestUri, Is.EqualTo(new Uri(digestUri, UriKind.RelativeOrAbsolute)));
				Assert.That(header.Response, Is.EqualTo(response));
				Assert.That(header.Algorithm, Is.EqualTo(algorithm));
				Assert.That(header.Cnonce, Is.EqualTo(cnonce));
				Assert.That(header.Opaque, Is.EqualTo(opaque));
				Assert.That(header.MessageQop, Is.EqualTo(messageQop));
				Assert.That(header.NonceCount, Is.EqualTo(nonceCount));
			}
		}
	}
}
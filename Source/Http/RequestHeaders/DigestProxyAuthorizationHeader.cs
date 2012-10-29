using System;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class DigestProxyAuthorizationHeader
	{
		private const string AlgorithmRegexPattern = "algorithm=(MD5|MD5-sess|" + CommonRegexPatterns.Token + ")";
		private const string AuthSchemeRegexPattern = "[Dd]igest";
		private const string CnonceRegexPattern = "cnonce=" + CnonceValueRegexPattern;
		private const string CnonceValueRegexPattern = NonceValueRegexPattern;
		private const string DigestUriRegexPattern = "uri=" + DigestUriValueRegexPattern;
		private const string DigestUriValueRegexPattern = @"""" + CommonRegexPatterns.Uri + @"""";
		private const string LhexRegexPattern = @"\x30-\x39\x61-\x66";
		private const string MessageQopRegexPattern = "qop=" + MessageQopValueRegexPattern;
		private const string MessageQopValueRegexPattern = "auth|auth-int|" + CommonRegexPatterns.Token;
		private const string NcValueRegexPattern = "[" + LhexRegexPattern + "]{8}";
		private const string NonceCountRegexPattern = "nc=" + NcValueRegexPattern;
		private const string NonceRegexPattern = "nonce=" + NonceValueRegexPattern;
		private const string NonceValueRegexPattern = CommonRegexPatterns.QuotedString;
		private const string OpaqueRegexPattern = "opaque=" + OpaqueValueRegexPattern;
		private const string OpaqueValueRegexPattern = CommonRegexPatterns.QuotedString;
		private const string RealmRegexPattern = "realm=" + RealmValueRegexPattern;
		private const string RealmValueRegexPattern = CommonRegexPatterns.QuotedString;
		private const string RequestDigestRegexPattern = @"""[" + LhexRegexPattern + @"]{32}""";
		private const string ResponseRegexPattern = "response=" + RequestDigestRegexPattern;
		private const string UsernameRegexPattern = "username=" + UsernameValueRegexPattern;
		private const string UsernameValueRegexPattern = CommonRegexPatterns.QuotedString;
		private readonly string _algorithm;
		private readonly string _authScheme;
		private readonly string _cnonce;
		private readonly Uri _digestUri;
		private readonly string _messageQop;
		private readonly string _nonce;
		private readonly string _nonceCount;
		private readonly string _opaque;
		private readonly string _realm;
		private readonly string _response;
		private readonly string _username;

		private DigestProxyAuthorizationHeader(string authScheme, string username, string realm, string nonce, Uri digestUri, string response, string algorithm, string cnonce, string opaque, string messageQop, string nonceCount)
		{
			authScheme.ThrowIfNull("authScheme");
			username.ThrowIfNull("username");
			realm.ThrowIfNull("realm");
			nonce.ThrowIfNull("nonce");
			digestUri.ThrowIfNull("digestUri");
			response.ThrowIfNull("response");

			_authScheme = authScheme;
			_username = username;
			_realm = realm;
			_nonce = nonce;
			_digestUri = digestUri;
			_response = response;
			_algorithm = algorithm;
			_cnonce = cnonce;
			_opaque = opaque;
			_messageQop = messageQop;
			_nonceCount = nonceCount;
		}

		public string AuthScheme
		{
			get
			{
				return _authScheme;
			}
		}

		public string Username
		{
			get
			{
				return _username;
			}
		}

		public string Realm
		{
			get
			{
				return _realm;
			}
		}

		public string Nonce
		{
			get
			{
				return _nonce;
			}
		}

		public Uri DigestUri
		{
			get
			{
				return _digestUri;
			}
		}

		public string Response
		{
			get
			{
				return _response;
			}
		}

		public string Algorithm
		{
			get
			{
				return _algorithm;
			}
		}

		public string Cnonce
		{
			get
			{
				return _cnonce;
			}
		}

		public string Opaque
		{
			get
			{
				return _opaque;
			}
		}

		public string MessageQop
		{
			get
			{
				return _messageQop;
			}
		}

		public string NonceCount
		{
			get
			{
				return _nonceCount;
			}
		}

		public static DigestProxyAuthorizationHeader Parse(string headerValue)
		{
			if (headerValue == null)
			{
				return null;
			}

			string[] spaceSplitParts = headerValue.SplitSpaces(2);

			if (spaceSplitParts.Length < 2)
			{
				return null;
			}

			string authScheme = spaceSplitParts[0];

			if (!Regex.IsMatch(authScheme, AuthSchemeRegexPattern))
			{
				return null;
			}

			string[] parameters = spaceSplitParts[1].SplitElements();
			string username = null;
			string realm = null;
			string nonce = null;
			string digestUri = null;
			string response = null;
			string algorithm = null;
			string cnonce = null;
			string opaque = null;
			string messageQop = null;
			string nonceCount = null;

			foreach (string parameter in parameters)
			{
				if (username == null && Regex.IsMatch(parameter, UsernameRegexPattern))
				{
					username = parameter.GetParameterValue<string>(true);
				}
				else if (realm == null && Regex.IsMatch(parameter, RealmRegexPattern))
				{
					realm = parameter.GetParameterValue<string>(true);
				}
				else if (nonce == null && Regex.IsMatch(parameter, NonceRegexPattern))
				{
					nonce = parameter.GetParameterValue<string>(true);
				}
				else if (digestUri == null && Regex.IsMatch(parameter, DigestUriRegexPattern))
				{
					digestUri = parameter.GetParameterValue<string>(true);
				}
				else if (response == null && Regex.IsMatch(parameter, ResponseRegexPattern))
				{
					response = parameter.GetParameterValue<string>(true);
				}
				else if (algorithm == null && Regex.IsMatch(parameter, AlgorithmRegexPattern))
				{
					algorithm = parameter.GetParameterValue<string>();
				}
				else if (cnonce == null && Regex.IsMatch(parameter, CnonceRegexPattern))
				{
					cnonce = parameter.GetParameterValue<string>(true);
				}
				else if (opaque == null && Regex.IsMatch(parameter, OpaqueRegexPattern))
				{
					opaque = parameter.GetParameterValue<string>(true);
				}
				else if (messageQop == null && Regex.IsMatch(parameter, MessageQopRegexPattern))
				{
					messageQop = parameter.GetParameterValue<string>();
				}
				else if (nonceCount == null && Regex.IsMatch(parameter, NonceCountRegexPattern))
				{
					nonceCount = parameter.GetParameterValue<string>();
				}
			}

			if (username == null || realm == null || nonce == null || digestUri == null || response == null)
			{
				return null;
			}

			Uri parsedDigestUri = digestUri.IfNotNull(arg => Uri.IsWellFormedUriString(arg, UriKind.RelativeOrAbsolute) ? new Uri(digestUri, UriKind.RelativeOrAbsolute) : null);

			return new DigestProxyAuthorizationHeader(authScheme, username, realm, nonce, parsedDigestUri, response, algorithm, cnonce, opaque, messageQop, nonceCount);
		}
	}
}
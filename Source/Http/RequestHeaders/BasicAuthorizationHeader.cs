using System;
using System.Text;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class BasicAuthorizationHeader
	{
		private const string PasswordRegexPattern = CommonRegexPatterns.Text;
		private const string RegexPattern = "^[Bb]asic" + CommonRegexPatterns.SP + CommonRegexPatterns.Base64 + "$";
		private const string UseridRegexPattern = CommonRegexPatterns.Text;
		private readonly string _authScheme;
		private readonly string _password;
		private readonly string _userid;

		private BasicAuthorizationHeader(string authScheme, string userid, string password)
		{
			authScheme.ThrowIfNull("authScheme");
			userid.ThrowIfNull("userid");
			password.ThrowIfNull("password");

			_authScheme = authScheme;
			_userid = userid;
			_password = password;
		}

		public string AuthScheme
		{
			get
			{
				return _authScheme;
			}
		}

		public string Userid
		{
			get
			{
				return _userid;
			}
		}

		public string Password
		{
			get
			{
				return _password;
			}
		}

		public static BasicAuthorizationHeader Parse(string headerValue)
		{
			if (headerValue == null || !Regex.IsMatch(headerValue, RegexPattern))
			{
				return null;
			}

			string[] spaceSplitParts = headerValue.SplitSpaces(2);
			byte[] bytes;

			try
			{
				bytes = Convert.FromBase64String(spaceSplitParts[1]);
			}
			catch
			{
				return null;
			}

			string useridAndPassword = Encoding.ASCII.GetString(bytes);
			string[] useridAndPasswordParts = useridAndPassword.Split(':');

			return useridAndPasswordParts.Length == 2 && Regex.IsMatch(useridAndPasswordParts[0], UseridRegexPattern) && Regex.IsMatch(useridAndPasswordParts[1], PasswordRegexPattern)
				? new BasicAuthorizationHeader(spaceSplitParts[0], useridAndPasswordParts[0], useridAndPasswordParts[1])
				: null;
		}
	}
}
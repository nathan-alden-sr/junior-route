using System;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class ContentMd5Header
	{
		private const string RegexPattern = "^" + CommonRegexPatterns.Base64 + "$";
		private readonly byte[] _md5Digest;

		private ContentMd5Header(byte[] md5Digest)
		{
			md5Digest.ThrowIfNull("md5Digest");

			_md5Digest = md5Digest;
		}

		public byte[] Md5Digest
		{
			get
			{
				return _md5Digest;
			}
		}

		public static ContentMd5Header Parse(string headerValue)
		{
			if (headerValue == null || !Regex.IsMatch(headerValue, RegexPattern))
			{
				return null;
			}

			byte[] hash = Convert.FromBase64String(headerValue);

			return hash.Length == 16 ? new ContentMd5Header(hash) : null;
		}
	}
}
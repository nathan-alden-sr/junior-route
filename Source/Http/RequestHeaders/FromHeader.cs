using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class FromHeader
	{
		private const string MailboxRegexPattern = @"((?>[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+\x20*|""((?=[\x01-\x7f])[^""\\]|\\[\x01-\x7f])*""\x20*)*(?<angle><))?((?!\.)(?>\.?[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+)+|""((?=[\x01-\x7f])[^""\\]|\\[\x01-\x7f])*"")@(((?!-)[a-zA-Z\d\-]+(?<!-)\.)+[a-zA-Z]{2,}|\[(((?(?<!\[)\.)(25[0-5]|2[0-4]\d|[01]?\d?\d)){4}|[a-zA-Z\d\-]*[a-zA-Z\d]:((?=[\x01-\x7f])[^\\\[\]]|\\[\x01-\x7f])+)\])(?(angle)>)";
		private const string RegexPattern = "^" + MailboxRegexPattern + "$";
		private readonly string _mailbox;

		private FromHeader(string mailbox)
		{
			mailbox.ThrowIfNull("mailbox");

			_mailbox = mailbox;
		}

		public string Mailbox
		{
			get
			{
				return _mailbox;
			}
		}

		public static FromHeader Parse(string headerValue)
		{
			if (headerValue == null)
			{
				return null;
			}

			return Regex.IsMatch(headerValue, RegexPattern) ? new FromHeader(headerValue) : null;
		}
	}
}
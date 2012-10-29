using System;
using System.Globalization;

namespace Junior.Route.Http
{
	public static class CommonRegexPatterns
	{
		public const string Alpha = @"A-Za-z";
		public const string Alphanum = Alpha + Digit;
		public const string AsctimeDate = "(?:(?:" + Wkday + ")" + SP + Date3 + SP + Time + SP + "[" + Digit + "]{4}" + ")";
		public const string Attribute = Token;
		public const string Base64 = "(?:(?:[" + UpAlpha + LoAlpha + Digit + "+/]{4})*(?:[" + UpAlpha + LoAlpha + Digit + "+/]{2}==|[" + UpAlpha + LoAlpha + Digit + "+/]{3}=)?)";
		public const string BytesUnit = "bytes";
		public const string CR = @"\r";
		public const string Char = @"\x00-\x7f";
		public const string Charset = Token;
		public const string Comment = @"\(.*\)";
		public const string ContentCoding = Token;
		public const string CrLf = CR + LF;
		public const string Ctl = @"\x00-\x1f\x7f";
		public const string Date1 = "[" + Digit + "]{2}" + SP + "(?:" + Month + ")" + SP + "[" + Digit + "]{4}";
		public const string Date2 = "[" + Digit + "]{2}-(?:" + Month + ")-[" + Digit + "]{2}";
		public const string Date3 = "(?:" + Month + ")" + SP + "(?:[" + Digit + "]{2}|" + SP + "[" + Digit + "])";
		public const string DeltaSeconds = "[" + Digit + "]+";
		public const string Digit = @"0-9";
		public const string Domainlabel = "[" + Alphanum + "]|[" + Alphanum + "](?:[-" + Alphanum + "])*[" + Alphanum + "]";
		public const string EntityTag = "(?:" + Weak + ")?" + OpaqueTag;
		public const string FieldName = Token;
		public const string HT = @"\t";
		public const string Hex = "A-Fa-f0-9";
		public const string Host = "(?:" + Hostname + ")|(?:" + IPv4Address + ")";
		public const string Hostname = "(?:(?:" + Domainlabel + @")\.)*(?:" + Toplabel + @")\.?";
		public const string Hostport = "(?:(?:" + Host + @")(?:\:" + Port + ")?)";
		public const string HttpDate = "(?:(?:" + Rfc1123Date + ")|(?:" + Rfc850Date + ")|(?:" + AsctimeDate + "))";
		public const string IPv4Address = @"(?:25[0-5]|2[0-4]\d|[01]\d{0,2})(?:\.(?:25[0-5]|2[0-4]\d|[01]\d{0,2})){3}";
		public const string LF = @"\n";
		public const string LoAlpha = @"a-z";
		public const string Lws = "(?:" + CrLf + ")?[" + SP + HT + "]+";
		public const string Month = "Jan|Fed|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec";
		public const string Octet = @"\x00-\xff";
		public const string OpaqueTag = QuotedString;
		public const string Parameter = Token + "=(?:" + Token + "|" + QuotedString + ")";
		public const string Port = @"(?:6553[0-5]|655[0-2]\d|65[0-4]\d\d|6[0-4]\d{3}|[1-5]\d{4}|[1-9]\d{0,3}|0)";
		public const string Product = Token + "(:?/" + ProductVersion + ")?";
		public const string ProductVersion = Token;
		public const string Pseudonym = Token;
		public const string Q = "q";
		public const string QdText = @"(?:[\!\#-\[\]-\~\x80-\xff]|" + Lws + ")";
		public const string Quote = @"""";
		public const string QuotedPair = @"\\[" + Char + "]";
		public const string QuotedString = @"""(?:" + QdText + "|" + QuotedPair + @")*""";
		public const string Qvalue = @"(0(?:\.[" + Digit + @"]{0,3})?)|(1(?:\.(?:0{0,3})?)?)";
		public const string Rfc1123Date = "(?:(?:" + Wkday + ")," + SP + Date1 + SP + Time + SP + "GMT)";
		public const string Rfc850Date = "(?:(?:" + Weekday + ")," + SP + Date2 + SP + Time + SP + "GMT)";
		public const string SP = @" ";
		public const string Separators = @"\(\)\<\>\@\,\;\:\\\""\/\[\]\?\=\{\}" + SP + HT;
		public const string Text = @"(?:[ -\xff]|" + Lws + ")";
		public const string Time = "[" + Digit + "]{2}:[" + Digit + "]{2}:[" + Digit + "]{2}";
		public const string Token = "[" + Char + "-[" + Ctl + Separators + "]]+";
		public const string Toplabel = "[" + Alpha + "]|[" + Alpha + "](?:[-" + Alphanum + "])*[" + Alphanum + "]";
		public const string TransferExtension = Token + "(?:;" + SP + "*(?:" + Parameter + "))*";
		public const string UpAlpha = @"A-Z";
		public const string Uri = @"\S+";
		public const string Weak = "W/";
		public const string Weekday = "Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday";
		public const string Wkday = "Mon|Tue|Wed|Thu|Fri|Sat|Sun";

		public static string ListOfElements(string elementRegexPattern, int minimum = 0, int? maximum = null)
		{
			if (maximum < minimum)
			{
				throw new ArgumentException("Maximum cannot be less than minimum.", "maximum");
			}
			if (minimum < 0)
			{
				throw new ArgumentOutOfRangeException("minimum", "Minimum cannot be less than 0.");
			}
			if (maximum < 1)
			{
				throw new ArgumentOutOfRangeException("maximum", "Maximum cannot be less than 1.");
			}
			if (maximum == 1)
			{
				return String.Format("(?:(?:{0})?{1}){2}", Lws, elementRegexPattern, minimum == 0 ? "?" : "");
			}

			return String.Format(
				"(?:(?:{0})?{1}(?:(?:{0})?,(?:{0})?{1}){{{2},{3}}}){4}",
				Lws,
				elementRegexPattern,
				Math.Max(0, minimum - 1),
				maximum != null ? (maximum.Value - 1).ToString(CultureInfo.InvariantCulture) : "",
				minimum == 0 ? "?" : "");
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class ViaHeader
	{
		private const string ProtocolNameRegexPattern = CommonRegexPatterns.Token;
		private const string ProtocolVersionRegexPattern = CommonRegexPatterns.Token;
		private const string ReceivedByRegexPattern = "(?:" + CommonRegexPatterns.Hostport + "|" + CommonRegexPatterns.Pseudonym + ")";
		private const string ReceivedProtocolRegexPattern = "(?:" + ProtocolNameRegexPattern + "/)?" + ProtocolVersionRegexPattern;
		private const string RegexPattern = "(?:" + ReceivedProtocolRegexPattern + CommonRegexPatterns.SP + "+" + ReceivedByRegexPattern + "(?:" + CommonRegexPatterns.SP + "+" + CommonRegexPatterns.Comment + ")?)";
		private static readonly string _elementsRegexPattern = String.Format("^{0}$", CommonRegexPatterns.ListOfElements(RegexPattern, 1));
		private readonly string _receivedBy;
		private readonly string _receivedProtocol;

		private ViaHeader(string receivedProtocol, string receivedBy)
		{
			receivedProtocol.ThrowIfNull("receivedProtocol");
			receivedBy.ThrowIfNull("receivedBy");

			_receivedProtocol = receivedProtocol;
			_receivedBy = receivedBy;
		}

		public string ReceivedProtocol
		{
			get
			{
				return _receivedProtocol;
			}
		}

		public string ReceivedBy
		{
			get
			{
				return _receivedBy;
			}
		}

		public static IEnumerable<ViaHeader> ParseMany(string headerValue)
		{
			if (headerValue == null)
			{
				return Enumerable.Empty<ViaHeader>();
			}

			return Regex.IsMatch(headerValue, _elementsRegexPattern)
				       ? headerValue.SplitElements()
					         .Select(arg => arg.SplitSpaces())
					         .Select(arg => new ViaHeader(arg[0], arg[1]))
				       : Enumerable.Empty<ViaHeader>();
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class TransferEncodingHeader
	{
		private static readonly string _elementsRegexPattern = "^(?:chunked|" + String.Format("^(?:{0})$", CommonRegexPatterns.ListOfElements(CommonRegexPatterns.TransferExtension, 1)) + ")";

		private readonly string _transferCoding;

		private TransferEncodingHeader(string transferCoding)
		{
			transferCoding.ThrowIfNull("transferCoding");

			_transferCoding = transferCoding;
		}

		public string TransferCoding
		{
			get
			{
				return _transferCoding;
			}
		}

		public static IEnumerable<TransferEncodingHeader> ParseMany(string headerValue)
		{
			if (headerValue == null)
			{
				return Enumerable.Empty<TransferEncodingHeader>();
			}
			return Regex.IsMatch(headerValue, _elementsRegexPattern) ? headerValue.SplitElements().Select(arg => new TransferEncodingHeader(arg)) : Enumerable.Empty<TransferEncodingHeader>();
		}
	}
}
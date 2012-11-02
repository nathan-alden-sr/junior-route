using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Junior.Route.Http.RequestHeaders
{
	public class RangeHeader
	{
		private const string ByteRangeSpecRegexPattern = FirstBytePosRegexPattern + "-(?:" + LastBytePosRegexPattern + ")?";
		private const string FirstBytePosRegexPattern = "[" + CommonRegexPatterns.Digit + "]+";
		private const string LastBytePosRegexPattern = "[" + CommonRegexPatterns.Digit + "]+";
		private const string SuffixByteRangeSpecRegexPattern = "-" + SuffixLengthRegexPattern;
		private const string SuffixLengthRegexPattern = "[" + CommonRegexPatterns.Digit + "]+";
		private static readonly string _byteRangeSetRegexPattern = CommonRegexPatterns.ListOfElements("(?:" + ByteRangeSpecRegexPattern + "|" + SuffixByteRangeSpecRegexPattern + ")", 1);
		private static readonly string _byteRangesSpecifierRegexPattern = CommonRegexPatterns.BytesUnit + "=" + _byteRangeSetRegexPattern;
		private static readonly string _rangesSpecifierRegexPattern = _byteRangesSpecifierRegexPattern;
		private readonly ulong? _firstBytePos;
		private readonly ulong? _lastBytePos;

		private RangeHeader(ulong? firstBytePos, ulong? lastBytePos)
		{
			if (firstBytePos == null && lastBytePos == null)
			{
				throw new ArgumentException("first-byte-pos and last-byte-pos cannot both be null.");
			}
			if (firstBytePos > lastBytePos)
			{
				throw new ArgumentException("first-byte-pos cannot be greater than last-byte-pos.", "firstBytePos");
			}

			_firstBytePos = firstBytePos;
			_lastBytePos = lastBytePos;
		}

		public ulong? FirstBytePos
		{
			get
			{
				return _firstBytePos;
			}
		}

		public ulong? LastBytePos
		{
			get
			{
				return _lastBytePos;
			}
		}

		public static IEnumerable<RangeHeader> ParseMany(string headerValue)
		{
			if (headerValue == null || !Regex.IsMatch(headerValue, _rangesSpecifierRegexPattern))
			{
				yield break;
			}

			string[] elements = headerValue.Remove(0, 6).SplitElements();

			foreach (string element in elements)
			{
				string[] byteRangeParts = element.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

				if (byteRangeParts.Length == 2)
				{
					ulong firstBytePos = UInt64.Parse(byteRangeParts[0]);
					ulong lastBytePos = UInt64.Parse(byteRangeParts[1]);

					if (firstBytePos > lastBytePos)
					{
						yield break;
					}
					yield return new RangeHeader(firstBytePos, lastBytePos);
				}
				else if (element[0] == '-')
				{
					yield return new RangeHeader(null, UInt64.Parse(byteRangeParts[0]));
				}
				else
				{
					yield return new RangeHeader(UInt64.Parse(byteRangeParts[0]), null);
				}
			}
		}
	}
}
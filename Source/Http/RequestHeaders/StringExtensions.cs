using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public static class StringExtensions
	{
		public static string[] SplitElements(this string value)
		{
			value.ThrowIfNull("value");

			return value.SplitOnCommasOutsideQuotes()
				.Select(arg => arg.Trim())
				.Where(arg => arg.Length > 0)
				.ToArray();
		}

		public static string[] SplitSemicolons(this string value)
		{
			value.ThrowIfNull("value");

			return value.Split(';')
				.Select(arg => arg.Trim())
				.Where(arg => arg.Length > 0)
				.ToArray();
		}

		public static string[] SplitEqualSign(this string value)
		{
			value.ThrowIfNull("value");

			return value.Split('=')
				.Select(arg => arg.Trim())
				.Where(arg => arg.Length > 0)
				.ToArray();
		}

		public static string[] SplitSpaces(this string value, int? count = null)
		{
			value.ThrowIfNull("value");

			string[] stringSplitParts = count != null ? value.Split(new[] { ' ' }, count.Value) : value.Split(new[] { ' ' });

			return stringSplitParts
				.Select(arg => arg.Trim())
				.Where(arg => arg.Length > 0)
				.ToArray();
		}

		public static IEnumerable<int> FindCommasOutsideQuotes(this string value, char commaCharacter = ',', char quoteCharacter = '"')
		{
			value.ThrowIfNull("value");

			bool insideQuote = false;

			for (int i = 0; i < value.Length; i++)
			{
				if (value[i] == commaCharacter && !insideQuote)
				{
					yield return i;
				}
				else if (value[i] == quoteCharacter)
				{
					insideQuote = !insideQuote;
				}
			}
		}

		public static IEnumerable<string> SplitOnSpacesOutsideQuotes(this string value, char quoteCharacter = '"')
		{
			return SplitOnCommasOutsideQuotes(value, ' ', quoteCharacter);
		}

		public static IEnumerable<string> SplitOnCommasOutsideQuotes(this string value, char commaCharacter = ',', char quoteCharacter = '"')
		{
			value.ThrowIfNull("value");

			IEnumerable<int> commaIndices = FindCommasOutsideQuotes(value, commaCharacter, quoteCharacter);
			int startIndex = 0;

			foreach (int commaIndex in commaIndices)
			{
				yield return value.Substring(startIndex, commaIndex - startIndex);

				startIndex = commaIndex + 1;
			}

			yield return value.Substring(startIndex);
		}

		public static void GetParameterParts<T>(this string value, out string parameterName, out T parameterValue, bool removeOptionalQuotes = false)
		{
			value.ThrowIfNull("value");

			string[] equalSignSplitParts = value.SplitEqualSign();

			parameterName = equalSignSplitParts[0];
			parameterValue = equalSignSplitParts.Length == 2 ? (T)Convert.ChangeType(removeOptionalQuotes ? RemoveOptionalQuotes(equalSignSplitParts[1]) : equalSignSplitParts[1], typeof(T)) : default(T);
		}

		public static T GetParameterValue<T>(this string value, bool removeOptionalQuotes = false)
		{
			value.ThrowIfNull("value");

			string parameterValue = value.SplitEqualSign()[1];

			return (T)Convert.ChangeType(removeOptionalQuotes ? RemoveOptionalQuotes(parameterValue) : parameterValue, typeof(T));
		}

		public static string RemoveOptionalQuotes(this string value, char quoteCharacter = '"')
		{
			return value.Length >= 2 && value[0] == quoteCharacter && value[value.Length - 1] == quoteCharacter ? value.Substring(1, value.Length - 2) : value;
		}

		public static DateTime? ParseHttpDate(this string value)
		{
			value.ThrowIfNull("value");

			DateTime date;

			if (Regex.IsMatch(value, "^" + CommonRegexPatterns.Rfc1123Date + "$"))
			{
				value = value.Substring(0, value.Length - 4);

				if (!DateTime.TryParseExact(value, "ddd, dd MMM yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out date))
				{
					return null;
				}
			}
			else if (Regex.IsMatch(value, "^" + CommonRegexPatterns.Rfc850Date + "$"))
			{
				value = value.Substring(0, value.Length - 4);

				if (!DateTime.TryParseExact(value, "dddd, dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out date))
				{
					return null;
				}
			}
			else if (Regex.IsMatch(value, "^" + CommonRegexPatterns.AsctimeDate + "$"))
			{
				if (!DateTime.TryParseExact(value, value[8] == ' ' ? "ddd MMM  d HH:mm:ss yyyy" : "ddd MMM dd HH:mm:ss yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out date))
				{
					return null;
				}
			}
			else
			{
				return null;
			}

			return date.ToUniversalTime();
		}
	}
}
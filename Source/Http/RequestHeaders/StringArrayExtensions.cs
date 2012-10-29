using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	internal static class StringArrayExtensions
	{
		public static QvalueValidity ValidateQvalue(this string[] values)
		{
			values.ThrowIfNull("values");

			if (values.Length != 2 || !Regex.IsMatch(values[0], "^" + CommonRegexPatterns.Q + "$"))
			{
				return QvalueValidity.NotQvalue;
			}

			return Regex.IsMatch(values[1], "^(?:" + CommonRegexPatterns.Qvalue + ")$") ? QvalueValidity.Valid : QvalueValidity.Invalid;
		}
	}
}
using System.Text.RegularExpressions;

namespace NathanAlden.JuniorRouting.Core.RequestValueComparers
{
	public class CaseSensitiveRegexRequestValueComparer : IRequestValueComparer
	{
		public static readonly CaseSensitiveRegexRequestValueComparer Default = new CaseSensitiveRegexRequestValueComparer();

		public bool Matches(string value, string requestValue)
		{
			return Regex.IsMatch(requestValue, value);
		}
	}
}
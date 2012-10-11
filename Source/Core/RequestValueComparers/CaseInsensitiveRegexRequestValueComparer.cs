using System.Text.RegularExpressions;

namespace NathanAlden.JuniorRouting.Core.RequestValueComparers
{
	public class CaseInsensitiveRegexRequestValueComparer : IRequestValueComparer
	{
		public static readonly CaseInsensitiveRegexRequestValueComparer Default = new CaseInsensitiveRegexRequestValueComparer();

		public bool Matches(string value, string requestValue)
		{
			return Regex.IsMatch(requestValue, value, RegexOptions.IgnoreCase);
		}
	}
}
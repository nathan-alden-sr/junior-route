using System.Text.RegularExpressions;

namespace Junior.Route.Routing.RequestValueComparers
{
	public class CaseInsensitiveRegexRequestValueComparer : IRequestValueComparer
	{
		public static readonly CaseInsensitiveRegexRequestValueComparer Instance = new CaseInsensitiveRegexRequestValueComparer();

		private CaseInsensitiveRegexRequestValueComparer()
		{
		}

		public bool Matches(string value, string requestValue)
		{
			return Regex.IsMatch(requestValue, value, RegexOptions.IgnoreCase);
		}
	}
}
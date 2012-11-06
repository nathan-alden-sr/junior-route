using System.Text.RegularExpressions;

namespace Junior.Route.Routing.RequestValueComparers
{
	public class CaseInsensitiveRegexComparer : IRequestValueComparer
	{
		public static readonly CaseInsensitiveRegexComparer Instance = new CaseInsensitiveRegexComparer();

		private CaseInsensitiveRegexComparer()
		{
		}

		public bool Matches(string value, string requestValue)
		{
			return Regex.IsMatch(requestValue, value, RegexOptions.IgnoreCase);
		}
	}
}
using System.Text.RegularExpressions;

namespace Junior.Route.Routing.RequestValueComparers
{
	public class CaseSensitiveRegexComparer : IRequestValueComparer
	{
		public static readonly CaseSensitiveRegexComparer Instance = new CaseSensitiveRegexComparer();

		private CaseSensitiveRegexComparer()
		{
		}

		public bool Matches(string value, string requestValue)
		{
			return Regex.IsMatch(requestValue, value);
		}
	}
}
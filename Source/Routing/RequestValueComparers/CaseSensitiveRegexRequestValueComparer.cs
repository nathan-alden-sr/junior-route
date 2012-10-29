using System.Text.RegularExpressions;

namespace Junior.Route.Routing.RequestValueComparers
{
	public class CaseSensitiveRegexRequestValueComparer : IRequestValueComparer
	{
		public static readonly CaseSensitiveRegexRequestValueComparer Instance = new CaseSensitiveRegexRequestValueComparer();

		private CaseSensitiveRegexRequestValueComparer()
		{
		}

		public bool Matches(string value, string requestValue)
		{
			return Regex.IsMatch(requestValue, value);
		}
	}
}
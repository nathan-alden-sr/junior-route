namespace Junior.Route.Routing.RequestValueComparers
{
	public class OptionalCaseSensitiveRegexComparer : IRequestValueComparer
	{
		public static readonly OptionalCaseSensitiveRegexComparer Instance = new OptionalCaseSensitiveRegexComparer();

		private OptionalCaseSensitiveRegexComparer()
		{
		}

		public bool Matches(string value, string requestValue)
		{
			return value == null || CaseSensitiveRegexComparer.Instance.Matches(value, requestValue);
		}
	}
}
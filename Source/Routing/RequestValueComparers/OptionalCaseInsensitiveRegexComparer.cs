namespace Junior.Route.Routing.RequestValueComparers
{
	public class OptionalCaseInsensitiveRegexComparer : IRequestValueComparer
	{
		public static readonly OptionalCaseInsensitiveRegexComparer Instance = new OptionalCaseInsensitiveRegexComparer();

		private OptionalCaseInsensitiveRegexComparer()
		{
		}

		public bool Matches(string value, string requestValue)
		{
			return value == null || CaseInsensitiveRegexComparer.Instance.Matches(value, requestValue);
		}
	}
}
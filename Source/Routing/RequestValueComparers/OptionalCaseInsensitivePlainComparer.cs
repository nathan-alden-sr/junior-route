namespace Junior.Route.Routing.RequestValueComparers
{
	public class OptionalCaseInsensitivePlainComparer : IRequestValueComparer
	{
		public static readonly OptionalCaseInsensitivePlainComparer Instance = new OptionalCaseInsensitivePlainComparer();

		private OptionalCaseInsensitivePlainComparer()
		{
		}

		public bool Matches(string value, string requestValue)
		{
			return value == null || CaseInsensitivePlainComparer.Instance.Matches(value, requestValue);
		}
	}
}
namespace Junior.Route.Routing.RequestValueComparers
{
	public class OptionalCaseSensitivePlainComparer : IRequestValueComparer
	{
		public static readonly OptionalCaseSensitivePlainComparer Instance = new OptionalCaseSensitivePlainComparer();

		private OptionalCaseSensitivePlainComparer()
		{
		}

		public bool Matches(string value, string requestValue)
		{
			return value == null || CaseSensitivePlainComparer.Instance.Matches(value, requestValue);
		}
	}
}
using System;

namespace Junior.Route.Routing.RequestValueComparers
{
	public class CaseInsensitivePlainRequestValueComparer : IRequestValueComparer
	{
		public static readonly CaseInsensitivePlainRequestValueComparer Instance = new CaseInsensitivePlainRequestValueComparer();

		private CaseInsensitivePlainRequestValueComparer()
		{
		}

		public bool Matches(string value, string requestValue)
		{
			return String.Equals(value, requestValue, StringComparison.OrdinalIgnoreCase);
		}
	}
}
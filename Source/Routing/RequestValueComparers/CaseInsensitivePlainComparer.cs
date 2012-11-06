using System;

namespace Junior.Route.Routing.RequestValueComparers
{
	public class CaseInsensitivePlainComparer : IRequestValueComparer
	{
		public static readonly CaseInsensitivePlainComparer Instance = new CaseInsensitivePlainComparer();

		private CaseInsensitivePlainComparer()
		{
		}

		public bool Matches(string value, string requestValue)
		{
			return String.Equals(value, requestValue, StringComparison.OrdinalIgnoreCase);
		}
	}
}
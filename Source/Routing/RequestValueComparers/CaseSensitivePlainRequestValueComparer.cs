using System;

namespace Junior.Route.Routing.RequestValueComparers
{
	public class CaseSensitivePlainRequestValueComparer : IRequestValueComparer
	{
		public static readonly CaseSensitivePlainRequestValueComparer Instance = new CaseSensitivePlainRequestValueComparer();

		private CaseSensitivePlainRequestValueComparer()
		{
		}

		public bool Matches(string value, string requestValue)
		{
			return String.Equals(value, requestValue, StringComparison.Ordinal);
		}
	}
}
using System;

namespace Junior.Route.Routing.RequestValueComparers
{
	public class CaseSensitivePlainComparer : IRequestValueComparer
	{
		public static readonly CaseSensitivePlainComparer Instance = new CaseSensitivePlainComparer();

		private CaseSensitivePlainComparer()
		{
		}

		public bool Matches(string value, string requestValue)
		{
			return String.Equals(value, requestValue, StringComparison.Ordinal);
		}
	}
}
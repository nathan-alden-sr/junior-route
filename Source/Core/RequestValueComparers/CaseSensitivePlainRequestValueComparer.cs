using System;

namespace NathanAlden.JuniorRouting.Core.RequestValueComparers
{
	public class CaseSensitivePlainRequestValueComparer : IRequestValueComparer
	{
		public static readonly CaseSensitivePlainRequestValueComparer Default = new CaseSensitivePlainRequestValueComparer();

		public bool Matches(string value, string requestValue)
		{
			return String.Equals(value, requestValue, StringComparison.Ordinal);
		}
	}
}
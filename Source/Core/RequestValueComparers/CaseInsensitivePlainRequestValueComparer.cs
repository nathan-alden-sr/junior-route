using System;

namespace NathanAlden.JuniorRouting.Core.RequestValueComparers
{
	public class CaseInsensitivePlainRequestValueComparer : IRequestValueComparer
	{
		public static readonly CaseInsensitivePlainRequestValueComparer Default = new CaseInsensitivePlainRequestValueComparer();

		public bool Matches(string value, string requestValue)
		{
			return String.Equals(value, requestValue, StringComparison.OrdinalIgnoreCase);
		}
	}
}
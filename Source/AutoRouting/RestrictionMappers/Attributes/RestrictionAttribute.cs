using System;

using Junior.Route.Routing.RequestValueComparers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	public abstract class RestrictionAttribute : Attribute
	{
		public abstract void Map(Routing.Route route);

		protected IRequestValueComparer GetComparer(RequestValueComparer comparer)
		{
			switch (comparer)
			{
				case RequestValueComparer.CaseInsensitivePlain:
					return CaseInsensitivePlainRequestValueComparer.Instance;
				case RequestValueComparer.CaseInsensitiveRegex:
					return CaseInsensitiveRegexRequestValueComparer.Instance;
				case RequestValueComparer.CaseSensitivePlain:
					return CaseSensitivePlainRequestValueComparer.Instance;
				case RequestValueComparer.CaseSensitiveRegex:
					return CaseSensitiveRegexRequestValueComparer.Instance;
				default:
					throw new ArgumentOutOfRangeException("comparer");
			}
		}
	}
}
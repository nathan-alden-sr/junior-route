using System;

using Junior.Route.AutoRouting.Containers;
using Junior.Route.Routing.RequestValueComparers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	public abstract class RestrictionAttribute : Attribute
	{
		public abstract void Map(Routing.Route route, IContainer container);

		protected IRequestValueComparer GetComparer(RequestValueComparer comparer)
		{
			switch (comparer)
			{
				case RequestValueComparer.CaseInsensitivePlain:
					return CaseInsensitivePlainComparer.Instance;
				case RequestValueComparer.CaseInsensitiveRegex:
					return CaseInsensitiveRegexComparer.Instance;
				case RequestValueComparer.CaseSensitivePlain:
					return CaseSensitivePlainComparer.Instance;
				case RequestValueComparer.CaseSensitiveRegex:
					return CaseSensitiveRegexComparer.Instance;
				case RequestValueComparer.OptionalCaseInsensitivePlain:
					return OptionalCaseInsensitivePlainComparer.Instance;
				case RequestValueComparer.OptionalCaseInsensitiveRegex:
					return OptionalCaseInsensitiveRegexComparer.Instance;
				case RequestValueComparer.OptionalCaseSensitivePlain:
					return OptionalCaseSensitivePlainComparer.Instance;
				case RequestValueComparer.OptionalCaseSensitiveRegex:
					return OptionalCaseSensitiveRegexComparer.Instance;
				default:
					throw new ArgumentOutOfRangeException("comparer");
			}
		}
	}
}
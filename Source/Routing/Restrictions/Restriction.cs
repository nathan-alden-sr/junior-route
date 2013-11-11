using System.Collections.Generic;

namespace Junior.Route.Routing.Restrictions
{
	public static class Restriction
	{
		public static IRestriction And(IEnumerable<IRestriction> restrictions)
		{
			return new AndRestriction(restrictions);
		}

		public static IRestriction And(params IRestriction[] restrictions)
		{
			return new AndRestriction(restrictions);
		}

		public static IRestriction Or(IEnumerable<IRestriction> restrictions)
		{
			return new OrRestriction(restrictions);
		}

		public static IRestriction Or(params IRestriction[] restrictions)
		{
			return new OrRestriction(restrictions);
		}
	}
}
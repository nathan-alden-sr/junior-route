using System;

using Junior.Common;

namespace Junior.Route.AutoRouting.ClassFilters
{
	public class AllFilter : IClassFilter
	{
		public bool Matches(Type type)
		{
			type.ThrowIfNull("type");

			return true;
		}
	}
}
using System;

namespace Junior.Route.AutoRouting.ClassFilters
{
	public interface IClassFilter
	{
		bool Matches(Type type);
	}
}
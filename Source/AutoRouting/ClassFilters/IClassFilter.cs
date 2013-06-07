using System;
using System.Threading.Tasks;

namespace Junior.Route.AutoRouting.ClassFilters
{
	public interface IClassFilter
	{
		Task<bool> MatchesAsync(Type type);
	}
}
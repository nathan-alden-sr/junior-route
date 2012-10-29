using System.Reflection;

namespace Junior.Route.AutoRouting.MethodFilters
{
	public interface IMethodFilter
	{
		bool Matches(MethodInfo method);
	}
}
using System.Reflection;
using System.Threading.Tasks;

namespace Junior.Route.AutoRouting.MethodFilters
{
	public interface IMethodFilter
	{
		Task<bool> MatchesAsync(MethodInfo method);
	}
}
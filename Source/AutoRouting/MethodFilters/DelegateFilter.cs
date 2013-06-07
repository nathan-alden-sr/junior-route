using System;
using System.Reflection;
using System.Threading.Tasks;

using Junior.Common;

namespace Junior.Route.AutoRouting.MethodFilters
{
	public class DelegateFilter : IMethodFilter
	{
		private readonly Func<MethodInfo, bool> _matchDelegate;

		public DelegateFilter(Func<MethodInfo, bool> matchDelegate)
		{
			matchDelegate.ThrowIfNull("matchDelegate");

			_matchDelegate = matchDelegate;
		}

		public Task<bool> MatchesAsync(MethodInfo method)
		{
			method.ThrowIfNull("method");

			return _matchDelegate(method).AsCompletedTask();
		}
	}
}
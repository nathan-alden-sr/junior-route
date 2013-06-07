using System;
using System.Threading.Tasks;

using Junior.Common;

namespace Junior.Route.AutoRouting.ClassFilters
{
	public class DelegateFilter : IClassFilter
	{
		private readonly Func<Type, bool> _matchDelegate;

		public DelegateFilter(Func<Type, bool> matchDelegate)
		{
			matchDelegate.ThrowIfNull("_matchDelegate");

			_matchDelegate = matchDelegate;
		}

		public Task<bool> MatchesAsync(Type type)
		{
			type.ThrowIfNull("type");

			return _matchDelegate(type).AsCompletedTask();
		}
	}
}
using System;
using System.Reflection;

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

		public bool Matches(MethodInfo method)
		{
			method.ThrowIfNull("method");

			return _matchDelegate(method);
		}
	}
}
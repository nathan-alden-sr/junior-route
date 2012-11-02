using System;
using System.Collections.Generic;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class RefererUrlPathAndQueryAttribute : RestrictionAttribute
	{
		private readonly RequestValueComparer? _comparer;
		private readonly IEnumerable<string> _pathsAndQueries;

		public RefererUrlPathAndQueryAttribute(string pathAndQuery, RequestValueComparer comparer)
		{
			_pathsAndQueries = pathAndQuery.ToEnumerable();
			_comparer = comparer;
		}

		public RefererUrlPathAndQueryAttribute(params string[] pathsAndQueries)
		{
			pathsAndQueries.ThrowIfNull("pathsAndQueries");

			_pathsAndQueries = pathsAndQueries;
		}

		public override void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			if (_comparer != null)
			{
				route.RestrictByRefererUrlPathsAndQueries(_pathsAndQueries, GetComparer(_comparer.Value));
			}
			else
			{
				route.RestrictByRefererUrlPathsAndQueries(_pathsAndQueries);
			}
		}
	}
}
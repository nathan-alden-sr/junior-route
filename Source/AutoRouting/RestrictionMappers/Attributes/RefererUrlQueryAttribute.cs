using System;
using System.Collections.Generic;

using Junior.Common;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class RefererUrlQueryAttribute : RestrictionAttribute
	{
		private readonly RequestValueComparer? _comparer;
		private readonly IEnumerable<string> _queries;

		public RefererUrlQueryAttribute(string query, RequestValueComparer comparer)
		{
			query.ThrowIfNull("query");

			_queries = query.ToEnumerable();
			_comparer = comparer;
		}

		public RefererUrlQueryAttribute(params string[] queries)
		{
			queries.ThrowIfNull("queries");

			_queries = queries;
		}

		public override void Map(Routing.Route route)
		{
			route.ThrowIfNull("route");

			if (_comparer != null)
			{
				route.RestrictByRefererUrlQueries(_queries, GetComparer(_comparer.Value));
			}
			else
			{
				route.RestrictByRefererUrlQueries(_queries);
			}
		}
	}
}
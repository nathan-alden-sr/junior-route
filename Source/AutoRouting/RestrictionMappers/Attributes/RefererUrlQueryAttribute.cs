using System;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class RefererUrlQueryAttribute : RestrictionAttribute
	{
		private readonly RequestValueComparer? _comparer;
		private readonly string[] _queries;

		public RefererUrlQueryAttribute(string query, RequestValueComparer comparer)
		{
			query.ThrowIfNull("query");

			_queries = new[] { query };
			_comparer = comparer;
		}

		public RefererUrlQueryAttribute(params string[] queries)
		{
			queries.ThrowIfNull("queries");

			_queries = queries;
		}

		public override void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

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
using System.Diagnostics;
using System.Web;

using Junior.Common;

using NathanAlden.JuniorRouting.Core.RequestValueComparers;

namespace NathanAlden.JuniorRouting.Core.Restrictions
{
	[DebuggerDisplay("{_query}")]
	public class UrlQueryRestriction : IHttpRouteRestriction
	{
		private readonly IRequestValueComparer _comparer;
		private readonly string _query;

		public UrlQueryRestriction(string query, IRequestValueComparer comparer)
		{
			query.ThrowIfNull("query");
			comparer.ThrowIfNull("comparer");

			_query = query;
			_comparer = comparer;
		}

		public string Query
		{
			get
			{
				return _query;
			}
		}

		public IRequestValueComparer Comparer
		{
			get
			{
				return _comparer;
			}
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return _comparer.Matches(_query, request.Url.Query);
		}
	}
}
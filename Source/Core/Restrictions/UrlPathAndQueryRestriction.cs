using System.Diagnostics;
using System.Web;

using Junior.Common;

using NathanAlden.JuniorRouting.Core.RequestValueComparers;

namespace NathanAlden.JuniorRouting.Core.Restrictions
{
	[DebuggerDisplay("{_pathAndQuery}")]
	public class UrlPathAndQueryRestriction : IHttpRouteRestriction
	{
		private readonly IRequestValueComparer _comparer;
		private readonly string _pathAndQuery;

		public UrlPathAndQueryRestriction(string pathAndQuery, IRequestValueComparer comparer)
		{
			pathAndQuery.ThrowIfNull("pathAndQuery");
			comparer.ThrowIfNull("comparer");

			_pathAndQuery = pathAndQuery;
			_comparer = comparer;
		}

		public string PathAndQuery
		{
			get
			{
				return _pathAndQuery;
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

			return _comparer.Matches(_pathAndQuery, request.Url.PathAndQuery);
		}
	}
}
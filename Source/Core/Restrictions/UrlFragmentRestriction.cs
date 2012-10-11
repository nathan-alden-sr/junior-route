using System.Diagnostics;
using System.Web;

using Junior.Common;

using NathanAlden.JuniorRouting.Core.RequestValueComparers;

namespace NathanAlden.JuniorRouting.Core.Restrictions
{
	[DebuggerDisplay("{_fragment}")]
	public class UrlFragmentRestriction : IHttpRouteRestriction
	{
		private readonly IRequestValueComparer _comparer;
		private readonly string _fragment;

		public UrlFragmentRestriction(string fragment, IRequestValueComparer comparer)
		{
			fragment.ThrowIfNull("fragment");
			comparer.ThrowIfNull("comparer");

			_fragment = fragment;
			_comparer = comparer;
		}

		public string Fragment
		{
			get
			{
				return _fragment;
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

			return _comparer.Matches(_fragment, request.Url.Fragment);
		}
	}
}
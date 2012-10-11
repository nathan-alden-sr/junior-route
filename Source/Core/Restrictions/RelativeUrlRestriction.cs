using System.Diagnostics;
using System.Web;

using Junior.Common;

using NathanAlden.JuniorRouting.Core.RequestValueComparers;

namespace NathanAlden.JuniorRouting.Core.Restrictions
{
	[DebuggerDisplay("{_relativeUrl}")]
	public class RelativeUrlRestriction : IHttpRouteRestriction
	{
		private readonly IRequestValueComparer _comparer;
		private readonly string _relativeUrl;

		public RelativeUrlRestriction(string relativeUrl, IRequestValueComparer comparer)
		{
			relativeUrl.ThrowIfNull("url");
			comparer.ThrowIfNull("comparer");

			_relativeUrl = relativeUrl;
			_comparer = comparer;
		}

		public string RelativeUrl
		{
			get
			{
				return _relativeUrl;
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

			return _comparer.Matches(_relativeUrl, GetRelativeUrl(request));
		}

		private static string GetRelativeUrl(HttpRequestBase request)
		{
			string virtualPath = HttpRuntime.AppDomainAppVirtualPath;
			string absolutePath = request.Url.AbsolutePath;

			return absolutePath.Remove(0, virtualPath.Length).TrimStart('/');
		}
	}
}
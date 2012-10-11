using System.Diagnostics;
using System.Web;

using Junior.Common;

using NathanAlden.JuniorRouting.Core.RequestValueComparers;

namespace NathanAlden.JuniorRouting.Core.Restrictions
{
	[DebuggerDisplay("{_localPath}")]
	public class UrlLocalPathRestriction : IHttpRouteRestriction
	{
		private readonly IRequestValueComparer _comparer;
		private readonly string _localPath;

		public UrlLocalPathRestriction(string localPath, IRequestValueComparer comparer)
		{
			localPath.ThrowIfNull("localPath");
			comparer.ThrowIfNull("comparer");

			_localPath = localPath;
			_comparer = comparer;
		}

		public string LocalPath
		{
			get
			{
				return _localPath;
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

			return _comparer.Matches(_localPath, request.Url.LocalPath);
		}
	}
}
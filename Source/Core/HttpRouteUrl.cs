using System.Diagnostics;

using Junior.Common;

using NathanAlden.JuniorRouting.Core.RequestValueComparers;

namespace NathanAlden.JuniorRouting.Core
{
	[DebuggerDisplay("{_url}")]
	public class HttpRouteUrl
	{
		public static readonly HttpRouteUrl Root = new HttpRouteUrl("", new CaseInsensitivePlainRequestValueComparer());
		private readonly IRequestValueComparer _comparer;
		private readonly string _url;

		public HttpRouteUrl(string url, IRequestValueComparer comparer)
		{
			url.ThrowIfNull("url");
			comparer.ThrowIfNull("comparer");

			_url = url;
			_comparer = comparer;
		}

		public string Url
		{
			get
			{
				return _url;
			}
		}

		public IRequestValueComparer Comparer
		{
			get
			{
				return _comparer;
			}
		}

		public bool Matches(string relativeRequestUrl)
		{
			return _comparer.Matches(_url, relativeRequestUrl);
		}
	}
}
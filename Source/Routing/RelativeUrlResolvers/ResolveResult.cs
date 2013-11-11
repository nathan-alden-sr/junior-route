using Junior.Common;

namespace Junior.Route.Routing.RelativeUrlResolvers
{
	public class ResolveResult
	{
		private readonly string _relativeUrl;
		private readonly ResolveResultType _resultType;

		private ResolveResult(ResolveResultType resultType, string relativeUrl)
		{
			_resultType = resultType;
			_relativeUrl = relativeUrl;
		}

		public ResolveResultType ResultType
		{
			get
			{
				return _resultType;
			}
		}

		public string RelativeUrl
		{
			get
			{
				return _relativeUrl;
			}
		}

		public static ResolveResult UrlResolved(string relativeUrl)
		{
			relativeUrl.ThrowIfNull("relativeUrl");

			return new ResolveResult(ResolveResultType.UrlResolved, relativeUrl);
		}

		public static ResolveResult UrlNotResolved()
		{
			return new ResolveResult(ResolveResultType.UrlNotResolved, null);
		}
	}
}
using Junior.Common;

namespace Junior.Route.AutoRouting.ResolvedRelativeUrlMappers
{
	public class ResolvedRelativeUrlResult
	{
		private readonly string _resolvedRelativeUrl;
		private readonly ResolvedRelativeUrlResultType _resultType;

		private ResolvedRelativeUrlResult(ResolvedRelativeUrlResultType resultType, string resolvedRelativeUrl)
		{
			_resultType = resultType;
			_resolvedRelativeUrl = resolvedRelativeUrl;
		}

		public ResolvedRelativeUrlResultType ResultType
		{
			get
			{
				return _resultType;
			}
		}

		public string ResolvedRelativeUrl
		{
			get
			{
				return _resolvedRelativeUrl;
			}
		}

		public static ResolvedRelativeUrlResult ResolvedRelativeUrlMapped(string resolvedRelativeUrl)
		{
			resolvedRelativeUrl.ThrowIfNull("resolvedRelativeUrl");

			return new ResolvedRelativeUrlResult(ResolvedRelativeUrlResultType.ResolvedRelativeUrlMapped, resolvedRelativeUrl);
		}

		public static ResolvedRelativeUrlResult ResolvedRelativeUrlNotMapped()
		{
			return new ResolvedRelativeUrlResult(ResolvedRelativeUrlResultType.ResolvedRelativeUrlNotMapped, null);
		}
	}
}
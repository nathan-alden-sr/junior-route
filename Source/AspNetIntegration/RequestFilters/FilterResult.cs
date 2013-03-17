namespace Junior.Route.AspNetIntegration.RequestFilters
{
	public class FilterResult
	{
		private readonly FilterResultType _resultType;

		private FilterResult(FilterResultType resultType)
		{
			_resultType = resultType;
		}

		public FilterResultType ResultType
		{
			get
			{
				return _resultType;
			}
		}

		public static FilterResult UseJuniorRouteHandler()
		{
			return new FilterResult(FilterResultType.UseJuniorRouteHandler);
		}

		public static FilterResult Defer()
		{
			return new FilterResult(FilterResultType.Defer);
		}
	}
}
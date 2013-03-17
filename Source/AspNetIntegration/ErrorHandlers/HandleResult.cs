namespace Junior.Route.AspNetIntegration.ErrorHandlers
{
	public class HandleResult
	{
		private readonly HandleResultType _resultType;

		private HandleResult(HandleResultType resultType)
		{
			_resultType = resultType;
		}

		public HandleResultType ResultType
		{
			get
			{
				return _resultType;
			}
		}

		public static HandleResult ErrorHandled()
		{
			return new HandleResult(HandleResultType.Handled);
		}

		public static HandleResult ErrorNotHandled()
		{
			return new HandleResult(HandleResultType.NotHandled);
		}
	}
}
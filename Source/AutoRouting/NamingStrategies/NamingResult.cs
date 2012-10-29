using Junior.Common;

namespace Junior.Route.AutoRouting.NamingStrategies
{
	public class NamingResult
	{
		private readonly string _name;
		private readonly NamingResultType _resultType;

		private NamingResult(NamingResultType resultType, string name)
		{
			_resultType = resultType;
			_name = name;
		}

		public NamingResultType ResultType
		{
			get
			{
				return _resultType;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public static NamingResult RouteNamed(string name)
		{
			name.ThrowIfNull("name");

			return new NamingResult(NamingResultType.RouteNamed, name);
		}

		public static NamingResult RouteNotNamed()
		{
			return new NamingResult(NamingResultType.RouteNotNamed, null);
		}
	}
}
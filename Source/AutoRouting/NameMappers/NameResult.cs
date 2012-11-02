using Junior.Common;

namespace Junior.Route.AutoRouting.NameMappers
{
	public class NameResult
	{
		private readonly string _name;
		private readonly NameResultType _resultType;

		private NameResult(NameResultType resultType, string name)
		{
			_resultType = resultType;
			_name = name;
		}

		public NameResultType ResultType
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

		public static NameResult NameMapped(string name)
		{
			name.ThrowIfNull("name");

			return new NameResult(NameResultType.NameMapped, name);
		}

		public static NameResult NameNotMapped()
		{
			return new NameResult(NameResultType.NameNotMapped, null);
		}
	}
}
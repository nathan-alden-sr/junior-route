using Junior.Route.Common;

namespace Junior.Route.AutoRouting.SchemeMappers
{
	public class SchemeResult
	{
		private readonly SchemeResultType _resultType;
		private readonly Scheme? _scheme;

		private SchemeResult(SchemeResultType resultType, Scheme? scheme = null)
		{
			_resultType = resultType;
			_scheme = scheme;
		}

		public SchemeResultType ResultType
		{
			get
			{
				return _resultType;
			}
		}

		public Scheme? Scheme
		{
			get
			{
				return _scheme;
			}
		}

		public static SchemeResult SchemeMapped(Scheme scheme)
		{
			return new SchemeResult(SchemeResultType.SchemeMapped, scheme);
		}

		public static SchemeResult SchemeNotMapped()
		{
			return new SchemeResult(SchemeResultType.SchemeNotMapped);
		}
	}
}
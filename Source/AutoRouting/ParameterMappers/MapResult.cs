namespace Junior.Route.AutoRouting.ParameterMappers
{
	public class MapResult
	{
		private readonly MapResultType _resultType;
		private readonly object _value;

		private MapResult(MapResultType resultType, object value = null)
		{
			_resultType = resultType;
			_value = value;
		}

		public MapResultType ResultType
		{
			get
			{
				return _resultType;
			}
		}

		public object Value
		{
			get
			{
				return _value;
			}
		}

		public static MapResult ValueMapped(object value)
		{
			return new MapResult(MapResultType.ValueMapped, value);
		}

		public static MapResult ValueNotMapped()
		{
			return new MapResult(MapResultType.ValueNotMapped);
		}
	}
}
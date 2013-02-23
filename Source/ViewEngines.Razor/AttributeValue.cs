using System;

namespace Junior.Route.ViewEngines.Razor
{
	public class AttributeValue
	{
		private readonly bool _literal;
		private readonly PositionTagged<string> _prefix;
		private readonly PositionTagged<object> _value;

		public AttributeValue(PositionTagged<string> prefix, PositionTagged<object> value, bool literal)
		{
			_prefix = prefix;
			_value = value;
			_literal = literal;
		}

		public PositionTagged<string> Prefix
		{
			get
			{
				return _prefix;
			}
		}

		public PositionTagged<object> Value
		{
			get
			{
				return _value;
			}
		}

		public bool Literal
		{
			get
			{
				return _literal;
			}
		}

		public static AttributeValue FromTuple(Tuple<Tuple<string, int>, Tuple<object, int>, bool> value)
		{
			return new AttributeValue(value.Item1, value.Item2, value.Item3);
		}

		public static AttributeValue FromTuple(Tuple<Tuple<string, int>, Tuple<string, int>, bool> value)
		{
			return new AttributeValue(value.Item1, new PositionTagged<object>(value.Item2.Item1, value.Item2.Item2), value.Item3);
		}

		public static implicit operator AttributeValue(Tuple<Tuple<string, int>, Tuple<object, int>, bool> value)
		{
			return FromTuple(value);
		}

		public static implicit operator AttributeValue(Tuple<Tuple<string, int>, Tuple<string, int>, bool> value)
		{
			return FromTuple(value);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Junior.Route.ViewEngines.Razor
{
	[DebuggerDisplay("({Position})\"{Value}\"")]
	public class PositionTagged<T>
	{
		private readonly int _position;
		private readonly T _value;

		public PositionTagged(T value, int offset)
		{
			_position = offset;
			_value = value;
		}

		public int Position
		{
			get
			{
				return _position;
			}
		}

		public T Value
		{
			get
			{
				return _value;
			}
		}

		protected bool Equals(PositionTagged<T> other)
		{
			return Position == other.Position && EqualityComparer<T>.Default.Equals(Value, other.Value);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((PositionTagged<T>)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Position * 397) ^ EqualityComparer<T>.Default.GetHashCode(Value);
			}
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public static implicit operator T(PositionTagged<T> value)
		{
			return value.Value;
		}

		public static implicit operator PositionTagged<T>(Tuple<T, int> value)
		{
			return new PositionTagged<T>(value.Item1, value.Item2);
		}

		public static bool operator ==(PositionTagged<T> left, PositionTagged<T> right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(PositionTagged<T> left, PositionTagged<T> right)
		{
			return !Equals(left, right);
		}
	}
}
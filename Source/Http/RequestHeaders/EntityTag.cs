using System;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class EntityTag : IEquatable<EntityTag>
	{
		private readonly string _value;
		private readonly bool _weak;

		public EntityTag(string value, bool weak)
		{
			value.ThrowIfNull("value");

			_value = value;
			_weak = weak;
		}

		public string Value
		{
			get
			{
				return _value;
			}
		}

		public bool Weak
		{
			get
			{
				return _weak;
			}
		}

		public bool Equals(EntityTag other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return String.Equals(_value, other._value) && _weak.Equals(other._weak);
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
			return Equals((EntityTag)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((_value != null ? _value.GetHashCode() : 0) * 397) ^ _weak.GetHashCode();
			}
		}

		public static EntityTag Parse(string entityTag)
		{
			if (Regex.IsMatch(entityTag, CommonRegexPatterns.EntityTag))
			{
				bool weak = entityTag.StartsWith("W/");

				return new EntityTag(entityTag.Substring(weak ? 2 : 0).RemoveOptionalQuotes(), weak);
			}

			// Some browsers and user-agents violate the HTTP specification and send unquoted entity tags
			return new EntityTag(entityTag.RemoveOptionalQuotes(), false);
		}
	}
}
using System;

using Junior.Common;

namespace Junior.Route.AutoRouting.ClassFilters
{
	public class NameEndsWithFilter : IClassFilter
	{
		private readonly StringComparison _comparison;
		private readonly string _value;

		public NameEndsWithFilter(string value, StringComparison comparison = StringComparison.Ordinal)
		{
			value.ThrowIfNull("value");

			_value = value;
			_comparison = comparison;
		}

		public bool Matches(Type type)
		{
			type.ThrowIfNull("type");

			return type.Name.EndsWith(_value, _comparison);
		}
	}
}
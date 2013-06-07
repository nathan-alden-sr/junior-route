using System;
using System.Threading.Tasks;

using Junior.Common;

namespace Junior.Route.AutoRouting.ClassFilters
{
	public class NameStartsWithFilter : IClassFilter
	{
		private readonly StringComparison _comparison;
		private readonly string _value;

		public NameStartsWithFilter(string value, StringComparison comparison = StringComparison.Ordinal)
		{
			value.ThrowIfNull("value");

			_value = value;
			_comparison = comparison;
		}

		public Task<bool> MatchesAsync(Type type)
		{
			type.ThrowIfNull("type");

			return type.Name.StartsWith(_value, _comparison).AsCompletedTask();
		}
	}
}
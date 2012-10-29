using System.Collections.Generic;

using Junior.Common;

namespace Junior.Route.Common
{
	public static class HashSetExtensions
	{
		public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> values)
		{
			hashSet.ThrowIfNull("hashSet");
			values.ThrowIfNull("values");

			foreach (T value in values)
			{
				hashSet.Add(value);
			}
		}
	}
}
using System.Collections.Generic;

using Junior.Route.Common;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Common
{
	public static class HashSetExtensionsTester
	{
		[TestFixture]
		public class When_adding_range_of_items
		{
			[SetUp]
			public void SetUp()
			{
				_hashSet = new HashSet<int>();
				_hashSet.AddRange((IEnumerable<int>)new[] { 1, 10 });
				_hashSet.AddRange(0, 0, 1, 1, 2, 10);
			}

			private HashSet<int> _hashSet;

			[Test]
			public void Must_add_items()
			{
				Assert.That(_hashSet, Is.EquivalentTo(new[] { 0, 1, 2, 10 }));
			}
		}
	}
}
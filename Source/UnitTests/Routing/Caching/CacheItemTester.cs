using System;

using Junior.Route.Routing.Caching;
using Junior.Route.Routing.Responses;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Routing.Caching
{
	public static class CacheItemTester
	{
		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_cacheResponse = new CacheResponse(Response.OK());
				_cachedUtcTimestamp = new DateTime(2012, 01, 01, 0, 0, 0, DateTimeKind.Utc);
				_expiresUtcTimestamp = new DateTime(2013, 01, 01, 0, 0, 0, DateTimeKind.Utc);
				_cacheItem = new CacheItem(_cacheResponse, _cachedUtcTimestamp, _expiresUtcTimestamp);
			}

			private CacheResponse _cacheResponse;
			private CacheItem _cacheItem;
			private DateTime _cachedUtcTimestamp;
			private DateTime _expiresUtcTimestamp;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_cacheItem.CachedUtcTimestamp, Is.EqualTo(_cachedUtcTimestamp));
				Assert.That(_cacheItem.ExpiresUtcTimestamp, Is.EqualTo(_expiresUtcTimestamp));
				Assert.That(_cacheItem.Response, Is.SameAs(_cacheResponse));
			}
		}

		[TestFixture]
		public class When_creating_instance_with_non_utc_datetimes
		{
			[Test]
			public void Must_throw_exception()
			{
				var cacheResponse = new CacheResponse(Response.OK());

				Assert.Throws<ArgumentException>(() => new CacheItem(cacheResponse, new DateTime(2012, 01, 01, 0, 0, 0, DateTimeKind.Local), new DateTime(2012, 01, 01, 0, 0, 0, DateTimeKind.Utc)));
				Assert.Throws<ArgumentException>(() => new CacheItem(cacheResponse, new DateTime(2012, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2012, 01, 01, 0, 0, 0, DateTimeKind.Local)));
			}
		}
	}
}
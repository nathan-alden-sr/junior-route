using System;
using System.Collections.Generic;
using System.Linq;

using Junior.Common;
using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class CacheControlHeaderTester
	{
		[TestFixture]
		public class When_parsing_invalid_accept_cache_control_header
		{
			[Test]
			[TestCase("no-cache, no-store, X=, Y=")]
			[TestCase(@"no-cache, param=""1")]
			[TestCase(@"no-cache, param=1""")]
			public void Must_not_result_in_header(string headerValue)
			{
				Assert.That(CacheControlHeader.Parse(headerValue), Is.Null);
			}
		}

		[TestFixture]
		public class When_parsing_valid_accept_cache_control_header
		{
			[Test]
			[TestCase(@"param1=""1"", param2=""2""", false, false, null, null, null, false, false, "param1", "1", "param2", "2")]
			[TestCase("no-cache, no-store, max-age=3600", true, true, 3600, null, null, false, false)]
			[TestCase("no-cache, no-store, max-age=3600, max-stale=0, min-fresh=100, no-transform, only-if-cached, param=1", true, true, 3600, 0, 100, true, true, "param", "1")]
			[TestCase(@"no-cache, no-store, max-age=3600, max-stale=0, min-fresh=100, no-transform, only-if-cached, param=""1""", true, true, 3600, 0, 100, true, true, "param", "1")]
			[TestCase(@"no-cache, no-store, max-age=3600, max-stale, min-fresh=100, no-transform, only-if-cached, param=""1""", true, true, 3600, null, 100, true, true, "param", "1")]
			[TestCase("no-cache,\r\n no-store,\r\n max-age=3600,\r\n max-stale,\r\n min-fresh=100,\r\n no-transform,\r\n only-if-cached,\r\n param=\"1\"", true, true, 3600, null, 100, true, true, "param", "1")]
			public void Must_parse_correctly(object[] parameters)
			{
				var headerValue = (string)parameters[0];
				var noCache = (bool)parameters[1];
				var noStore = (bool)parameters[2];
				var maxAge = (int?)parameters[3];
				var maxStale = (int?)parameters[4];
				var minFresh = (int?)parameters[5];
				var noTransform = (bool)parameters[6];
				var onlyIfCached = (bool)parameters[7];
				var cacheExtensions = new List<Tuple<string, string>>();

				for (int i = 8; i < parameters.Length; i += 2)
				{
					cacheExtensions.Add(new Tuple<string, string>((string)parameters[i], (string)parameters[i + 1]));
				}

				CacheControlHeader header = CacheControlHeader.Parse(headerValue);

				Assert.That(header, Is.Not.Null);
				Assert.That(header.NoCache, Is.EqualTo(noCache));
				Assert.That(header.NoStore, Is.EqualTo(noStore));
				Assert.That(header.MaxAge, Is.EqualTo(maxAge.IfNotNull(arg => (TimeSpan?)TimeSpan.FromSeconds(arg))));
				Assert.That(header.MaxAgeSeconds, Is.EqualTo(maxAge));
				Assert.That(header.MaxStale, Is.EqualTo(maxStale.IfNotNull(arg => (TimeSpan?)TimeSpan.FromSeconds(arg))));
				Assert.That(header.MaxStaleSeconds, Is.EqualTo(maxStale));
				Assert.That(header.MinFresh, Is.EqualTo(minFresh.IfNotNull(arg => (TimeSpan?)TimeSpan.FromSeconds(arg))));
				Assert.That(header.MinFreshSeconds, Is.EqualTo(minFresh));
				Assert.That(header.NoTransform, Is.EqualTo(noTransform));
				Assert.That(header.OnlyIfCached, Is.EqualTo(onlyIfCached));
				Assert.That(header.CacheExtensions.Select(arg => new Tuple<string, string>(arg.Name, arg.Value)), Is.EquivalentTo(cacheExtensions));
			}
		}
	}
}
using System;
using System.Web;

using Junior.Route.Routing.Caching;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Routing.Caching
{
	public static class CachePolicyTester
	{
		[TestFixture]
		public class When_applying_policy
		{
			[SetUp]
			public void SetUp()
			{
				_httpCachePolicyBase = MockRepository.GenerateMock<HttpCachePolicyBase>();

				_cachePolicy = new CachePolicy();
				_cachePolicy.AllowResponseInBrowserHistory(true);
				_cachePolicy.ETag("etag");
				_cachePolicy.NoStore();
				_cachePolicy.NoTransforms();
				_cachePolicy.OmitVaryStar(true);
				_cachePolicy.ProxyMaxAge(TimeSpan.FromHours(1));
				_cachePolicy.Revalidation(HttpCacheRevalidation.AllCaches);
			}

			private CachePolicy _cachePolicy;
			private HttpCachePolicyBase _httpCachePolicyBase;

			[Test]
			public void Must_apply_no_client_and_server_cacheability()
			{
				_cachePolicy.NoClientCaching();
				_cachePolicy.ServerCaching(DateTime.UtcNow);

				_cachePolicy.Apply(_httpCachePolicyBase);

				_httpCachePolicyBase.AssertWasCalled(arg => arg.SetCacheability(HttpCacheability.ServerAndNoCache));
			}

			[Test]
			public void Must_apply_no_client_cacheability()
			{
				_cachePolicy.NoClientCaching();

				_cachePolicy.Apply(_httpCachePolicyBase);

				_httpCachePolicyBase.AssertWasCalled(arg => arg.SetCacheability(HttpCacheability.NoCache));
			}

			[Test]
			public void Must_apply_private_client_and_server_cacheability()
			{
				_cachePolicy.PrivateClientCaching(TimeSpan.FromDays(1));
				_cachePolicy.ServerCaching(DateTime.UtcNow);

				_cachePolicy.Apply(_httpCachePolicyBase);

				_httpCachePolicyBase.AssertWasCalled(arg => arg.SetCacheability(HttpCacheability.ServerAndPrivate));
			}

			[Test]
			public void Must_apply_private_client_cacheability()
			{
				_cachePolicy.PrivateClientCaching(TimeSpan.FromDays(1));

				_cachePolicy.Apply(_httpCachePolicyBase);

				_httpCachePolicyBase.AssertWasCalled(arg => arg.SetCacheability(HttpCacheability.Private));
			}

			[Test]
			public void Must_apply_provided_settings()
			{
				_cachePolicy.Apply(_httpCachePolicyBase);

				_httpCachePolicyBase.AssertWasCalled(arg => arg.SetAllowResponseInBrowserHistory(true));
				_httpCachePolicyBase.AssertWasCalled(arg => arg.SetETag("etag"));
				_httpCachePolicyBase.AssertWasCalled(arg => arg.SetNoStore());
				_httpCachePolicyBase.AssertWasCalled(arg => arg.SetNoTransforms());
				_httpCachePolicyBase.AssertWasCalled(arg => arg.SetOmitVaryStar(true));
				_httpCachePolicyBase.AssertWasCalled(arg => arg.SetProxyMaxAge(TimeSpan.FromHours(1)));
				_httpCachePolicyBase.AssertWasCalled(arg => arg.SetRevalidation(HttpCacheRevalidation.AllCaches));
			}

			[Test]
			public void Must_apply_public_client_and_server_cacheability()
			{
				_cachePolicy.PublicClientCaching(TimeSpan.FromDays(1));
				_cachePolicy.ServerCaching(DateTime.UtcNow);

				_cachePolicy.Apply(_httpCachePolicyBase);

				_httpCachePolicyBase.AssertWasCalled(arg => arg.SetCacheability(HttpCacheability.Public));
			}

			[Test]
			public void Must_apply_public_client_cacheability()
			{
				_cachePolicy.PublicClientCaching(TimeSpan.FromDays(1));

				_cachePolicy.Apply(_httpCachePolicyBase);

				_httpCachePolicyBase.AssertWasCalled(arg => arg.SetCacheability(HttpCacheability.Public));
			}
		}

		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_cachePolicy = new CachePolicy();
			}

			private ICachePolicy _cachePolicy;

			[Test]
			public void Must_create_instance_with_no_policy()
			{
				Assert.That(_cachePolicy.HasPolicy, Is.False);
			}
		}

		[TestFixture]
		public class When_resetting_instance
		{
			[SetUp]
			public void SetUp()
			{
				_cachePolicy = new CachePolicy();
				_cachePolicy.ServerCaching(DateTime.UtcNow);
				_cachePolicy.Reset();
			}

			private CachePolicy _cachePolicy;

			[Test]
			public void Must_not_have_policy()
			{
				Assert.That(((ICachePolicy)_cachePolicy).HasPolicy, Is.False);
			}
		}

		[TestFixture]
		public class When_setting_allow_response_in_browser_history
		{
			[SetUp]
			public void SetUp()
			{
				_cachePolicy = new CachePolicy();
				_cachePolicy.AllowResponseInBrowserHistory(true);
			}

			private CachePolicy _cachePolicy;

			[Test]
			public void Must_have_policy()
			{
				Assert.That(((ICachePolicy)_cachePolicy).HasPolicy, Is.True);
			}
		}

		[TestFixture]
		public class When_setting_etag
		{
			[SetUp]
			public void SetUp()
			{
				_cachePolicy = new CachePolicy();
				_cachePolicy.ETag("etag");
			}

			private CachePolicy _cachePolicy;

			[Test]
			public void Must_have_policy()
			{
				Assert.That(((ICachePolicy)_cachePolicy).HasPolicy, Is.True);
			}

			[Test]
			public void Must_set_etag()
			{
				Assert.That(((ICachePolicy)_cachePolicy).ETag, Is.EqualTo("etag"));
			}
		}

		[TestFixture]
		public class When_setting_no_server_caching
		{
			[SetUp]
			public void SetUp()
			{
				_cachePolicy = new CachePolicy();
				_cachePolicy.NoServerCaching();
			}

			private CachePolicy _cachePolicy;

			[Test]
			public void Must_have_policy()
			{
				Assert.That(((ICachePolicy)_cachePolicy).HasPolicy, Is.True);
			}

			[Test]
			public void Must_not_allow_server_caching()
			{
				Assert.That(((ICachePolicy)_cachePolicy).AllowsServerCaching, Is.False);
			}
		}

		[TestFixture]
		public class When_setting_no_store
		{
			[SetUp]
			public void SetUp()
			{
				_cachePolicy = new CachePolicy();
				_cachePolicy.NoStore();
			}

			private CachePolicy _cachePolicy;

			[Test]
			public void Must_have_policy()
			{
				Assert.That(((ICachePolicy)_cachePolicy).HasPolicy, Is.True);
			}
		}

		[TestFixture]
		public class When_setting_no_transforms
		{
			[SetUp]
			public void SetUp()
			{
				_cachePolicy = new CachePolicy();
				_cachePolicy.NoTransforms();
			}

			private CachePolicy _cachePolicy;

			[Test]
			public void Must_have_policy()
			{
				Assert.That(((ICachePolicy)_cachePolicy).HasPolicy, Is.True);
			}
		}

		[TestFixture]
		public class When_setting_omit_vary_star
		{
			[SetUp]
			public void SetUp()
			{
				_cachePolicy = new CachePolicy();
				_cachePolicy.OmitVaryStar(true);
			}

			private CachePolicy _cachePolicy;

			[Test]
			public void Must_have_policy()
			{
				Assert.That(((ICachePolicy)_cachePolicy).HasPolicy, Is.True);
			}
		}

		[TestFixture]
		public class When_setting_private_client_caching_with_expires_timestamp
		{
			[SetUp]
			public void SetUp()
			{
				_cachePolicy = new CachePolicy();
			}

			private CachePolicy _cachePolicy;

			[Test]
			public void Must_have_policy()
			{
				_cachePolicy.PrivateClientCaching(new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc));

				Assert.That(((ICachePolicy)_cachePolicy).HasPolicy, Is.True);
			}

			[Test]
			public void Must_require_utc()
			{
				Assert.Throws<ArgumentException>(() => _cachePolicy.PrivateClientCaching(new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Local)));
			}

			[Test]
			public void Must_set_expires()
			{
				var expires = new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc);

				_cachePolicy.PrivateClientCaching(expires);

				var cachePolicy = (ICachePolicy)_cachePolicy;

				Assert.That(cachePolicy.ClientCacheExpirationUtcTimestamp, Is.EqualTo(expires));
			}
		}

		[TestFixture]
		public class When_setting_private_client_caching_with_max_age
		{
			[SetUp]
			public void SetUp()
			{
				_cachePolicy = new CachePolicy();
			}

			private CachePolicy _cachePolicy;

			[Test]
			public void Must_have_policy()
			{
				_cachePolicy.PrivateClientCaching(TimeSpan.FromDays(1));

				Assert.That(((ICachePolicy)_cachePolicy).HasPolicy, Is.True);
			}

			[Test]
			public void Must_set_max_age()
			{
				TimeSpan maxAge = TimeSpan.FromDays(1);

				_cachePolicy.PrivateClientCaching(maxAge);

				var cachePolicy = (ICachePolicy)_cachePolicy;

				Assert.That(cachePolicy.ClientCacheMaxAge, Is.EqualTo(maxAge));
			}
		}

		[TestFixture]
		public class When_setting_proxy_max_age
		{
			[SetUp]
			public void SetUp()
			{
				_cachePolicy = new CachePolicy();
				_cachePolicy.ProxyMaxAge(TimeSpan.FromDays(1));
			}

			private CachePolicy _cachePolicy;

			[Test]
			public void Must_have_policy()
			{
				Assert.That(((ICachePolicy)_cachePolicy).HasPolicy, Is.True);
			}
		}

		[TestFixture]
		public class When_setting_public_client_caching_with_expires_timestamp
		{
			[SetUp]
			public void SetUp()
			{
				_cachePolicy = new CachePolicy();
			}

			private CachePolicy _cachePolicy;

			[Test]
			public void Must_have_policy()
			{
				_cachePolicy.PublicClientCaching(new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc));

				Assert.That(((ICachePolicy)_cachePolicy).HasPolicy, Is.True);
			}

			[Test]
			public void Must_require_utc()
			{
				Assert.Throws<ArgumentException>(() => _cachePolicy.PublicClientCaching(new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Local)));
			}

			[Test]
			public void Must_set_expires()
			{
				var expires = new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc);

				_cachePolicy.PublicClientCaching(expires);

				var cachePolicy = (ICachePolicy)_cachePolicy;

				Assert.That(cachePolicy.ClientCacheExpirationUtcTimestamp, Is.EqualTo(expires));
			}
		}

		[TestFixture]
		public class When_setting_public_client_caching_with_max_age
		{
			[SetUp]
			public void SetUp()
			{
				_cachePolicy = new CachePolicy();
			}

			private CachePolicy _cachePolicy;

			[Test]
			public void Must_have_policy()
			{
				_cachePolicy.PublicClientCaching(TimeSpan.FromDays(1));

				Assert.That(((ICachePolicy)_cachePolicy).HasPolicy, Is.True);
			}

			[Test]
			public void Must_set_max_age()
			{
				TimeSpan maxAge = TimeSpan.FromDays(1);

				_cachePolicy.PublicClientCaching(maxAge);

				var cachePolicy = (ICachePolicy)_cachePolicy;

				Assert.That(cachePolicy.ClientCacheMaxAge, Is.EqualTo(maxAge));
			}
		}

		[TestFixture]
		public class When_setting_revalidation
		{
			[SetUp]
			public void SetUp()
			{
				_cachePolicy = new CachePolicy();
				_cachePolicy.Revalidation(HttpCacheRevalidation.AllCaches);
			}

			private CachePolicy _cachePolicy;

			[Test]
			public void Must_have_policy()
			{
				Assert.That(((ICachePolicy)_cachePolicy).HasPolicy, Is.True);
			}
		}

		[TestFixture]
		public class When_setting_server_caching
		{
			[SetUp]
			public void SetUp()
			{
				_cachePolicy = new CachePolicy();
				_cachePolicy.ServerCaching(DateTime.UtcNow);
			}

			private CachePolicy _cachePolicy;

			[Test]
			public void Must_allow_server_caching()
			{
				Assert.That(((ICachePolicy)_cachePolicy).AllowsServerCaching, Is.True);
			}

			[Test]
			public void Must_have_policy()
			{
				Assert.That(((ICachePolicy)_cachePolicy).HasPolicy, Is.True);
			}
		}
	}
}
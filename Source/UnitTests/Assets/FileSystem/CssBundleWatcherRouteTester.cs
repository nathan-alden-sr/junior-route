using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

using Junior.Common;
using Junior.Route.Assets.FileSystem;
using Junior.Route.Common;
using Junior.Route.Routing;
using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Responses;
using Junior.Route.Routing.Restrictions;

using NUnit.Framework;

using Rhino.Mocks;

namespace Junior.Route.UnitTests.Assets.FileSystem
{
	public static class CssBundleWatcherRouteTester
	{
		[TestFixture]
		public class When_changing_bundle
		{
			[SetUp]
			public void SetUp()
			{
				_path = Path.GetTempFileName();
				File.WriteAllText(_path, ".css { text-align: right; }");
				_bundle = new Bundle().File("file1");
				_fileSystem = MockRepository.GenerateMock<IFileSystem>();
				_fileSystem.Stub(arg => arg.AbsolutePath("file1")).Return(_path);
				_fileSystem
					.Stub(arg => arg.OpenFile(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					.WhenCalled(arg => arg.ReturnValue = File.OpenRead(_path))
					.Return(null);
				_concatenator = MockRepository.GenerateMock<IAssetConcatenator>();
				_concatenator
					.Stub(arg => arg.Concatenate(Arg<IEnumerable<string>>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = ((IEnumerable<string>)arg.Arguments.First()).First())
					.Return(null);
				_transformer = MockRepository.GenerateMock<IAssetTransformer>();
				_transformer
					.Stub(arg => arg.Transform(Arg<string>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = arg.Arguments.First())
					.Return(null);
				_watcher = new BundleWatcher(_bundle, _fileSystem, _concatenator, _transformer);
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_systemClock.Stub(arg => arg.UtcDateTime).Return(new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc));
				_routeId = Guid.NewGuid();
				_cssBundleWatcherRoute = new CssBundleWatcherRoute("route", _routeId, "relative", _watcher, _httpRuntime, _systemClock);
			}

			[TearDown]
			public void TearDown()
			{
				File.Delete(_path);
			}

			private Bundle _bundle;
			private BundleWatcher _watcher;
			private IFileSystem _fileSystem;
			private IAssetConcatenator _concatenator;
			private IAssetTransformer _transformer;
			private string _path;
			private IHttpRuntime _httpRuntime;
			private ISystemClock _systemClock;
			private Guid _routeId;
			private CssBundleWatcherRoute _cssBundleWatcherRoute;

			[Test]
			public void Must_reset_route_resolved_relative_url_hash_in_query_string()
			{
				Assert.That(_cssBundleWatcherRoute.ResolvedRelativeUrl, Is.StringEnding("00bddee8b5e0f219a85f96858c83c7b3"));

				File.WriteAllText(_path, ".new-css { text-align: right; }");

				Thread.Sleep(TimeSpan.FromSeconds(2));

				Assert.That(_cssBundleWatcherRoute.ResolvedRelativeUrl, Is.Not.StringEnding("00bddee8b5e0f219a85f96858c83c7b3"));
			}
		}

		[TestFixture]
		public class When_retrieving_css_bundle_response
		{
			[SetUp]
			public void SetUp()
			{
				_path = Path.GetTempFileName();
				File.WriteAllText(_path, ".css { text-align: right; }");
				_bundle = new Bundle().File("file1");
				_fileSystem = MockRepository.GenerateMock<IFileSystem>();
				_fileSystem.Stub(arg => arg.AbsolutePath("file1")).Return(_path);
				_fileSystem
					.Stub(arg => arg.OpenFile(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					.WhenCalled(arg => arg.ReturnValue = File.OpenRead(_path))
					.Return(null);
				_concatenator = MockRepository.GenerateMock<IAssetConcatenator>();
				_concatenator
					.Stub(arg => arg.Concatenate(Arg<IEnumerable<string>>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = ((IEnumerable<string>)arg.Arguments.First()).First())
					.Return(null);
				_transformer = MockRepository.GenerateMock<IAssetTransformer>();
				_transformer
					.Stub(arg => arg.Transform(Arg<string>.Is.Anything))
					.WhenCalled(arg => arg.ReturnValue = arg.Arguments.First())
					.Return(null);
				_watcher = new BundleWatcher(_bundle, _fileSystem, _concatenator, _transformer);
				_httpRuntime = MockRepository.GenerateMock<IHttpRuntime>();
				_systemClock = MockRepository.GenerateMock<ISystemClock>();
				_systemClock.Stub(arg => arg.UtcDateTime).Return(new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc));
				_context = MockRepository.GenerateMock<HttpContextBase>();
				_routeId = Guid.NewGuid();
				_watcherRoute = new CssBundleWatcherRoute("route", _routeId, "relative", _watcher, _httpRuntime, _systemClock);
				_response = _watcherRoute.ProcessResponseAsync(_context).Result;
			}

			[TearDown]
			public void TearDown()
			{
				File.Delete(_path);
			}

			private Bundle _bundle;
			private BundleWatcher _watcher;
			private IFileSystem _fileSystem;
			private IAssetConcatenator _concatenator;
			private IAssetTransformer _transformer;
			private string _path;
			private CssBundleWatcherRoute _watcherRoute;
			private IHttpRuntime _httpRuntime;
			private ISystemClock _systemClock;
			private HttpContextBase _context;
			private IResponse _response;
			private Guid _routeId;

			[Test]
			public void Must_restrict_relative_url()
			{
				UrlRelativePathRestriction[] restrictions = _watcherRoute.GetRestrictions<UrlRelativePathRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));

				Assert.That(restrictions[0].RelativePath, Is.EqualTo("relative"));
			}

			[Test]
			public void Must_restrict_to_get_method()
			{
				MethodRestriction[] restrictions = _watcherRoute.GetRestrictions<MethodRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));

				Assert.That(restrictions[0].Method, Is.EqualTo("GET"));
			}

			[Test]
			public void Must_restrict_url_query()
			{
				UrlQueryRestriction[] restrictions = _watcherRoute.GetRestrictions<UrlQueryRestriction>().ToArray();

				Assert.That(restrictions, Has.Length.EqualTo(1));

				Assert.That(restrictions[0].Comparer, Is.SameAs(CaseInsensitiveRegexComparer.Instance));
				Assert.That(restrictions[0].Query, Is.EqualTo(@"^(?:\?(?i:v=[a-f0-9]{32}))?$"));
			}

			[Test]
			public void Must_set_correct_response_properties()
			{
				var httpCachePolicyBase = MockRepository.GenerateMock<HttpCachePolicyBase>();

				_response.CachePolicy.Apply(httpCachePolicyBase);

				httpCachePolicyBase.AssertWasCalled(arg => arg.SetCacheability(HttpCacheability.Public));

				Assert.That(_response.CachePolicy.AllowsServerCaching, Is.True);
				Assert.That(_response.CachePolicy.ETag, Is.EqualTo(_routeId.ToString("N").ToLowerInvariant()));
				Assert.That(_response.CachePolicy.ClientCacheExpirationUtcTimestamp, Is.EqualTo(new DateTime(2013, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
				Assert.That(_response.ContentEncoding, Is.SameAs(Encoding.UTF8));
				Assert.That(_response.ContentType, Is.EqualTo("text/css"));
				Assert.That(_response.StatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.OK));
			}

			[Test]
			public void Must_set_resolved_relative_url_with_bundlewatcher_hash_in_query_string()
			{
				Assert.That(_watcherRoute.ResolvedRelativeUrl, Is.EqualTo("relative?v=" + _watcher.Hash));
			}
		}
	}
}
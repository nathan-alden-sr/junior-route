using System;
using System.Linq;
using System.Web;

using Junior.Route.Routing.Responses;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Routing.Responses
{
	public static class CookieTester
	{
		[TestFixture]
		public class When_cloning_instance_with_multiple_values
		{
			[SetUp]
			public void SetUp()
			{
				var httpCookie = new HttpCookie("name")
					{
						Domain = "domain",
						Expires = new DateTime(2012, 1, 2, 3, 4, 5),
						HttpOnly = true,
						Path = "/",
						Secure = true,
						Values =
							{
								{ "name1", "value1" },
								{ "name2", "value2" }
							}
					};
				_cookie1 = new Cookie(httpCookie, true);
				_cookie2 = _cookie1.Clone();
			}

			private Cookie _cookie1;
			private Cookie _cookie2;

			[Test]
			public void Must_create_equal_instance()
			{
				Assert.That(_cookie1.Domain, Is.EqualTo(_cookie2.Domain));
				Assert.That(_cookie1.Expires, Is.EqualTo(_cookie2.Expires));
				Assert.That(_cookie1.HttpOnly, Is.EqualTo(_cookie2.HttpOnly));
				Assert.That(_cookie1.Path, Is.EqualTo(_cookie2.Path));
				Assert.That(_cookie1.Secure, Is.EqualTo(_cookie2.Secure));
				Assert.That(_cookie1.Shareable, Is.EqualTo(_cookie2.Shareable));
				Assert.That(_cookie1.Value, Is.EqualTo(_cookie2.Value));
				Assert.That(_cookie1.Values.Select(arg => new Tuple<string, string>(arg.Name, arg.Value)), Is.EquivalentTo(_cookie2.Values.Select(arg => new Tuple<string, string>(arg.Name, arg.Value))));
			}
		}

		[TestFixture]
		public class When_cloning_instance_with_single_value
		{
			[SetUp]
			public void SetUp()
			{
				var httpCookie = new HttpCookie("name", "value");

				_cookie1 = new Cookie(httpCookie, true);
				_cookie2 = _cookie1.Clone();
			}

			private Cookie _cookie1;
			private Cookie _cookie2;

			[Test]
			public void Must_create_equal_instance()
			{
				Assert.That(_cookie1.Value, Is.EqualTo(_cookie2.Value));
			}
		}

		[TestFixture]
		public class When_creating_instance_with_httpcookie
		{
			[SetUp]
			public void SetUp()
			{
				_httpCookie = new HttpCookie("name")
					{
						Domain = "domain",
						Expires = new DateTime(2012, 1, 2, 3, 4, 5),
						HttpOnly = true,
						Path = "/",
						Secure = true,
						Values =
							{
								{ "name1", "value1" },
								{ "name2", "value2" }
							}
					};
				_cookie = new Cookie(_httpCookie, true);
			}

			private HttpCookie _httpCookie;
			private Cookie _cookie;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_cookie.Domain, Is.EqualTo(_httpCookie.Domain));
				Assert.That(_cookie.Expires, Is.EqualTo(_httpCookie.Expires));
				Assert.That(_cookie.HttpOnly, Is.EqualTo(_httpCookie.HttpOnly));
				Assert.That(_cookie.Path, Is.EqualTo(_httpCookie.Path));
				Assert.That(_cookie.Secure, Is.EqualTo(_httpCookie.Secure));
				Assert.That(_cookie.Shareable, Is.EqualTo(true));
				Assert.That(_cookie.Value, Is.EqualTo(_httpCookie.Value));
				Assert.That(_cookie.Values.Select(arg => new Tuple<string, string>(arg.Name, arg.Value)), Is.EquivalentTo(_httpCookie.Values.AllKeys.Select(arg => new Tuple<string, string>(arg, _httpCookie.Values[arg]))));
			}
		}

		[TestFixture]
		public class When_getting_httpcookie_with_multiple_values
		{
			[SetUp]
			public void SetUp()
			{
				var httpCookie = new HttpCookie("name")
					{
						Domain = "domain",
						Expires = new DateTime(2012, 1, 2, 3, 4, 5),
						HttpOnly = true,
						Path = "/",
						Secure = true,
						Values =
							{
								{ "name1", "value1" },
								{ "name2", "value2" }
							}
					};

				_cookie = new Cookie(httpCookie, true);
				_httpCookie = _cookie.GetHttpCookie();
			}

			private HttpCookie _httpCookie;
			private Cookie _cookie;

			[Test]
			public void Must_create_equal_instance()
			{
				Assert.That(_httpCookie.Domain, Is.EqualTo(_cookie.Domain));
				Assert.That(_httpCookie.Expires, Is.EqualTo(_cookie.Expires));
				Assert.That(_httpCookie.HttpOnly, Is.EqualTo(_cookie.HttpOnly));
				Assert.That(_httpCookie.Path, Is.EqualTo(_cookie.Path));
				Assert.That(_httpCookie.Secure, Is.EqualTo(_cookie.Secure));
				Assert.That(_httpCookie.Value, Is.EqualTo(_cookie.Value));
				Assert.That(_httpCookie.Values.AllKeys.Select(arg => new Tuple<string, string>(arg, _httpCookie.Values[arg])), Is.EquivalentTo(_cookie.Values.Select(arg => new Tuple<string, string>(arg.Name, arg.Value))));
			}
		}

		[TestFixture]
		public class When_getting_httpcookie_with_single_value
		{
			[SetUp]
			public void SetUp()
			{
				var httpCookie = new HttpCookie("name", "value");

				_cookie = new Cookie(httpCookie, true);
				_httpCookie = _cookie.GetHttpCookie();
			}

			private HttpCookie _httpCookie;
			private Cookie _cookie;

			[Test]
			public void Must_create_equal_instance()
			{
				Assert.That(_httpCookie.Value, Is.EqualTo(_cookie.Value));
				Assert.That(
					_httpCookie.Values.AllKeys
						.Where(arg => arg != null)
						.Select(arg => new Tuple<string, string>(arg, _httpCookie.Values[arg])),
					Is.EquivalentTo(_cookie.Values.Select(arg => new Tuple<string, string>(arg.Name, arg.Value))));
			}
		}
	}
}
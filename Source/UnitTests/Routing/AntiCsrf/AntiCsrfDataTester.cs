using System.Web;

using Junior.Route.Routing.AntiCsrf;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Routing.AntiCsrf
{
	public static class AntiCsrfDataTester
	{
		[TestFixture]
		public class When_creating_instance
		{
			[SetUp]
			public void SetUp()
			{
				_cookie = new HttpCookie("name");
				_data = new AntiCsrfData("token", "html", _cookie);
			}

			private HttpCookie _cookie;
			private AntiCsrfData _data;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_data.Cookie, Is.SameAs(_cookie));
				Assert.That(_data.HiddenInputFieldHtml, Is.EqualTo("html"));
				Assert.That(_data.Token, Is.EqualTo("token"));
			}
		}
	}
}
using System.Web;

using Junior.Common;

namespace Junior.Route.Routing.AntiCsrf
{
	public class AntiCsrfData
	{
		private readonly HttpCookie _cookie;
		private readonly string _hiddenInputFieldHtml;
		private readonly string _token;

		public AntiCsrfData(string token, string hiddenInputFieldHtml, HttpCookie cookie)
		{
			token.ThrowIfNull("token");
			hiddenInputFieldHtml.ThrowIfNull("hiddenInputFieldHtml");
			cookie.ThrowIfNull("cookie");

			_token = token;
			_hiddenInputFieldHtml = hiddenInputFieldHtml;
			_cookie = cookie;
		}

		public string Token
		{
			get
			{
				return _token;
			}
		}

		public string HiddenInputFieldHtml
		{
			get
			{
				return _hiddenInputFieldHtml;
			}
		}

		public HttpCookie Cookie
		{
			get
			{
				return _cookie;
			}
		}
	}
}
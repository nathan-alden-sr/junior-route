using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Junior.Common;
using Junior.Route.Common;

namespace Junior.Route.Routing.Responses
{
	public sealed class Cookie
	{
		private readonly string _domain;
		private readonly DateTime _expires;
		private readonly bool _httpOnly;
		private readonly string _name;
		private readonly string _path;
		private readonly bool _secure;
		private readonly bool _shareable;
		private readonly string _value;
		private readonly HashSet<CookieValue> _values = new HashSet<CookieValue>();

		public Cookie(HttpCookie cookie, bool shareable = false)
		{
			cookie.ThrowIfNull("cookie");

			_name = cookie.Name;
			_path = cookie.Path;
			_secure = cookie.Secure;
			_shareable = shareable;
			_httpOnly = cookie.HttpOnly;
			_domain = cookie.Domain;
			_expires = cookie.Expires;
			_value = cookie.Value;

			IEnumerable<CookieValue> cookieValues = cookie.Values.AllKeys
				.Where(arg => arg != null)
				.Select(arg => new CookieValue(arg, cookie.Values[arg]));

			_values.AddRange(cookieValues);
		}

		public bool Shareable
		{
			get
			{
				return _shareable;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public string Path
		{
			get
			{
				return _path;
			}
		}

		public bool Secure
		{
			get
			{
				return _secure;
			}
		}

		public bool HttpOnly
		{
			get
			{
				return _httpOnly;
			}
		}

		public string Domain
		{
			get
			{
				return _domain;
			}
		}

		public DateTime Expires
		{
			get
			{
				return _expires;
			}
		}

		public string Value
		{
			get
			{
				return _value;
			}
		}

		public IEnumerable<CookieValue> Values
		{
			get
			{
				return _values;
			}
		}

		public HttpCookie GetHttpCookie()
		{
			var cookie = new HttpCookie(Name, Value)
				{
					Domain = Domain,
					Expires = Expires,
					HttpOnly = HttpOnly,
					Path = Path,
					Secure = Secure
				};

			foreach (CookieValue value in Values)
			{
				cookie.Values.Add(value.Name, value.Value);
			}

			return cookie;
		}

		public Cookie Clone()
		{
			return new Cookie(GetHttpCookie(), _shareable);
		}
	}
}
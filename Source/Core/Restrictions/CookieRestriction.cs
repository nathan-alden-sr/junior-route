using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

using Junior.Common;

using NathanAlden.JuniorRouting.Core.RequestValueComparers;

namespace NathanAlden.JuniorRouting.Core.Restrictions
{
	[DebuggerDisplay("{_name}={_value}")]
	public class CookieRestriction : IHttpRouteRestriction
	{
		private readonly string _name;
		private readonly IRequestValueComparer _nameComparer;
		private readonly string _value;
		private readonly IRequestValueComparer _valueComparer;

		public CookieRestriction(string name, IRequestValueComparer nameComparer, string value, IRequestValueComparer valueComparer)
		{
			name.ThrowIfNull("cookie");
			nameComparer.ThrowIfNull("cookieComparer");
			value.ThrowIfNull("value");
			valueComparer.ThrowIfNull("valueComparer");

			_name = name;
			_nameComparer = nameComparer;
			_value = value;
			_valueComparer = valueComparer;
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public IRequestValueComparer NameComparer
		{
			get
			{
				return _nameComparer;
			}
		}

		public string Value
		{
			get
			{
				return _value;
			}
		}

		public IRequestValueComparer ValueComparer
		{
			get
			{
				return _valueComparer;
			}
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			IEnumerable<string> matchingKeys = request.Cookies.AllKeys.Where(arg => _nameComparer.Matches(_name, arg));

			return matchingKeys.Any(arg => _valueComparer.Matches(_value, request.Cookies[arg].Value));
		}
	}
}
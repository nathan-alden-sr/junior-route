using System;
using System.Diagnostics;
using System.Web;

using Junior.Common;

namespace NathanAlden.JuniorRouting.Core.Restrictions
{
	[DebuggerDisplay("{_method}")]
	public class MethodRestriction : IHttpRouteRestriction
	{
		private readonly string _method;

		public MethodRestriction(string method)
		{
			method.ThrowIfNull("method");

			_method = method;
		}

		public string Method
		{
			get
			{
				return _method;
			}
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return String.Equals(_method, request.HttpMethod, StringComparison.OrdinalIgnoreCase);
		}
	}
}
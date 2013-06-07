using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;

namespace Junior.Route.Routing.Restrictions
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class MethodRestriction : IRestriction, IEquatable<MethodRestriction>
	{
		private readonly string _method;

		public MethodRestriction(string method)
		{
			method.ThrowIfNull("method");

			_method = method.ToUpperInvariant();
		}

		public string Method
		{
			get
			{
				return _method;
			}
		}

		// ReSharper disable UnusedMember.Local
		private string DebuggerDisplay
			// ReSharper restore UnusedMember.Local
		{
			get
			{
				return _method;
			}
		}

		public bool Equals(MethodRestriction other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return String.Equals(_method, other._method, StringComparison.OrdinalIgnoreCase);
		}

		public Task<bool> MatchesRequestAsync(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return String.Equals(_method, request.HttpMethod, StringComparison.OrdinalIgnoreCase).AsCompletedTask();
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((MethodRestriction)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return _method.GetHashCode();
			}
		}
	}
}
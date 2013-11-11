using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;

using Junior.Common;

namespace Junior.Route.Routing.Restrictions
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class RefererUrlPortRestriction : IRestriction, IEquatable<RefererUrlPortRestriction>
	{
		private readonly ushort _port;

		public RefererUrlPortRestriction(ushort port)
		{
			_port = port;
		}

		public ushort Port
		{
			get
			{
				return _port;
			}
		}

		// ReSharper disable UnusedMember.Local
		private string DebuggerDisplay
			// ReSharper restore UnusedMember.Local
		{
			get
			{
				return _port.ToString(CultureInfo.InvariantCulture);
			}
		}

		public bool Equals(RefererUrlPortRestriction other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return _port == other._port;
		}

		public MatchResult MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return _port == request.UrlReferrer.Port ? MatchResult.RestrictionMatched(this.ToEnumerable()) : MatchResult.RestrictionNotMatched(Enumerable.Empty<IRestriction>(), this.ToEnumerable());
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
			return Equals((RefererUrlPortRestriction)obj);
		}

		public override int GetHashCode()
		{
			return _port.GetHashCode();
		}
	}
}
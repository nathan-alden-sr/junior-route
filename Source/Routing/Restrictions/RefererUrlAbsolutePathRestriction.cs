using System;
using System.Diagnostics;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.RequestValueComparers;

namespace Junior.Route.Routing.Restrictions
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class RefererUrlAbsolutePathRestriction : IRouteRestriction, IEquatable<RefererUrlAbsolutePathRestriction>
	{
		private readonly string _absolutePath;
		private readonly IRequestValueComparer _comparer;

		public RefererUrlAbsolutePathRestriction(string absolutePath, IRequestValueComparer comparer)
		{
			absolutePath.ThrowIfNull("absolutePath");
			comparer.ThrowIfNull("comparer");

			_absolutePath = absolutePath;
			_comparer = comparer;
		}

		public string AbsolutePath
		{
			get
			{
				return _absolutePath;
			}
		}

		public IRequestValueComparer Comparer
		{
			get
			{
				return _comparer;
			}
		}

		// ReSharper disable UnusedMember.Local
		private string DebuggerDisplay
			// ReSharper restore UnusedMember.Local
		{
			get
			{
				return _absolutePath;
			}
		}

		public bool Equals(RefererUrlAbsolutePathRestriction other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return Equals(_comparer, other._comparer) && String.Equals(_absolutePath, other._absolutePath);
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return _comparer.Matches(_absolutePath, request.UrlReferrer.AbsolutePath);
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
			return Equals((RefererUrlAbsolutePathRestriction)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((_comparer != null ? _comparer.GetHashCode() : 0) * 397) ^ (_absolutePath != null ? _absolutePath.GetHashCode() : 0);
			}
		}
	}
}
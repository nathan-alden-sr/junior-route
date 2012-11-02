using System;
using System.Diagnostics;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.RequestValueComparers;

namespace Junior.Route.Routing.Restrictions
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class UrlFragmentRestriction : IRestriction, IEquatable<UrlFragmentRestriction>
	{
		private readonly IRequestValueComparer _comparer;
		private readonly string _fragment;

		public UrlFragmentRestriction(string fragment, IRequestValueComparer comparer)
		{
			fragment.ThrowIfNull("fragment");
			comparer.ThrowIfNull("comparer");

			_fragment = fragment;
			_comparer = comparer;
		}

		public string Fragment
		{
			get
			{
				return _fragment;
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
				return _fragment;
			}
		}

		public bool Equals(UrlFragmentRestriction other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return Equals(_comparer, other._comparer) && String.Equals(_fragment, other._fragment);
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return _comparer.Matches(_fragment, request.Url.Fragment);
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
			return Equals((UrlFragmentRestriction)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((_comparer != null ? _comparer.GetHashCode() : 0) * 397) ^ (_fragment != null ? _fragment.GetHashCode() : 0);
			}
		}
	}
}
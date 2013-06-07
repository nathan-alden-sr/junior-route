using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.RequestValueComparers;

namespace Junior.Route.Routing.Restrictions
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class UrlRelativePathRestriction : IRestriction, IEquatable<UrlRelativePathRestriction>
	{
		private readonly IRequestValueComparer _comparer;
		private readonly IHttpRuntime _httpRuntime;
		private readonly string _relativePath;

		public UrlRelativePathRestriction(string relativePath, IRequestValueComparer comparer, IHttpRuntime httpRuntime)
		{
			relativePath.ThrowIfNull("relativePath");
			comparer.ThrowIfNull("comparer");
			httpRuntime.ThrowIfNull("httpRuntime");

			_relativePath = relativePath;
			_comparer = comparer;
			_httpRuntime = httpRuntime;
		}

		public string RelativePath
		{
			get
			{
				return _relativePath;
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
				return _relativePath;
			}
		}

		public bool Equals(UrlRelativePathRestriction other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return Equals(_comparer, other._comparer) && String.Equals(_relativePath, other._relativePath);
		}

		public Task<bool> MatchesRequestAsync(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			return _comparer.Matches(_relativePath, GetRelativeUrl(request)).AsCompletedTask();
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
			return Equals((UrlRelativePathRestriction)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((_comparer != null ? _comparer.GetHashCode() : 0) * 397) ^ (_relativePath != null ? _relativePath.GetHashCode() : 0);
			}
		}

		private string GetRelativeUrl(HttpRequestBase request)
		{
			string virtualPath = _httpRuntime.AppDomainAppVirtualPath;
			string absolutePath = new Uri(request.Url.AbsoluteUri).AbsolutePath;

			return absolutePath.Remove(0, virtualPath.Length).TrimStart('/');
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;

using NathanAlden.JuniorRouting.Core;
using NathanAlden.JuniorRouting.Core.Restrictions;

namespace NathanAlden.JuniorRouting.Diagnostics.Responses
{
	public abstract class RouteTableView : View
	{
		public override string Title
		{
			get
			{
				return "Route Table - JuniorRouting";
			}
		}

		public override string RootUrl
		{
			get
			{
				return base.RootUrl + BaseUrl;
			}
		}

		public IEnumerable<HttpRoute> Routes
		{
			get;
			private set;
		}

		public int VisibleRestrictionTypeColumns
		{
			get;
			private set;
		}

		private string BaseUrl
		{
			get;
			set;
		}

		private IEnumerable<Type> RouteRestrictionTypes
		{
			get;
			set;
		}

		private static IEnumerable<Type> AvailableRestrictionTypes
		{
			get
			{
				yield return typeof(CookieRestriction);
				yield return typeof(HeaderRestriction);
				yield return typeof(MethodRestriction);
				yield return typeof(RefererUrlAuthorityRestriction);
				yield return typeof(RefererUrlFragmentRestriction);
				yield return typeof(RefererUrlHostRestriction);
				yield return typeof(RefererUrlHostTypeRestriction);
				yield return typeof(RefererUrlLocalPathRestriction);
				yield return typeof(RefererUrlPathAndQueryRestriction);
				yield return typeof(RefererUrlPortRestriction);
				yield return typeof(RefererUrlQueryRestriction);
				yield return typeof(RefererUrlQueryStringRestriction);
				yield return typeof(RefererUrlSchemeRestriction);
				yield return typeof(RelativeUrlRestriction);
				yield return typeof(UrlAuthorityRestriction);
				yield return typeof(UrlFragmentRestriction);
				yield return typeof(UrlHostRestriction);
				yield return typeof(UrlHostTypeRestriction);
				yield return typeof(UrlLocalPathRestriction);
				yield return typeof(UrlPathAndQueryRestriction);
				yield return typeof(UrlPortRestriction);
				yield return typeof(UrlQueryRestriction);
				yield return typeof(UrlQueryStringRestriction);
				yield return typeof(UrlSchemeRestriction);
			}
		}

		public void Populate(HttpRoutes routes, string baseUrl)
		{
			BaseUrl = baseUrl;
			RouteRestrictionTypes = routes.SelectMany(arg => arg.RestrictionTypes).Distinct().ToArray();
			Routes = routes;
			VisibleRestrictionTypeColumns =
				RouteRestrictionTypes.Intersect(AvailableRestrictionTypes).Distinct().Count() +
				(RouteRestrictionTypes.Contains(typeof(RefererUrlQueryStringRestriction)) ? 1 : 0) +
				(RouteRestrictionTypes.Contains(typeof(UrlQueryStringRestriction)) ? 1 : 0) +
				(RouteRestrictionTypes.Contains(typeof(CookieRestriction)) ? 1 : 0) +
				(RouteRestrictionTypes.Contains(typeof(HeaderRestriction)) ? 1 : 0);
		}

		public string ColumnVisibilityCss<T>()
			where T : IHttpRouteRestriction
		{
			return RouteRestrictionTypes.Contains(typeof(T)) ? "" : "invisible";
		}

		public string MapUriHostNameType(UriHostNameType type)
		{
			switch (type)
			{
				case UriHostNameType.Unknown:
					return "Unknown";
				case UriHostNameType.Basic:
					return "Basic";
				case UriHostNameType.Dns:
					return "DNS";
				case UriHostNameType.IPv4:
					return "IPv4";
				case UriHostNameType.IPv6:
					return "IPv6";
				default:
					throw new ArgumentOutOfRangeException("type");
			}
		}
	}
}
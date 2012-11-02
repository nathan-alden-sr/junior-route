using System;
using System.Collections.Generic;
using System.Linq;

using Junior.Common;
using Junior.Route.Common;
using Junior.Route.Diagnostics.Web;
using Junior.Route.Routing.Restrictions;

namespace Junior.Route.Routing.Diagnostics.Web
{
	public abstract class RouteTableView : View
	{
		private static readonly IEnumerable<Type> _availableRestrictionTypes = new[]
			{
				typeof(CookieRestriction),
				typeof(HeaderRestriction),
				typeof(HeaderRestriction<>),
				typeof(MethodRestriction),
				typeof(MissingHeaderRestriction),
				typeof(RefererUrlAbsolutePathRestriction),
				typeof(RefererUrlAuthorityRestriction),
				typeof(RefererUrlFragmentRestriction),
				typeof(RefererUrlHostRestriction),
				typeof(RefererUrlHostTypeRestriction),
				typeof(RefererUrlPathAndQueryRestriction),
				typeof(RefererUrlPortRestriction),
				typeof(RefererUrlQueryRestriction),
				typeof(RefererUrlQueryStringRestriction),
				typeof(RefererUrlSchemeRestriction),
				typeof(UrlRelativePathRestriction),
				typeof(UrlAuthorityRestriction),
				typeof(UrlFragmentRestriction),
				typeof(UrlHostRestriction),
				typeof(UrlHostTypeRestriction),
				typeof(UrlPortRestriction),
				typeof(UrlQueryRestriction),
				typeof(UrlQueryStringRestriction),
				typeof(UrlSchemeRestriction)
			};

		public override string Title
		{
			get
			{
				return "Route Table - JuniorRoute";
			}
		}

		public IEnumerable<Route> Routes
		{
			get;
			private set;
		}

		public int VisibleRestrictionTypeColumns
		{
			get;
			private set;
		}

		public int VisibleUrlRestrictionTypeColumns
		{
			get;
			private set;
		}

		public int VisibleRefererUrlRestrictionTypeColumns
		{
			get;
			private set;
		}

		private IEnumerable<Type> RouteRestrictionTypes
		{
			get;
			set;
		}

		public void Populate(IUrlResolver urlResolver, IEnumerable<Route> routes, string baseUrl)
		{
			urlResolver.ThrowIfNull("urlResolver");
			routes.ThrowIfNull("routes");
			baseUrl.ThrowIfNull("baseUrl");

			UrlResolver = urlResolver;
			routes = routes.ToArray();

			Type[] types = routes.SelectMany(arg => arg.GetRestrictionTypes()).Distinct().ToArray();
			RouteRestrictionTypes = types;
			Routes = routes;
			VisibleRestrictionTypeColumns =
				RouteRestrictionTypes.Intersect(_availableRestrictionTypes).Distinct().Count() +
				RouteRestrictionTypes.Where(arg => arg.IsGenericType).Select(arg => arg.GetGenericTypeDefinition()).Intersect(_availableRestrictionTypes).Distinct().Count() +
				(RouteRestrictionTypes.Contains(typeof(RefererUrlQueryStringRestriction)) ? 1 : 0) +
				(RouteRestrictionTypes.Contains(typeof(UrlQueryStringRestriction)) ? 1 : 0) +
				(RouteRestrictionTypes.Contains(typeof(CookieRestriction)) ? 1 : 0) +
				(RouteRestrictionTypes.Contains(typeof(HeaderRestriction)) ? 1 : 0);
			VisibleUrlRestrictionTypeColumns =
				RouteRestrictionTypes.Intersect(_availableRestrictionTypes.Where(arg => arg.Name.StartsWith("Url") || arg == typeof(UrlRelativePathRestriction))).Distinct().Count() +
				(RouteRestrictionTypes.Contains(typeof(UrlQueryStringRestriction)) ? 1 : 0);
			VisibleRefererUrlRestrictionTypeColumns =
				RouteRestrictionTypes.Intersect(_availableRestrictionTypes.Where(arg => arg.Name.StartsWith("RefererUrl"))).Distinct().Count() +
				(RouteRestrictionTypes.Contains(typeof(RefererUrlQueryStringRestriction)) ? 1 : 0);
		}

		public string ColumnVisibilityCss<T>()
			where T : IRestriction
		{
			return RouteRestrictionTypes.Contains(typeof(T)) ? "" : "invisible";
		}

		public string ColumnVisibilityCss(Type genericTypeDefinition)
		{
			return RouteRestrictionTypes.Any(arg => arg.IsGenericType && arg.GetGenericTypeDefinition() == genericTypeDefinition) ? "" : "invisible";
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
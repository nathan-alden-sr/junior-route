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

		private IEnumerable<Type> RestrictionTypes
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
			RestrictionTypes = types;
			Routes = routes;
			VisibleRestrictionTypeColumns =
				RestrictionTypes.Intersect(_availableRestrictionTypes).Distinct().Count() +
				RestrictionTypes.Where(arg => arg.IsGenericType).Select(arg => arg.GetGenericTypeDefinition()).Intersect(_availableRestrictionTypes).Distinct().Count() +
				(RestrictionTypes.Contains(typeof(RefererUrlQueryStringRestriction)) ? 1 : 0) +
				(RestrictionTypes.Contains(typeof(UrlQueryStringRestriction)) ? 1 : 0) +
				(RestrictionTypes.Contains(typeof(CookieRestriction)) ? 1 : 0) +
				(RestrictionTypes.Contains(typeof(HeaderRestriction)) ? 1 : 0);
			VisibleUrlRestrictionTypeColumns =
				RestrictionTypes.Intersect(_availableRestrictionTypes.Where(arg => arg.Name.StartsWith("Url") || arg == typeof(UrlRelativePathRestriction))).Distinct().Count() +
				(RestrictionTypes.Contains(typeof(UrlQueryStringRestriction)) ? 1 : 0);
			VisibleRefererUrlRestrictionTypeColumns =
				RestrictionTypes.Intersect(_availableRestrictionTypes.Where(arg => arg.Name.StartsWith("RefererUrl"))).Distinct().Count() +
				(RestrictionTypes.Contains(typeof(RefererUrlQueryStringRestriction)) ? 1 : 0);
		}

		public string ColumnVisibilityCss<T>()
			where T : IRestriction
		{
			return RestrictionTypes.Contains(typeof(T)) ? "" : "invisible";
		}

		public string ColumnVisibilityCss(Type genericTypeDefinition)
		{
			return RestrictionTypes.Any(arg => arg.IsGenericType && arg.GetGenericTypeDefinition() == genericTypeDefinition) ? "" : "invisible";
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
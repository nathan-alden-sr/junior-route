using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.Common;
using Junior.Route.Http.RequestHeaders;
using Junior.Route.Routing.AuthenticationProviders;
using Junior.Route.Routing.RequestValueComparers;
using Junior.Route.Routing.Responses;
using Junior.Route.Routing.Restrictions;

namespace Junior.Route.Routing
{
	[DebuggerDisplay(@"{DebuggerDisplay,nq}")]
	public class Route
	{
		private readonly Guid _id;
		private readonly string _name;
		private readonly Dictionary<Type, HashSet<IRestriction>> _restrictionsByRestrictionType = new Dictionary<Type, HashSet<IRestriction>>();
		private readonly Scheme _scheme;
		private IAuthenticationProvider _authenticationProvider;
		private string _resolvedRelativeUrl;
		private Func<HttpContextBase, Task<IResponse>> _responseDelegate = context => new Response().NoContent().AsCompletedTask<IResponse>();

		public Route(string name, Guid id, Scheme scheme, string resolvedRelativeUrl)
		{
			name.ThrowIfNull("name");
			resolvedRelativeUrl.ThrowIfNull("resolvedRelativeUrl");

			_name = name;
			_id = id;
			_scheme = scheme;
			_resolvedRelativeUrl = resolvedRelativeUrl;
		}

		protected Route(string name, Guid id, Scheme scheme)
		{
			name.ThrowIfNull("name");

			_name = name;
			_id = id;
			_scheme = scheme;
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public Guid Id
		{
			get
			{
				return _id;
			}
		}

		public Scheme Scheme
		{
			get
			{
				return _scheme;
			}
		}

		public string ResolvedRelativeUrl
		{
			get
			{
				return _resolvedRelativeUrl;
			}
			protected set
			{
				value.ThrowIfNull("value");

				_resolvedRelativeUrl = value;
			}
		}

		public Type ResponseType
		{
			get;
			private set;
		}

		// ReSharper disable UnusedMember.Local
		private string DebuggerDisplay
			// ReSharper restore UnusedMember.Local
		{
			get
			{
				return String.Format("Name={0}; Id={1}; Restrictions={2}; Response={3}", _name, _id, String.Join(", ", GetRestrictionTypes().Select(arg => arg.Name)), ResponseType.IfNotNull(arg => arg.Name) ?? "None");
			}
		}

		#region Restrictions

		public Route RestrictByCookie(string name, string value)
		{
			return AddRestrictions(new CookieRestriction(name, CaseInsensitivePlainComparer.Instance, value, CaseInsensitivePlainComparer.Instance));
		}

		public Route RestrictByCookie(string name, IRequestValueComparer nameComparer, string value, IRequestValueComparer valueComparer)
		{
			return AddRestrictions(new CookieRestriction(name, nameComparer, value, valueComparer));
		}

		public Route RestrictByHeader(string field, string value)
		{
			return AddRestrictions(new HeaderRestriction(field, value, CaseInsensitivePlainComparer.Instance));
		}

		public Route RestrictByHeader(string field, string value, IRequestValueComparer valueComparer)
		{
			return AddRestrictions(new HeaderRestriction(field, value, valueComparer));
		}

		public Route RestrictByHeader<T>(string field, Func<string, T> parseDelegate, Func<T, bool> matchDelegate)
		{
			return AddRestrictions(new HeaderRestriction<T>(field, parseDelegate, matchDelegate));
		}

		public Route RestrictByHeaders<T>(string field, Func<string, IEnumerable<T>> parseDelegate, Func<T, bool> matchDelegate)
		{
			return AddRestrictions(new HeaderRestriction<T>(field, parseDelegate, matchDelegate));
		}

		public Route RestrictByAcceptCharsetHeader(Func<AcceptCharsetHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("Accept-Charset", AcceptCharsetHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByAcceptEncodingHeader(Func<AcceptEncodingHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("Accept-Encoding", AcceptEncodingHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByAcceptHeader(Func<AcceptHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("Accept", AcceptHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByAcceptLanguageHeader(Func<AcceptLanguageHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("Accept-Language", AcceptLanguageHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByAllowHeader(Func<AllowHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("Allow", AllowHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByBasicAuthorizationHeader(Func<BasicAuthorizationHeader, bool> matchDelegate)
		{
			// ReSharper disable RedundantCast
			return RestrictByHeader("Authorization", (Func<string, BasicAuthorizationHeader>)BasicAuthorizationHeader.Parse, matchDelegate);
			// ReSharper restore RedundantCast
		}

		public Route RestrictByBasicProxyAuthorizationHeader(Func<BasicProxyAuthorizationHeader, bool> matchDelegate)
		{
			// ReSharper disable RedundantCast
			return RestrictByHeader("Proxy-Authorization", (Func<string, BasicProxyAuthorizationHeader>)BasicProxyAuthorizationHeader.Parse, matchDelegate);
			// ReSharper restore RedundantCast
		}

		public Route RestrictByCacheControlHeader(Func<CacheControlHeader, bool> matchDelegate)
		{
			// ReSharper disable RedundantCast
			return RestrictByHeader("Cache-Control", (Func<string, CacheControlHeader>)CacheControlHeader.Parse, matchDelegate);
			// ReSharper restore RedundantCast
		}

		public Route RestrictByConnectionHeader(Func<ConnectionHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("Connection", ConnectionHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByContentEncodingHeader(Func<ContentEncodingHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("Content-Encoding", ContentEncodingHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByContentLanguageHeader(Func<ContentLanguageHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("Content-Language", ContentLanguageHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByContentLengthHeader(Func<ContentLengthHeader, bool> matchDelegate)
		{
			// ReSharper disable RedundantCast
			return RestrictByHeader("Content-Length", (Func<string, ContentLengthHeader>)ContentLengthHeader.Parse, matchDelegate);
			// ReSharper restore RedundantCast
		}

		public Route RestrictByContentMd5Header(Func<ContentMd5Header, bool> matchDelegate)
		{
			// ReSharper disable RedundantCast
			return RestrictByHeader("Content-MD5", (Func<string, ContentMd5Header>)ContentMd5Header.Parse, matchDelegate);
			// ReSharper restore RedundantCast
		}

		public Route RestrictByDateHeader(Func<DateHeader, bool> matchDelegate)
		{
			// ReSharper disable RedundantCast
			return RestrictByHeader("Date", (Func<string, DateHeader>)DateHeader.Parse, matchDelegate);
			// ReSharper restore RedundantCast
		}

		public Route RestrictByDigestAuthorizationHeader(Func<DigestAuthorizationHeader, bool> matchDelegate)
		{
			// ReSharper disable RedundantCast
			return RestrictByHeader("Authorization", (Func<string, DigestAuthorizationHeader>)DigestAuthorizationHeader.Parse, matchDelegate);
			// ReSharper restore RedundantCast
		}

		public Route RestrictByDigestProxyAuthorizationHeader(Func<DigestProxyAuthorizationHeader, bool> matchDelegate)
		{
			// ReSharper disable RedundantCast
			return RestrictByHeader("Proxy-Authorization", (Func<string, DigestProxyAuthorizationHeader>)DigestProxyAuthorizationHeader.Parse, matchDelegate);
			// ReSharper restore RedundantCast
		}

		public Route RestrictByExpectHeader(Func<ExpectHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("Expect", ExpectHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByFromHeader(Func<FromHeader, bool> matchDelegate)
		{
			// ReSharper disable RedundantCast
			return RestrictByHeader("From", (Func<string, FromHeader>)FromHeader.Parse, matchDelegate);
			// ReSharper restore RedundantCast
		}

		public Route RestrictByHostHeader(Func<HostHeader, bool> matchDelegate)
		{
			// ReSharper disable RedundantCast
			return RestrictByHeader("Host", (Func<string, HostHeader>)HostHeader.Parse, matchDelegate);
			// ReSharper restore RedundantCast
		}

		public Route RestrictByIfMatchHeader(Func<IfMatchHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("If-Match", IfMatchHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByIfModifiedSinceHeader(Func<IfModifiedSinceHeader, bool> matchDelegate)
		{
			// ReSharper disable RedundantCast
			return RestrictByHeader("If-Modified-Since", (Func<string, IfModifiedSinceHeader>)IfModifiedSinceHeader.Parse, matchDelegate);
			// ReSharper restore RedundantCast
		}

		public Route RestrictByIfNoneMatchHeader(Func<IfNoneMatchHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("If-None-Match", IfNoneMatchHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByIfRangeHeader(Func<IfRangeHeader, bool> matchDelegate)
		{
			// ReSharper disable RedundantCast
			return RestrictByHeader("If-Range", (Func<string, IfRangeHeader>)IfRangeHeader.Parse, matchDelegate);
			// ReSharper restore RedundantCast
		}

		public Route RestrictByIfUnmodifiedSinceHeader(Func<IfUnmodifiedSinceHeader, bool> matchDelegate)
		{
			// ReSharper disable RedundantCast
			return RestrictByHeader("If-Unmodified-Since", (Func<string, IfUnmodifiedSinceHeader>)IfUnmodifiedSinceHeader.Parse, matchDelegate);
			// ReSharper restore RedundantCast
		}

		public Route RestrictByMaxForwardsHeader(Func<MaxForwardsHeader, bool> matchDelegate)
		{
			// ReSharper disable RedundantCast
			return RestrictByHeader("Max-Forwards", (Func<string, MaxForwardsHeader>)MaxForwardsHeader.Parse, matchDelegate);
			// ReSharper restore RedundantCast
		}

		public Route RestrictByPragmaHeader(Func<PragmaHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("Pragma", PragmaHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByRangeHeader(Func<RangeHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("Range", RangeHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByRefererHeader(Func<RefererHeader, bool> matchDelegate)
		{
			// ReSharper disable RedundantCast
			return RestrictByHeader("Referer", (Func<string, RefererHeader>)RefererHeader.Parse, matchDelegate);
			// ReSharper restore RedundantCast
		}

		public Route RestrictByTeHeader(Func<TeHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("TE", TeHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByTrailerHeader(Func<TrailerHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("Trailer", TrailerHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByTransferEncodingHeader(Func<TransferEncodingHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("Transfer-Encoding", TransferEncodingHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByUpgradeHeader(Func<UpgradeHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("Upgrade", UpgradeHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByUserAgentHeader(Func<UserAgentHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("User-Agent", UserAgentHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByVaryHeader(Func<VaryHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("Vary", VaryHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByViaHeader(Func<ViaHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("Via", ViaHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByWarningHeader(Func<WarningHeader, bool> matchDelegate)
		{
			return RestrictByHeaders("Warning", WarningHeader.ParseMany, matchDelegate);
		}

		public Route RestrictByMissingHeader(string field)
		{
			return AddRestrictions(new MissingHeaderRestriction(field));
		}

		public Route RestrictByMethods(IEnumerable<HttpMethod> methods)
		{
			methods.ThrowIfNull("methods");

			return AddRestrictions(methods.Select(arg => new MethodRestriction(arg.ToString())));
		}

		public Route RestrictByMethods(params HttpMethod[] methods)
		{
			return RestrictByMethods((IEnumerable<HttpMethod>)methods);
		}

		public Route RestrictByMethods(IEnumerable<string> methods)
		{
			methods.ThrowIfNull("methods");

			return AddRestrictions(methods.Select(arg => new MethodRestriction(arg)));
		}

		public Route RestrictByMethods(params string[] methods)
		{
			return RestrictByMethods((IEnumerable<string>)methods);
		}

		public Route RestrictByRefererUrl(Func<Uri, bool> matchDelegate)
		{
			return AddRestrictions(new RefererUrlRestriction(matchDelegate));
		}

		public Route RestrictByRefererUrlAbsolutePath(string absolutePath, IRequestValueComparer comparer)
		{
			return AddRestrictions(new RefererUrlAbsolutePathRestriction(absolutePath, comparer));
		}

		public Route RestrictByRefererUrlAbsolutePaths(IEnumerable<string> absolutePaths)
		{
			absolutePaths.ThrowIfNull("absolutePaths");

			return AddRestrictions(absolutePaths.Select(arg => new RefererUrlAbsolutePathRestriction(arg, CaseInsensitivePlainComparer.Instance)));
		}

		public Route RestrictByRefererUrlAbsolutePaths(params string[] pathsAndQueries)
		{
			return RestrictByRefererUrlAbsolutePaths((IEnumerable<string>)pathsAndQueries);
		}

		public Route RestrictByRefererUrlAbsolutePaths(IEnumerable<string> absolutePaths, IRequestValueComparer comparer)
		{
			absolutePaths.ThrowIfNull("absolutePaths");

			return AddRestrictions(absolutePaths.Select(arg => new RefererUrlAbsolutePathRestriction(arg, comparer)));
		}

		public Route RestrictByRefererUrlAuthority(string authority, IRequestValueComparer comparer)
		{
			return AddRestrictions(new RefererUrlAuthorityRestriction(authority, comparer));
		}

		public Route RestrictByRefererUrlAuthorities(IEnumerable<string> authorities)
		{
			authorities.ThrowIfNull("authorities");

			return AddRestrictions(authorities.Select(arg => new RefererUrlAuthorityRestriction(arg, CaseInsensitivePlainComparer.Instance)));
		}

		public Route RestrictByRefererUrlAuthorities(params string[] authorities)
		{
			return RestrictByRefererUrlAuthorities((IEnumerable<string>)authorities);
		}

		public Route RestrictByRefererUrlAuthorities(IEnumerable<string> authorities, IRequestValueComparer comparer)
		{
			authorities.ThrowIfNull("authorities");

			return AddRestrictions(authorities.Select(arg => new RefererUrlAuthorityRestriction(arg, comparer)));
		}

		public Route RestrictByRefererUrlFragment(string fragment, IRequestValueComparer comparer)
		{
			return AddRestrictions(new RefererUrlFragmentRestriction(fragment, comparer));
		}

		public Route RestrictByRefererUrlFragments(IEnumerable<string> fragments)
		{
			fragments.ThrowIfNull("fragments");

			return AddRestrictions(fragments.Select(arg => new RefererUrlFragmentRestriction(arg, CaseInsensitivePlainComparer.Instance)));
		}

		public Route RestrictByRefererUrlFragments(params string[] fragments)
		{
			return RestrictByRefererUrlFragments((IEnumerable<string>)fragments);
		}

		public Route RestrictByRefererUrlFragments(IEnumerable<string> fragments, IRequestValueComparer comparer)
		{
			fragments.ThrowIfNull("fragments");

			return AddRestrictions(fragments.Select(arg => new RefererUrlFragmentRestriction(arg, comparer)));
		}

		public Route RestrictByRefererUrlHost(string host, IRequestValueComparer comparer)
		{
			return AddRestrictions(new RefererUrlHostRestriction(host, comparer));
		}

		public Route RestrictByRefererUrlHosts(IEnumerable<string> hosts)
		{
			hosts.ThrowIfNull("hosts");

			return AddRestrictions(hosts.Select(arg => new RefererUrlHostRestriction(arg, CaseInsensitivePlainComparer.Instance)));
		}

		public Route RestrictByRefererUrlHosts(params string[] hosts)
		{
			return RestrictByRefererUrlHosts((IEnumerable<string>)hosts);
		}

		public Route RestrictByRefererUrlHosts(IEnumerable<string> hosts, IRequestValueComparer comparer)
		{
			hosts.ThrowIfNull("hosts");

			return AddRestrictions(hosts.Select(arg => new RefererUrlHostRestriction(arg, comparer)));
		}

		public Route RestrictByRefererUrlHostTypes(IEnumerable<UriHostNameType> types)
		{
			types.ThrowIfNull("types");

			return AddRestrictions(types.Select(arg => new RefererUrlHostTypeRestriction(arg)));
		}

		public Route RestrictByRefererUrlHostTypes(params UriHostNameType[] types)
		{
			return RestrictByRefererUrlHostTypes((IEnumerable<UriHostNameType>)types);
		}

		public Route RestrictByRefererUrlPathAndQuery(string pathAndQuery, IRequestValueComparer comparer)
		{
			return AddRestrictions(new RefererUrlPathAndQueryRestriction(pathAndQuery, comparer));
		}

		public Route RestrictByRefererUrlPathsAndQueries(IEnumerable<string> pathsAndQueries)
		{
			pathsAndQueries.ThrowIfNull("pathsAndQueries");

			return AddRestrictions(pathsAndQueries.Select(arg => new RefererUrlPathAndQueryRestriction(arg, CaseInsensitivePlainComparer.Instance)));
		}

		public Route RestrictByRefererUrlPathsAndQueries(params string[] pathsAndQueries)
		{
			return RestrictByRefererUrlPathsAndQueries((IEnumerable<string>)pathsAndQueries);
		}

		public Route RestrictByRefererUrlPathsAndQueries(IEnumerable<string> pathsAndQueries, IRequestValueComparer comparer)
		{
			pathsAndQueries.ThrowIfNull("pathsAndQueries");

			return AddRestrictions(pathsAndQueries.Select(arg => new RefererUrlPathAndQueryRestriction(arg, comparer)));
		}

		public Route RestrictByRefererUrlPorts(IEnumerable<ushort> ports)
		{
			ports.ThrowIfNull("ports");

			return AddRestrictions(ports.Select(arg => new RefererUrlPortRestriction(arg)));
		}

		public Route RestrictByRefererUrlPorts(params ushort[] ports)
		{
			return RestrictByRefererUrlPorts((IEnumerable<ushort>)ports);
		}

		public Route RestrictByRefererUrlQuery(string query, IRequestValueComparer comparer)
		{
			return AddRestrictions(new RefererUrlQueryRestriction(query, comparer));
		}

		public Route RestrictByRefererUrlQueries(IEnumerable<string> queries)
		{
			queries.ThrowIfNull("queries");

			return AddRestrictions(queries.Select(arg => new RefererUrlQueryRestriction(arg, CaseInsensitivePlainComparer.Instance)));
		}

		public Route RestrictByRefererUrlQueries(params string[] queries)
		{
			return RestrictByRefererUrlQueries((IEnumerable<string>)queries);
		}

		public Route RestrictByRefererUrlQueries(IEnumerable<string> queries, IRequestValueComparer comparer)
		{
			queries.ThrowIfNull("queries");

			return AddRestrictions(queries.Select(arg => new RefererUrlQueryRestriction(arg, comparer)));
		}

		public Route RestrictByRefererUrlQueryString(string field, string value)
		{
			return AddRestrictions(new RefererUrlQueryStringRestriction(field, CaseInsensitivePlainComparer.Instance, value, CaseInsensitivePlainComparer.Instance));
		}

		public Route RestrictByRefererUrlQueryString(string field, IRequestValueComparer fieldComparer, string value, IRequestValueComparer valueComparer)
		{
			return AddRestrictions(new RefererUrlQueryStringRestriction(field, fieldComparer, value, valueComparer));
		}

		public Route RestrictByRefererUrlScheme(string scheme, IRequestValueComparer comparer)
		{
			return AddRestrictions(new RefererUrlSchemeRestriction(scheme, comparer));
		}

		public Route RestrictByRefererUrlSchemes(IEnumerable<string> schemes)
		{
			schemes.ThrowIfNull("schemes");

			return AddRestrictions(schemes.Select(arg => new RefererUrlSchemeRestriction(arg, CaseInsensitivePlainComparer.Instance)));
		}

		public Route RestrictByRefererUrlSchemes(IEnumerable<string> schemes, IRequestValueComparer comparer)
		{
			schemes.ThrowIfNull("schemes");

			return AddRestrictions(schemes.Select(arg => new RefererUrlSchemeRestriction(arg, comparer)));
		}

		public Route RestrictByRefererUrlSchemes(params string[] schemes)
		{
			return RestrictByRefererUrlSchemes((IEnumerable<string>)schemes);
		}

		public Route RestrictByUrl(Func<Uri, bool> matchDelegate)
		{
			return AddRestrictions(new UrlRestriction(matchDelegate));
		}

		public Route RestrictByUrlAuthority(string authority, IRequestValueComparer comparer)
		{
			return AddRestrictions(new UrlAuthorityRestriction(authority, comparer));
		}

		public Route RestrictByUrlAuthorities(IEnumerable<string> authorities)
		{
			authorities.ThrowIfNull("authorities");

			return AddRestrictions(authorities.Select(arg => new UrlAuthorityRestriction(arg, CaseInsensitivePlainComparer.Instance)));
		}

		public Route RestrictByUrlAuthorities(params string[] authorities)
		{
			return RestrictByUrlAuthorities((IEnumerable<string>)authorities);
		}

		public Route RestrictByUrlAuthorities(IEnumerable<string> authorities, IRequestValueComparer comparer)
		{
			authorities.ThrowIfNull("authorities");

			return AddRestrictions(authorities.Select(arg => new UrlAuthorityRestriction(arg, comparer)));
		}

		public Route RestrictByUrlFragment(string fragment, IRequestValueComparer comparer)
		{
			return AddRestrictions(new UrlFragmentRestriction(fragment, comparer));
		}

		public Route RestrictByUrlFragments(IEnumerable<string> fragments)
		{
			fragments.ThrowIfNull("fragments");

			return AddRestrictions(fragments.Select(arg => new UrlFragmentRestriction(arg, CaseInsensitivePlainComparer.Instance)));
		}

		public Route RestrictByUrlFragments(params string[] fragments)
		{
			return RestrictByUrlFragments((IEnumerable<string>)fragments);
		}

		public Route RestrictByUrlFragments(IEnumerable<string> fragments, IRequestValueComparer comparer)
		{
			fragments.ThrowIfNull("fragments");

			return AddRestrictions(fragments.Select(arg => new UrlFragmentRestriction(arg, comparer)));
		}

		public Route RestrictByUrlHost(string host, IRequestValueComparer comparer)
		{
			return AddRestrictions(new UrlHostRestriction(host, comparer));
		}

		public Route RestrictByUrlHosts(IEnumerable<string> hosts)
		{
			hosts.ThrowIfNull("hosts");

			return AddRestrictions(hosts.Select(arg => new UrlHostRestriction(arg, CaseInsensitivePlainComparer.Instance)));
		}

		public Route RestrictByUrlHosts(params string[] hosts)
		{
			return RestrictByUrlHosts((IEnumerable<string>)hosts);
		}

		public Route RestrictByUrlHosts(IEnumerable<string> hosts, IRequestValueComparer comparer)
		{
			hosts.ThrowIfNull("hosts");

			return AddRestrictions(hosts.Select(arg => new UrlHostRestriction(arg, comparer)));
		}

		public Route RestrictByUrlHostTypes(IEnumerable<UriHostNameType> types)
		{
			types.ThrowIfNull("types");

			return AddRestrictions(types.Select(arg => new UrlHostTypeRestriction(arg)));
		}

		public Route RestrictByUrlHostTypes(params UriHostNameType[] types)
		{
			return RestrictByUrlHostTypes((IEnumerable<UriHostNameType>)types);
		}

		public Route RestrictByUrlPorts(IEnumerable<ushort> ports)
		{
			ports.ThrowIfNull("ports");

			return AddRestrictions(ports.Select(arg => new UrlPortRestriction(arg)));
		}

		public Route RestrictByUrlPorts(params ushort[] ports)
		{
			return RestrictByUrlPorts((IEnumerable<ushort>)ports);
		}

		public Route RestrictByUrlQuery(string query, IRequestValueComparer comparer)
		{
			return AddRestrictions(new UrlQueryRestriction(query, comparer));
		}

		public Route RestrictByUrlQueries(IEnumerable<string> queries)
		{
			queries.ThrowIfNull("queries");

			return AddRestrictions(queries.Select(arg => new UrlQueryRestriction(arg, CaseInsensitivePlainComparer.Instance)));
		}

		public Route RestrictByUrlQueries(params string[] queries)
		{
			return RestrictByUrlQueries((IEnumerable<string>)queries);
		}

		public Route RestrictByUrlQueries(IEnumerable<string> queries, IRequestValueComparer comparer)
		{
			queries.ThrowIfNull("queries");

			return AddRestrictions(queries.Select(arg => new UrlQueryRestriction(arg, comparer)));
		}

		public Route RestrictByUrlQueryString(string field, string value)
		{
			return AddRestrictions(new UrlQueryStringRestriction(field, CaseInsensitivePlainComparer.Instance, value, CaseInsensitivePlainComparer.Instance));
		}

		public Route RestrictByUrlQueryString(string field, IRequestValueComparer fieldComparer, string value, IRequestValueComparer valueComparer)
		{
			return AddRestrictions(new UrlQueryStringRestriction(field, fieldComparer, value, valueComparer));
		}

		public Route RestrictByUrlRelativePath(string relativePath, IRequestValueComparer comparer, IHttpRuntime httpRuntime)
		{
			return AddRestrictions(new UrlRelativePathRestriction(relativePath, comparer, httpRuntime));
		}

		public Route RestrictByUrlRelativePaths(IEnumerable<string> relativePaths, IHttpRuntime httpRuntime)
		{
			relativePaths.ThrowIfNull("relativePaths");

			return AddRestrictions(relativePaths.Select(arg => new UrlRelativePathRestriction(arg, CaseInsensitivePlainComparer.Instance, httpRuntime)));
		}

		public Route RestrictByUrlRelativePaths(IEnumerable<string> relativePaths, IRequestValueComparer comparer, IHttpRuntime httpRuntime)
		{
			relativePaths.ThrowIfNull("relativePaths");

			return AddRestrictions(relativePaths.Select(arg => new UrlRelativePathRestriction(arg, comparer, httpRuntime)));
		}

		public Route RestrictByUrlScheme(string scheme, IRequestValueComparer comparer)
		{
			return AddRestrictions(new UrlSchemeRestriction(scheme, comparer));
		}

		public Route RestrictByUrlSchemes(IEnumerable<string> schemes)
		{
			schemes.ThrowIfNull("schemes");

			return AddRestrictions(schemes.Select(arg => new UrlSchemeRestriction(arg, CaseInsensitivePlainComparer.Instance)));
		}

		public Route RestrictByUrlSchemes(IEnumerable<string> schemes, IRequestValueComparer comparer)
		{
			schemes.ThrowIfNull("schemes");

			return AddRestrictions(schemes.Select(arg => new UrlSchemeRestriction(arg, comparer)));
		}

		public Route RestrictByUrlSchemes(params string[] schemes)
		{
			return RestrictByUrlSchemes((IEnumerable<string>)schemes);
		}

		public Route AddRestrictions(IEnumerable<IRestriction> restrictions)
		{
			restrictions.ThrowIfNull("restrictions");

			foreach (IRestriction restriction in restrictions)
			{
				Type restrictionType = restriction.GetType();
				HashSet<IRestriction> restrictionHashSet;

				if (!_restrictionsByRestrictionType.TryGetValue(restrictionType, out restrictionHashSet))
				{
					restrictionHashSet = new HashSet<IRestriction>();
					_restrictionsByRestrictionType.Add(restrictionType, restrictionHashSet);
				}

				restrictionHashSet.Add(restriction);
			}

			return this;
		}

		public Route AddRestrictions(params IRestriction[] restrictions)
		{
			return AddRestrictions((IEnumerable<IRestriction>)restrictions);
		}

		public bool HasRestrictions<T>()
			where T : IRestriction
		{
			return _restrictionsByRestrictionType.ContainsKey(typeof(T));
		}

		public bool HasRestrictions(Type restrictionType)
		{
			restrictionType.ThrowIfNull("restrictionType");

			return _restrictionsByRestrictionType.ContainsKey(restrictionType);
		}

		public bool HasGenericRestrictions(Type restrictionGenericTypeDefinition)
		{
			restrictionGenericTypeDefinition.ThrowIfNull("restrictionGenericTypeDefinition");

			return _restrictionsByRestrictionType.Keys.Any(arg => arg.IsGenericType && arg.GetGenericTypeDefinition() == restrictionGenericTypeDefinition);
		}

		public IEnumerable<IRestriction> GetRestrictions()
		{
			return _restrictionsByRestrictionType.Values.SelectMany(arg => arg);
		}

		public IEnumerable<T> GetRestrictions<T>()
			where T : IRestriction
		{
			HashSet<IRestriction> restrictions;

			return _restrictionsByRestrictionType.TryGetValue(typeof(T), out restrictions) ? restrictions.Cast<T>() : Enumerable.Empty<T>();
		}

		public IEnumerable GetRestrictions(Type restrictionType)
		{
			restrictionType.ThrowIfNull("restrictionType");

			HashSet<IRestriction> restrictions;

			return _restrictionsByRestrictionType.TryGetValue(restrictionType, out restrictions) ? restrictions : Enumerable.Empty<IRestriction>();
		}

		public IEnumerable GetGenericRestrictions(Type restrictionGenericTypeDefinition)
		{
			restrictionGenericTypeDefinition.ThrowIfNull("restrictionGenericTypeDefinition");

			return _restrictionsByRestrictionType.Keys
				.Where(arg => arg.IsGenericType && arg.GetGenericTypeDefinition() == restrictionGenericTypeDefinition)
				.SelectMany(arg => _restrictionsByRestrictionType[arg]);
		}

		public IEnumerable<Type> GetRestrictionTypes()
		{
			return _restrictionsByRestrictionType.Keys;
		}

		public void ClearRestrictions()
		{
			_restrictionsByRestrictionType.Clear();
		}

		#endregion

		#region Authentication

		public Route AuthenticationProvider(IAuthenticationProvider provider)
		{
			provider.ThrowIfNull("provider");

			_authenticationProvider = provider;

			return this;
		}

		public Route NoAuthenticationProvider()
		{
			_authenticationProvider = null;

			return this;
		}

		#endregion

		#region Responses

		public Route RespondWith<T>(Func<HttpContextBase, T> responseDelegate, Type returnType)
			where T : class, IResponse
		{
			Type delegateType = typeof(T);

			if (returnType != delegateType && delegateType.IsInterface && !returnType.ImplementsInterface<T>())
			{
				throw new ArgumentException(String.Format("Return type must implement {0}.", delegateType.FullName), "returnType");
			}
			if (returnType != delegateType && !delegateType.IsInterface && !returnType.IsSubclassOf(delegateType))
			{
				throw new ArgumentException(String.Format("Return type must derive {0}.", delegateType.FullName), "returnType");
			}

			_responseDelegate = context => responseDelegate(context).AsCompletedTask<IResponse>();
			ResponseType = returnType;

			return this;
		}

		public Route RespondWith<T>(Func<HttpContextBase, T> responseDelegate)
			where T : class, IResponse
		{
			return RespondWith(responseDelegate, typeof(T));
		}

		public Route RespondWith<T>(T response, Type returnType)
			where T : class, IResponse
		{
			return RespondWith(context => response, returnType);
		}

		public Route RespondWith<T>(T response)
			where T : class, IResponse
		{
			return RespondWith(context => response);
		}

		public Route RespondWith<T>(Func<HttpContextBase, Task<T>> responseDelegate, Type returnType)
			where T : class, IResponse
		{
			Type delegateType = typeof(T);

			if (returnType != delegateType && delegateType.IsInterface && !returnType.ImplementsInterface<T>())
			{
				throw new ArgumentException(String.Format("Return type must implement {0}.", delegateType.FullName), "returnType");
			}
			if (returnType != delegateType && !delegateType.IsInterface && !returnType.IsSubclassOf(delegateType))
			{
				throw new ArgumentException(String.Format("Return type must derive {0}.", delegateType.FullName), "returnType");
			}

			_responseDelegate = async context => await responseDelegate(context);
			ResponseType = returnType;

			return this;
		}

		public Route RespondWith<T>(Func<HttpContextBase, Task<T>> responseDelegate)
			where T : class, IResponse
		{
			return RespondWith(responseDelegate, typeof(T));
		}

		public Route RespondWith<T>(Task<T> response, Type returnType)
			where T : class, IResponse
		{
			return RespondWith(context => response, returnType);
		}

		public Route RespondWith<T>(Task<T> response)
			where T : class, IResponse
		{
			return RespondWith(context => response);
		}

		public Route RespondWithNoContent(Action<HttpContextBase> @delegate)
		{
			@delegate.ThrowIfNull("delegate");

			_responseDelegate = context =>
			{
				@delegate(context);

				return new Response().NoContent().AsCompletedTask<IResponse>();
			};
			ResponseType = null;

			return this;
		}

		public Route RespondWithNoContent()
		{
			_responseDelegate = context => new Response().NoContent().AsCompletedTask<IResponse>();
			ResponseType = null;

			return this;
		}

		#endregion

		public async Task<MatchResult> MatchesRequestAsync(HttpRequestBase request)
		{
			request.ThrowIfNull("request");

			IRestriction[] restrictions = GetRestrictions().ToArray();
			var matchingRestrictions = new List<IRestriction>();

			foreach (IRestriction restriction in restrictions)
			{
				if (await restriction.MatchesRequestAsync(request))
				{
					matchingRestrictions.Add(restriction);
				}
			}

			return restrictions.Length == matchingRestrictions.Count ? MatchResult.RouteMatched(matchingRestrictions, _id.ToString()) : MatchResult.RouteNotMatched(matchingRestrictions, restrictions.Except(matchingRestrictions));
		}

		public async Task<AuthenticateResult> AuthenticateAsync(HttpRequestBase request, HttpResponseBase response)
		{
			request.ThrowIfNull("request");
			response.ThrowIfNull("response");

			if (_authenticationProvider == null)
			{
				return AuthenticateResult.NoAuthenticationPerformed();
			}

			AuthenticationResult result = await _authenticationProvider.AuthenticateAsync(request, response, this);

			return result == AuthenticationResult.AuthenticationSucceeded
				? AuthenticateResult.AuthenticationSucceeded()
				: AuthenticateResult.AuthenticationFailed(await _authenticationProvider.GetFailedAuthenticationResponseAsync(request));
		}

		public Task<IResponse> ProcessResponseAsync(HttpContextBase context)
		{
			context.ThrowIfNull("context");

			return _responseDelegate(context);
		}
	}
}
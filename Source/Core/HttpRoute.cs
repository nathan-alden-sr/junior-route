using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

using Junior.Common;

using NathanAlden.JuniorRouting.Core.RequestValueComparers;
using NathanAlden.JuniorRouting.Core.Responses;
using NathanAlden.JuniorRouting.Core.Restrictions;

namespace NathanAlden.JuniorRouting.Core
{
	[DebuggerDisplay(@"Restrictions: {RestrictionTypeNames}")]
	public class HttpRoute
	{
		private readonly Dictionary<Type, List<IHttpRouteRestriction>> _restrictionsByRestrictionType = new Dictionary<Type, List<IHttpRouteRestriction>>();
		private Func<IHttpRouteResponse> _responseDelegate = () => NoContentResponse.Default;

		public HttpRoute(params IHttpRouteRestriction[] restrictions)
		{
			IEnumerable<IGrouping<Type, IHttpRouteRestriction>> restrictionsByRestrictionType = (restrictions ?? new IHttpRouteRestriction[0]).GroupBy(restriction => restriction.GetType());

			foreach (IGrouping<Type, IHttpRouteRestriction> groupedRestrictions in restrictionsByRestrictionType)
			{
				_restrictionsByRestrictionType.Add(groupedRestrictions.Key, new List<IHttpRouteRestriction>(groupedRestrictions));
			}
		}

		public IEnumerable<Type> RestrictionTypes
		{
			get
			{
				return _restrictionsByRestrictionType.Keys;
			}
		}

		public IEnumerable<IHttpRouteRestriction> Restrictions
		{
			get
			{
				return _restrictionsByRestrictionType.SelectMany(arg => arg.Value);
			}
		}

		// ReSharper disable UnusedMember.Local
		private string RestrictionTypeNames
			// ReSharper restore UnusedMember.Local
		{
			get
			{
				return String.Join(", ", Restrictions.Select(arg => arg.GetType().Name));
			}
		}

		public HttpRoute Cookie(string name, string value)
		{
			name.ThrowIfNull("name");
			value.ThrowIfNull("value");

			AddRestriction(new CookieRestriction(name, CaseInsensitivePlainRequestValueComparer.Default, value, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute Cookie(string name, IRequestValueComparer nameComparer, string value, IRequestValueComparer valueComparer)
		{
			name.ThrowIfNull("name");
			nameComparer.ThrowIfNull("nameComparer");
			value.ThrowIfNull("value");
			valueComparer.ThrowIfNull("valueComparer");

			AddRestriction(new CookieRestriction(name, nameComparer, value, valueComparer));

			return this;
		}

		public HttpRoute Header(string field, string value)
		{
			field.ThrowIfNull("field");
			value.ThrowIfNull("value");

			AddRestriction(new HeaderRestriction(field, value, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute Header(string field, string value, IRequestValueComparer valueComparer)
		{
			field.ThrowIfNull("field");
			value.ThrowIfNull("value");
			valueComparer.ThrowIfNull("valueComparer");

			AddRestriction(new HeaderRestriction(field, value, valueComparer));

			return this;
		}

		public HttpRoute HeaderWithCaseInsensitiveValueComparer(string field, params string[] values)
		{
			field.ThrowIfNull("field");
			values.ThrowIfNull("values");

			if (values.Length == 0)
			{
				throw new ArgumentException("Must provide at least 1 value.", "values");
			}

			AddRestriction(new HeaderRestriction(field, BuildValueRegex(values), CaseInsensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute HeaderWithCaseInsensitivePlainValueComparer(string field, string value)
		{
			field.ThrowIfNull("field");
			value.ThrowIfNull("value");

			AddRestriction(new HeaderRestriction(field, value, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute HeaderWithCaseInsensitiveRegexValueComparer(string field, string value)
		{
			field.ThrowIfNull("field");
			value.ThrowIfNull("value");

			AddRestriction(new HeaderRestriction(field, value, CaseInsensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute HeaderWithCaseSensitiveValueComparer(string field, params string[] values)
		{
			field.ThrowIfNull("field");
			values.ThrowIfNull("values");

			if (values.Length == 0)
			{
				throw new ArgumentException("Must provide at least 1 value.", "values");
			}

			AddRestriction(new HeaderRestriction(field, BuildValueRegex(values), CaseSensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute HeaderWithCaseSensitivePlainValueComparer(string field, string value)
		{
			field.ThrowIfNull("field");
			value.ThrowIfNull("value");

			AddRestriction(new HeaderRestriction(field, value, CaseSensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute HeaderWithCaseSensitiveRegexValueComparer(string field, string value)
		{
			field.ThrowIfNull("field");
			value.ThrowIfNull("value");

			AddRestriction(new HeaderRestriction(field, value, CaseSensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute Method(HttpMethod method)
		{
			AddRestriction(new MethodRestriction(method.ToString().ToUpperInvariant()));

			return this;
		}

		public HttpRoute Method(string method)
		{
			AddRestriction(new MethodRestriction(method.ToUpperInvariant()));

			return this;
		}

		public HttpRoute Methods(params HttpMethod[] methods)
		{
			methods.ThrowIfNull("methods");

			foreach (HttpMethod method in methods)
			{
				AddRestriction(new MethodRestriction(method.ToString().ToUpperInvariant()));
			}

			return this;
		}

		public HttpRoute Methods(params string[] methods)
		{
			methods.ThrowIfNull("methods");

			foreach (string method in methods)
			{
				AddRestriction(new MethodRestriction(method.ToUpperInvariant()));
			}

			return this;
		}

		public HttpRoute Get()
		{
			return Method(HttpMethod.Get);
		}

		public HttpRoute Head()
		{
			return Method(HttpMethod.Head);
		}

		public HttpRoute Post()
		{
			return Method(HttpMethod.Post);
		}

		public HttpRoute Put()
		{
			return Method(HttpMethod.Put);
		}

		public HttpRoute Delete()
		{
			return Method(HttpMethod.Delete);
		}

		public HttpRoute Trace()
		{
			return Method(HttpMethod.Trace);
		}

		public HttpRoute Connect()
		{
			return Method(HttpMethod.Connect);
		}

		public HttpRoute RefererUrl(Uri url, RefererUrlRestriction.RefererUrlMatchDelegate matchDelegate)
		{
			url.ThrowIfNull("url");
			matchDelegate.ThrowIfNull("matchDelegate");

			AddRestriction(new RefererUrlRestriction(url, matchDelegate));

			return this;
		}

		public HttpRoute RefererUrlAuthority(string authority)
		{
			authority.ThrowIfNull("authority");

			AddRestriction(new RefererUrlAuthorityRestriction(authority));

			return this;
		}

		public HttpRoute RefererUrlAuthorities(params string[] authorities)
		{
			authorities.ThrowIfNull("authorities");

			foreach (string authority in authorities)
			{
				AddRestriction(new RefererUrlAuthorityRestriction(authority));
			}

			return this;
		}

		public HttpRoute RefererUrlFragment(string fragment)
		{
			fragment.ThrowIfNull("fragment");

			AddRestriction(new RefererUrlFragmentRestriction(fragment, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlFragment(string fragment, IRequestValueComparer comparer)
		{
			fragment.ThrowIfNull("fragment");
			comparer.ThrowIfNull("comparer");

			AddRestriction(new RefererUrlFragmentRestriction(fragment, comparer));

			return this;
		}

		public HttpRoute RefererUrlFragmentWithCaseInsensitivePlainComparer(string fragment)
		{
			fragment.ThrowIfNull("fragment");

			AddRestriction(new RefererUrlFragmentRestriction(fragment, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlFragmentWithCaseInsensitiveRegexComparer(string fragment)
		{
			fragment.ThrowIfNull("fragment");

			AddRestriction(new RefererUrlFragmentRestriction(fragment, CaseInsensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlFragmentWithCaseSensitivePlainComparer(string fragment)
		{
			fragment.ThrowIfNull("fragment");

			AddRestriction(new RefererUrlFragmentRestriction(fragment, CaseSensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlFragmentWithCaseSensitiveRegexComparer(string fragment)
		{
			fragment.ThrowIfNull("fragment");

			AddRestriction(new RefererUrlFragmentRestriction(fragment, CaseSensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlHost(string host)
		{
			host.ThrowIfNull("host");

			AddRestriction(new RefererUrlHostRestriction(host));

			return this;
		}

		public HttpRoute RefererUrlHosts(params string[] hosts)
		{
			hosts.ThrowIfNull("hosts");

			foreach (string host in hosts)
			{
				AddRestriction(new RefererUrlHostRestriction(host));
			}

			return this;
		}

		public HttpRoute RefererUrlHostType(UriHostNameType type)
		{
			AddRestriction(new RefererUrlHostTypeRestriction(type));

			return this;
		}

		public HttpRoute RefererUrlHostTypes(params UriHostNameType[] types)
		{
			types.ThrowIfNull("types");

			foreach (UriHostNameType type in types)
			{
				AddRestriction(new RefererUrlHostTypeRestriction(type));
			}

			return this;
		}

		public HttpRoute RefererUrlLocalPath(string localPath)
		{
			localPath.ThrowIfNull("localPath");

			AddRestriction(new RefererUrlLocalPathRestriction(localPath, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlLocalPath(string localPath, IRequestValueComparer comparer)
		{
			localPath.ThrowIfNull("localPath");
			comparer.ThrowIfNull("comparer");

			AddRestriction(new RefererUrlLocalPathRestriction(localPath, comparer));

			return this;
		}

		public HttpRoute RefererUrlLocalPathWithCaseInsensitivePlainComparer(string localPath)
		{
			localPath.ThrowIfNull("localPath");

			AddRestriction(new RefererUrlLocalPathRestriction(localPath, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlLocalPathWithCaseInsensitiveRegexComparer(string localPath)
		{
			localPath.ThrowIfNull("localPath");

			AddRestriction(new RefererUrlLocalPathRestriction(localPath, CaseInsensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlLocalPathWithCaseSensitivePlainComparer(string localPath)
		{
			localPath.ThrowIfNull("localPath");

			AddRestriction(new RefererUrlLocalPathRestriction(localPath, CaseSensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlLocalPathWithCaseSensitiveRegexComparer(string localPath)
		{
			localPath.ThrowIfNull("localPath");

			AddRestriction(new RefererUrlLocalPathRestriction(localPath, CaseSensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlPathAndQuery(string pathAndQuery)
		{
			pathAndQuery.ThrowIfNull("pathAndQuery");

			AddRestriction(new RefererUrlPathAndQueryRestriction(pathAndQuery, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlPathAndQuery(string pathAndQuery, IRequestValueComparer comparer)
		{
			pathAndQuery.ThrowIfNull("pathAndQuery");
			comparer.ThrowIfNull("comparer");

			AddRestriction(new RefererUrlPathAndQueryRestriction(pathAndQuery, comparer));

			return this;
		}

		public HttpRoute RefererUrlPathAndQueryWithCaseInsensitivePlainComparer(string pathAndQuery)
		{
			pathAndQuery.ThrowIfNull("pathAndQuery");

			AddRestriction(new RefererUrlPathAndQueryRestriction(pathAndQuery, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlPathAndQueryWithCaseInsensitiveRegexComparer(string pathAndQuery)
		{
			pathAndQuery.ThrowIfNull("pathAndQuery");

			AddRestriction(new RefererUrlPathAndQueryRestriction(pathAndQuery, CaseInsensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlPathAndQueryWithCaseSensitivePlainComparer(string pathAndQuery)
		{
			pathAndQuery.ThrowIfNull("pathAndQuery");

			AddRestriction(new RefererUrlPathAndQueryRestriction(pathAndQuery, CaseSensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlPathAndQueryWithCaseSensitiveRegexComparer(string pathAndQuery)
		{
			pathAndQuery.ThrowIfNull("pathAndQuery");

			AddRestriction(new RefererUrlPathAndQueryRestriction(pathAndQuery, CaseSensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlPort(ushort port)
		{
			AddRestriction(new RefererUrlPortRestriction(port));

			return this;
		}

		public HttpRoute RefererUrlPorts(params ushort[] ports)
		{
			ports.ThrowIfNull("ports");

			foreach (ushort port in ports)
			{
				AddRestriction(new RefererUrlPortRestriction(port));
			}

			return this;
		}

		public HttpRoute RefererUrlQuery(string query)
		{
			query.ThrowIfNull("query");

			AddRestriction(new RefererUrlQueryRestriction(query, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlQuery(string query, IRequestValueComparer comparer)
		{
			query.ThrowIfNull("query");
			comparer.ThrowIfNull("comparer");

			AddRestriction(new RefererUrlQueryRestriction(query, comparer));

			return this;
		}

		public HttpRoute RefererUrlQueryWithCaseInsensitivePlainComparer(string query)
		{
			query.ThrowIfNull("query");

			AddRestriction(new RefererUrlQueryRestriction(query, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlQueryWithCaseInsensitiveRegexComparer(string query)
		{
			query.ThrowIfNull("query");

			AddRestriction(new RefererUrlQueryRestriction(query, CaseInsensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlQueryWithCaseSensitivePlainComparer(string query)
		{
			query.ThrowIfNull("query");

			AddRestriction(new RefererUrlQueryRestriction(query, CaseSensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlQueryWithCaseSensitiveRegexComparer(string query)
		{
			query.ThrowIfNull("query");

			AddRestriction(new RefererUrlQueryRestriction(query, CaseSensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlQueryString(string field, string value)
		{
			field.ThrowIfNull("field");
			value.ThrowIfNull("value");

			AddRestriction(new RefererUrlQueryStringRestriction(field, CaseInsensitivePlainRequestValueComparer.Default, value, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RefererUrlQueryString(string field, IRequestValueComparer fieldComparer, string value, IRequestValueComparer valueComparer)
		{
			field.ThrowIfNull("field");
			fieldComparer.ThrowIfNull("fieldComparer");
			value.ThrowIfNull("value");
			valueComparer.ThrowIfNull("valueComparer");

			AddRestriction(new RefererUrlQueryStringRestriction(field, fieldComparer, value, valueComparer));

			return this;
		}

		public HttpRoute RefererUrlScheme(string scheme)
		{
			scheme.ThrowIfNull("scheme");

			AddRestriction(new RefererUrlSchemeRestriction(scheme));

			return this;
		}

		public HttpRoute RefererUrlSchemes(params string[] schemes)
		{
			schemes.ThrowIfNull("schemes");

			foreach (string scheme in schemes)
			{
				AddRestriction(new RefererUrlSchemeRestriction(scheme));
			}

			return this;
		}

		public HttpRoute RelativeUrl(string relativeUrl)
		{
			relativeUrl.ThrowIfNull("relativeUrl");

			AddRestriction(new RelativeUrlRestriction(relativeUrl, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RelativeUrl(string relativeUrl, IRequestValueComparer comparer)
		{
			relativeUrl.ThrowIfNull("relativeUrl");
			comparer.ThrowIfNull("comparer");

			AddRestriction(new RelativeUrlRestriction(relativeUrl, comparer));

			return this;
		}

		public HttpRoute RelativeUrlWithCaseInsensitivePlainComparer(string relativeUrl)
		{
			relativeUrl.ThrowIfNull("relativeUrl");

			AddRestriction(new RelativeUrlRestriction(relativeUrl, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RelativeUrlWithCaseInsensitiveRegexComparer(string relativeUrl)
		{
			relativeUrl.ThrowIfNull("relativeUrl");

			AddRestriction(new RelativeUrlRestriction(relativeUrl, CaseInsensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RelativeUrlWithCaseSensitivePlainComparer(string relativeUrl)
		{
			relativeUrl.ThrowIfNull("relativeUrl");

			AddRestriction(new RelativeUrlRestriction(relativeUrl, CaseSensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute RelativeUrlWithCaseSensitiveRegexComparer(string relativeUrl)
		{
			relativeUrl.ThrowIfNull("relativeUrl");

			AddRestriction(new RelativeUrlRestriction(relativeUrl, CaseSensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute Url(Uri url, UrlRestriction.UrlMatchDelegate matchDelegate)
		{
			url.ThrowIfNull("url");
			matchDelegate.ThrowIfNull("matchDelegate");

			AddRestriction(new UrlRestriction(url, matchDelegate));

			return this;
		}

		public HttpRoute UrlAuthority(string authority)
		{
			authority.ThrowIfNull("authority");

			AddRestriction(new UrlAuthorityRestriction(authority));

			return this;
		}

		public HttpRoute UrlAuthorities(params string[] authorities)
		{
			authorities.ThrowIfNull("authorities");

			foreach (string authority in authorities)
			{
				AddRestriction(new UrlAuthorityRestriction(authority));
			}

			return this;
		}

		public HttpRoute UrlFragment(string fragment)
		{
			fragment.ThrowIfNull("fragment");

			AddRestriction(new UrlFragmentRestriction(fragment, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlFragment(string fragment, IRequestValueComparer comparer)
		{
			fragment.ThrowIfNull("fragment");
			comparer.ThrowIfNull("comparer");

			AddRestriction(new UrlFragmentRestriction(fragment, comparer));

			return this;
		}

		public HttpRoute UrlFragmentWithCaseInsensitivePlainComparer(string fragment)
		{
			fragment.ThrowIfNull("fragment");

			AddRestriction(new UrlFragmentRestriction(fragment, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlFragmentWithCaseInsensitiveRegexComparer(string fragment)
		{
			fragment.ThrowIfNull("fragment");

			AddRestriction(new UrlFragmentRestriction(fragment, CaseInsensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlFragmentWithCaseSensitivePlainComparer(string fragment)
		{
			fragment.ThrowIfNull("fragment");

			AddRestriction(new UrlFragmentRestriction(fragment, CaseSensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlFragmentWithCaseSensitiveRegexComparer(string fragment)
		{
			fragment.ThrowIfNull("fragment");

			AddRestriction(new UrlFragmentRestriction(fragment, CaseSensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlHost(string host)
		{
			host.ThrowIfNull("host");

			AddRestriction(new UrlHostRestriction(host));

			return this;
		}

		public HttpRoute UrlHosts(params string[] hosts)
		{
			hosts.ThrowIfNull("hosts");

			foreach (string host in hosts)
			{
				AddRestriction(new UrlHostRestriction(host));
			}

			return this;
		}

		public HttpRoute UrlHostType(UriHostNameType type)
		{
			AddRestriction(new UrlHostTypeRestriction(type));

			return this;
		}

		public HttpRoute UrlHostTypes(params UriHostNameType[] types)
		{
			types.ThrowIfNull("types");

			foreach (UriHostNameType type in types)
			{
				AddRestriction(new UrlHostTypeRestriction(type));
			}

			return this;
		}

		public HttpRoute UrlLocalPath(string localPath)
		{
			localPath.ThrowIfNull("localPath");

			AddRestriction(new UrlLocalPathRestriction(localPath, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlLocalPath(string localPath, IRequestValueComparer comparer)
		{
			localPath.ThrowIfNull("localPath");
			comparer.ThrowIfNull("comparer");

			AddRestriction(new UrlLocalPathRestriction(localPath, comparer));

			return this;
		}

		public HttpRoute UrlLocalPathWithCaseInsensitivePlainComparer(string localPath)
		{
			localPath.ThrowIfNull("localPath");

			AddRestriction(new UrlLocalPathRestriction(localPath, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlLocalPathWithCaseInsensitiveRegexComparer(string localPath)
		{
			localPath.ThrowIfNull("localPath");

			AddRestriction(new UrlLocalPathRestriction(localPath, CaseInsensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlLocalPathWithCaseSensitivePlainComparer(string localPath)
		{
			localPath.ThrowIfNull("localPath");

			AddRestriction(new UrlLocalPathRestriction(localPath, CaseSensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlLocalPathWithCaseSensitiveRegexComparer(string localPath)
		{
			localPath.ThrowIfNull("localPath");

			AddRestriction(new UrlLocalPathRestriction(localPath, CaseSensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlPathAndQuery(string pathAndQuery)
		{
			pathAndQuery.ThrowIfNull("pathAndQuery");

			AddRestriction(new UrlPathAndQueryRestriction(pathAndQuery, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlPathAndQuery(string pathAndQuery, IRequestValueComparer comparer)
		{
			pathAndQuery.ThrowIfNull("pathAndQuery");
			comparer.ThrowIfNull("comparer");

			AddRestriction(new UrlPathAndQueryRestriction(pathAndQuery, comparer));

			return this;
		}

		public HttpRoute UrlPathAndQueryWithCaseInsensitivePlainComparer(string pathAndQuery)
		{
			pathAndQuery.ThrowIfNull("pathAndQuery");

			AddRestriction(new UrlPathAndQueryRestriction(pathAndQuery, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlPathAndQueryWithCaseInsensitiveRegexComparer(string pathAndQuery)
		{
			pathAndQuery.ThrowIfNull("pathAndQuery");

			AddRestriction(new UrlPathAndQueryRestriction(pathAndQuery, CaseInsensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlPathAndQueryWithCaseSensitivePlainComparer(string pathAndQuery)
		{
			pathAndQuery.ThrowIfNull("pathAndQuery");

			AddRestriction(new UrlPathAndQueryRestriction(pathAndQuery, CaseSensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlPathAndQueryWithCaseSensitiveRegexComparer(string pathAndQuery)
		{
			pathAndQuery.ThrowIfNull("pathAndQuery");

			AddRestriction(new UrlPathAndQueryRestriction(pathAndQuery, CaseSensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlPort(ushort port)
		{
			AddRestriction(new UrlPortRestriction(port));

			return this;
		}

		public HttpRoute UrlPorts(params ushort[] ports)
		{
			ports.ThrowIfNull("ports");

			foreach (ushort port in ports)
			{
				AddRestriction(new UrlPortRestriction(port));
			}

			return this;
		}

		public HttpRoute UrlQuery(string query)
		{
			query.ThrowIfNull("query");

			AddRestriction(new UrlQueryRestriction(query, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlQuery(string query, IRequestValueComparer comparer)
		{
			query.ThrowIfNull("query");
			comparer.ThrowIfNull("comparer");

			AddRestriction(new UrlQueryRestriction(query, comparer));

			return this;
		}

		public HttpRoute UrlQueryWithCaseInsensitivePlainComparer(string query)
		{
			query.ThrowIfNull("query");

			AddRestriction(new UrlQueryRestriction(query, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlQueryWithCaseInsensitiveRegexComparer(string query)
		{
			query.ThrowIfNull("query");

			AddRestriction(new UrlQueryRestriction(query, CaseInsensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlQueryWithCaseSensitivePlainComparer(string query)
		{
			query.ThrowIfNull("query");

			AddRestriction(new UrlQueryRestriction(query, CaseSensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlQueryWithCaseSensitiveRegexComparer(string query)
		{
			query.ThrowIfNull("query");

			AddRestriction(new UrlQueryRestriction(query, CaseSensitiveRegexRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlQueryString(string key, string value)
		{
			key.ThrowIfNull("key");
			value.ThrowIfNull("value");

			AddRestriction(new UrlQueryStringRestriction(key, CaseInsensitivePlainRequestValueComparer.Default, value, CaseInsensitivePlainRequestValueComparer.Default));

			return this;
		}

		public HttpRoute UrlQueryString(string key, IRequestValueComparer keyComparer, string value, IRequestValueComparer valueComparer)
		{
			key.ThrowIfNull("key");
			keyComparer.ThrowIfNull("keyComparer");
			value.ThrowIfNull("value");
			valueComparer.ThrowIfNull("valueComparer");

			AddRestriction(new UrlQueryStringRestriction(key, keyComparer, value, valueComparer));

			return this;
		}

		public HttpRoute UrlScheme(string scheme)
		{
			scheme.ThrowIfNull("scheme");

			AddRestriction(new UrlSchemeRestriction(scheme));

			return this;
		}

		public HttpRoute UrlSchemes(params string[] schemes)
		{
			schemes.ThrowIfNull("schemes");

			foreach (string scheme in schemes)
			{
				AddRestriction(new UrlSchemeRestriction(scheme));
			}

			return this;
		}

		public HttpRoute Restriction(IHttpRouteRestriction restriction)
		{
			restriction.ThrowIfNull("restriction");

			AddRestriction(restriction);

			return this;
		}

		public HttpRoute Response(Func<IHttpRouteResponse> responseDelegate)
		{
			responseDelegate.ThrowIfNull("response");

			_responseDelegate = responseDelegate;

			return this;
		}

		public HttpRoute Response(IHttpRouteResponse response)
		{
			response.ThrowIfNull("response");

			_responseDelegate = () => response;

			return this;
		}

		public bool MatchesRequest(HttpRequestBase request)
		{
			return Restrictions.All(arg => arg.MatchesRequest(request));
		}

		public bool HasRestrictions<T>()
			where T : IHttpRouteRestriction
		{
			return _restrictionsByRestrictionType.ContainsKey(typeof(T));
		}

		public bool HasRestrictions(Type restrictionType)
		{
			return _restrictionsByRestrictionType.ContainsKey(restrictionType);
		}

		public IEnumerable<T> GetRestrictions<T>()
			where T : IHttpRouteRestriction
		{
			return _restrictionsByRestrictionType[typeof(T)].Cast<T>();
		}

		public IEnumerable GetRestrictions(Type restrictionType)
		{
			return _restrictionsByRestrictionType[restrictionType];
		}

		public IHttpRouteResponse GetResponse()
		{
			return _responseDelegate();
		}

		public static HttpRoute Create(params IHttpRouteRestriction[] restrictions)
		{
			return new HttpRoute(restrictions);
		}

		private void AddRestriction(IHttpRouteRestriction restriction)
		{
			Type restrictionType = restriction.GetType();
			List<IHttpRouteRestriction> restrictionList;

			if (!_restrictionsByRestrictionType.TryGetValue(restrictionType, out restrictionList))
			{
				restrictionList = new List<IHttpRouteRestriction>();
				_restrictionsByRestrictionType.Add(restrictionType, restrictionList);
			}

			restrictionList.Add(restriction);
		}

		private static string BuildValueRegex(IEnumerable<string> values)
		{
			return String.Join("|", values.Select(arg => String.Format("({0})", Regex.Escape(arg))));
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Junior.Common;
using Junior.Route.Assets.FileSystem;
using Junior.Route.AutoRouting.AuthenticationProviders;
using Junior.Route.AutoRouting.AuthenticationStrategies;
using Junior.Route.AutoRouting.ClassFilters;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.FormsAuthentication;
using Junior.Route.AutoRouting.IdMappers;
using Junior.Route.AutoRouting.MethodFilters;
using Junior.Route.AutoRouting.NameMappers;
using Junior.Route.AutoRouting.ParameterMappers;
using Junior.Route.AutoRouting.ResolvedRelativeUrlMappers;
using Junior.Route.AutoRouting.ResponseMappers;
using Junior.Route.AutoRouting.RestrictionMappers;
using Junior.Route.AutoRouting.RestrictionMappers.Attributes;
using Junior.Route.AutoRouting.SchemeMappers;
using Junior.Route.Common;
using Junior.Route.Routing;
using Junior.Route.Routing.AuthenticationProviders;

using DelegateFilter = Junior.Route.AutoRouting.ClassFilters.DelegateFilter;

namespace Junior.Route.AutoRouting
{
	public class AutoRouteCollection
	{
		private readonly HashSet<Func<IRouteCollection, IEnumerable<Routing.Route>>> _additionalRoutes = new HashSet<Func<IRouteCollection, IEnumerable<Routing.Route>>>();
		private readonly HashSet<Assembly> _assemblies = new HashSet<Assembly>();
		private readonly HashSet<IAuthenticationStrategy> _authenticationStrategies = new HashSet<IAuthenticationStrategy>();
		private readonly HashSet<IClassFilter> _classFilters = new HashSet<IClassFilter>();
		private readonly HashSet<IIdMapper> _idMappers = new HashSet<IIdMapper>();
		private readonly HashSet<IMethodFilter> _methodFilters = new HashSet<IMethodFilter>();
		private readonly HashSet<INameMapper> _nameMappers = new HashSet<INameMapper>();
		private readonly HashSet<IResolvedRelativeUrlMapper> _resolvedRelativeUrlMappers = new HashSet<IResolvedRelativeUrlMapper>();
		private readonly HashSet<IRestrictionMapper> _restrictionMappers = new HashSet<IRestrictionMapper>();
		private readonly HashSet<ISchemeMapper> _schemeMappers = new HashSet<ISchemeMapper>();
		private IAuthenticationProvider _authenticationProvider;
		private IContainer _bundleDependencyContainer;
		private bool _duplicateRouteNamesAllowed;
		private IContainer _endpointContainer = new NewInstancePerRouteEndpointContainer();
		private IResponseMapper _responseMapper = new NoContentMapper();
		private IContainer _restrictionContainer;

		public AutoRouteCollection(bool duplicateRouteNamesAllowed = false)
		{
			_duplicateRouteNamesAllowed = duplicateRouteNamesAllowed;
		}

		public AutoRouteCollection DuplicateRouteNamesAllowed()
		{
			_duplicateRouteNamesAllowed = true;

			return this;
		}

		public AutoRouteCollection DuplicateRouteNamesDisallowed()
		{
			_duplicateRouteNamesAllowed = false;

			return this;
		}

		public AutoRouteCollection Assemblies(IEnumerable<Assembly> assemblies)
		{
			assemblies.ThrowIfNull("assemblies");

			_assemblies.AddRange(assemblies);

			return this;
		}

		public AutoRouteCollection Assemblies(params Assembly[] assemblies)
		{
			return Assemblies((IEnumerable<Assembly>)assemblies);
		}

		public AutoRouteCollection NewInstancePerRouteContainer()
		{
			_endpointContainer = new NewInstancePerRouteEndpointContainer();

			return this;
		}

		public AutoRouteCollection SingleInstancePerRouteContainer()
		{
			_endpointContainer = new SingleInstancePerRouteEndpointContainer();

			return this;
		}

		public AutoRouteCollection EndpointContainer(IContainer container)
		{
			container.ThrowIfNull("container");

			_endpointContainer = container;

			return this;
		}

		public AutoRouteCollection BundleDependencyContainer(IContainer container)
		{
			container.ThrowIfNull("container");

			_bundleDependencyContainer = container;

			return this;
		}

		public AutoRouteCollection RestrictionContainer(IContainer container)
		{
			container.ThrowIfNull("container");

			_restrictionContainer = container;

			return this;
		}

		public async Task<IRouteCollection> GenerateRouteCollectionAsync()
		{
			if (!_assemblies.Any())
			{
				throw new InvalidOperationException("At least one assembly must be provided.");
			}
			if (!_nameMappers.Any())
			{
				throw new InvalidOperationException("At least one name mapper must be provided.");
			}
			if (!_idMappers.Any())
			{
				throw new InvalidOperationException("At least one ID mapper must be provided.");
			}
			if (!_resolvedRelativeUrlMappers.Any())
			{
				throw new InvalidOperationException("At least one resolved relative URL mapper must be provided.");
			}
			if (_restrictionMappers.Any() && _restrictionContainer == null)
			{
				throw new InvalidOperationException("Restriction mappers are configured but no restriction container was provided.");
			}

			var routes = new RouteCollection(_duplicateRouteNamesAllowed);
			IEnumerable<Type> matchingTypes = await GetMatchingTypesAsync();

			foreach (Type matchingType in matchingTypes)
			{
				IEnumerable<MethodInfo> matchingMethods = await GetMatchingMethodsAsync(matchingType);

				foreach (MethodInfo matchingMethod in matchingMethods)
				{
					string name = await GetNameAsync(matchingType, matchingMethod);

					if (name == null)
					{
						throw new ApplicationException(String.Format("Unable to determine a route name for '{0}.{1}'.", matchingType.FullName, matchingMethod.Name));
					}

					Guid? id = await GetIdAsync(matchingType, matchingMethod);

					if (id == null)
					{
						throw new ApplicationException(String.Format("Unable to determine a route ID for '{0}.{1}'.", matchingType.FullName, matchingMethod.Name));
					}

					Scheme? scheme = await GetSchemeAsync(matchingType, matchingMethod);

					if (scheme == null)
					{
						throw new ApplicationException(String.Format("Unable to determine a route scheme for '{0}.{1}'.", matchingType.FullName, matchingMethod.Name));
					}

					Type[] ignoredResolvedRelativeUrlMapperTypes = matchingMethod
						.GetCustomAttributes<IgnoreResolvedRelativeUrlMapperTypeAttribute>()
						.SelectMany(arg => arg.IgnoredTypes)
						.ToArray();
					string resolvedRelativeUrl = await GetResolvedRelativeUrlAsync(ignoredResolvedRelativeUrlMapperTypes, matchingType, matchingMethod);

					if (resolvedRelativeUrl == null)
					{
						throw new ApplicationException(String.Format("Unable to determine a route resolved relative URL for {0}.{1}.", matchingType.FullName, matchingMethod.Name));
					}

					var route = new Routing.Route(name, id.Value, scheme.Value, resolvedRelativeUrl);
					Type[] ignoredRestrictionMapperTypes = matchingMethod
						.GetCustomAttributes<IgnoreRestrictionMapperTypeAttribute>()
						.SelectMany(arg => arg.IgnoredTypes)
						.ToArray();

					foreach (IRestrictionMapper restrictionMapper in _restrictionMappers.Where(arg => !ignoredRestrictionMapperTypes.Contains(arg.GetType())))
					{
						await restrictionMapper.MapAsync(matchingType, matchingMethod, route, _restrictionContainer);
					}

					await _responseMapper.MapAsync(() => _endpointContainer, matchingType, matchingMethod, route);

					if (_authenticationProvider != null)
					{
						bool result = false;

						foreach (IAuthenticationStrategy authenticationStrategy in _authenticationStrategies)
						{
							if (await authenticationStrategy.MustAuthenticateAsync(matchingType, matchingMethod))
							{
								result = true;
								break;
							}
						}
						if (result)
						{
							route.AuthenticationProvider(_authenticationProvider);
						}
					}

					routes.Add(route);
				}
			}
			foreach (Func<IRouteCollection, IEnumerable<Routing.Route>> @delegate in _additionalRoutes)
			{
				routes.Add(@delegate(routes));
			}

			return routes;
		}

		private async Task<string> GetResolvedRelativeUrlAsync(IEnumerable<Type> ignoredResolvedRelativeUrlMapperTypes, Type matchingType, MethodInfo matchingMethod)
		{
			foreach (IResolvedRelativeUrlMapper resolvedRelativeUrlMapper in _resolvedRelativeUrlMappers.Where(arg => !ignoredResolvedRelativeUrlMapperTypes.Contains(arg.GetType())))
			{
				ResolvedRelativeUrlResult result = await resolvedRelativeUrlMapper.MapAsync(matchingType, matchingMethod);

				if (result.ResultType == ResolvedRelativeUrlResultType.ResolvedRelativeUrlMapped)
				{
					return result.ResolvedRelativeUrl;
				}
			}

			return null;
		}

		private async Task<IEnumerable<Type>> GetMatchingTypesAsync()
		{
			var matchingTypes = new List<Type>();

			foreach (Type type in _assemblies.SelectMany(arg => arg.GetTypes()).Where(arg => arg.Namespace != null && (arg.IsPublic || arg.IsNestedPublic) && !arg.IsAbstract && !arg.IsValueType))
			{
				bool matches = true;

				foreach (IClassFilter classFilter in _classFilters)
				{
					if (!await classFilter.MatchesAsync(type))
					{
						matches = false;
						break;
					}
				}
				if (matches)
				{
					matchingTypes.Add(type);
				}
			}

			return matchingTypes;
		}

		private async Task<IEnumerable<MethodInfo>> GetMatchingMethodsAsync(IReflect matchingType)
		{
			var matchingMethods = new List<MethodInfo>();

			foreach (MethodInfo method in matchingType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(arg => !arg.IsSpecialName))
			{
				bool matches = true;

				foreach (IMethodFilter methodFilter in _methodFilters)
				{
					if (!await methodFilter.MatchesAsync(method))
					{
						matches = false;
						break;
					}
				}
				if (matches)
				{
					matchingMethods.Add(method);
				}
			}

			return matchingMethods;
		}

		private async Task<string> GetNameAsync(Type matchingType, MethodInfo matchingMethod)
		{
			foreach (INameMapper nameMapper in _nameMappers)
			{
				NameResult result = (await nameMapper.MapAsync(matchingType, matchingMethod));

				if (result.ResultType == NameResultType.NameMapped)
				{
					return result.Name;
				}
			}

			return null;
		}

		private async Task<Guid?> GetIdAsync(Type matchingType, MethodInfo matchingMethod)
		{
			foreach (IIdMapper idMapper in _idMappers)
			{
				IdResult result = await idMapper.MapAsync(matchingType, matchingMethod);

				if (result.ResultType == IdResultType.IdMapped)
				{
					return result.Id;
				}
			}

			return null;
		}

		private async Task<Scheme?> GetSchemeAsync(Type matchingType, MethodInfo matchingMethod)
		{
			foreach (ISchemeMapper schemeMapper in _schemeMappers)
			{
				SchemeResult result = await schemeMapper.MapAsync(matchingType, matchingMethod);

				if (result.ResultType == SchemeResultType.SchemeMapped)
				{
					return result.Scheme;
				}
			}

			return null;
		}

		private void ThrowIfNoBundleDependencyContainer()
		{
			if (_bundleDependencyContainer == null)
			{
				throw new InvalidOperationException("No bundle dependency container was provided.");
			}
		}

		#region Class filters

		public AutoRouteCollection ClassesInNamespace(string @namespace)
		{
			_classFilters.Add(new InNamespaceFilter(@namespace));

			return this;
		}

		public AutoRouteCollection ClassesWithNamespaceAncestor(string @namespace)
		{
			_classFilters.Add(new HasNamespaceAncestorFilter(@namespace));

			return this;
		}

		public AutoRouteCollection ClassesImplementingInterface<T>()
			where T : class
		{
			_classFilters.Add(new ImplementsInterfaceFilter<T>());

			return this;
		}

		public AutoRouteCollection ClassesImplementingInterface(Type interfaceType)
		{
			_classFilters.Add(new ImplementsInterfaceFilter(interfaceType));

			return this;
		}

		public AutoRouteCollection ClassesDerivingAnotherClass<T>()
			where T : class
		{
			_classFilters.Add(new DerivesFilter<T>());

			return this;
		}

		public AutoRouteCollection ClassesDerivingAnotherClass(Type baseType)
		{
			_classFilters.Add(new DerivesFilter(baseType));

			return this;
		}

		public AutoRouteCollection ClassesWithNamesStartingWith(string value, StringComparison comparison = StringComparison.Ordinal)
		{
			_classFilters.Add(new NameStartsWithFilter(value, comparison));

			return this;
		}

		public AutoRouteCollection ClassesWithNamesEndingWith(string value, StringComparison comparison = StringComparison.Ordinal)
		{
			_classFilters.Add(new NameEndsWithFilter(value, comparison));

			return this;
		}

		public AutoRouteCollection ClassesWithNamesMatchingRegexPattern(string pattern, RegexOptions options = RegexOptions.None)
		{
			_classFilters.Add(new NameMatchesRegexPatternFilter(pattern, options));

			return this;
		}

		public AutoRouteCollection ClassesMatching(Func<Type, bool> matchDelegate)
		{
			_classFilters.Add(new DelegateFilter(matchDelegate));

			return this;
		}

		public AutoRouteCollection ClassFilters(IEnumerable<IClassFilter> filters)
		{
			filters.ThrowIfNull("filters");

			_classFilters.AddRange(filters);

			return this;
		}

		public AutoRouteCollection ClassFilters(params IClassFilter[] filters)
		{
			return ClassFilters((IEnumerable<IClassFilter>)filters);
		}

		#endregion

		#region Method filters

		public AutoRouteCollection MethodsMatching(Func<MethodInfo, bool> matchDelegate)
		{
			_methodFilters.Add(new MethodFilters.DelegateFilter(matchDelegate));

			return this;
		}

		public AutoRouteCollection MethodFilters(IEnumerable<IMethodFilter> filters)
		{
			filters.ThrowIfNull("filters");

			_methodFilters.AddRange(filters);

			return this;
		}

		public AutoRouteCollection MethodFilters(params IMethodFilter[] filters)
		{
			return MethodFilters((IEnumerable<IMethodFilter>)filters);
		}

		#endregion

		#region Name mappers

		public AutoRouteCollection NameAfterRelativeClassNamespaceAndClassNameAndMethodName(
			string rootNamespace,
			bool makeLowercase = false,
			string wordSeparator = " ",
			string wordRegexPattern = NameAfterRelativeClassNamespaceAndClassNameAndMethodNameMapper.DefaultWordRegexPattern)
		{
			_nameMappers.Add(new NameAfterRelativeClassNamespaceAndClassNameAndMethodNameMapper(rootNamespace, makeLowercase, wordSeparator, wordRegexPattern));

			return this;
		}

		public AutoRouteCollection NameAfterRelativeClassNamespaceAndClassName(
			string rootNamespace,
			bool makeLowercase = false,
			string wordSeparator = " ",
			string wordRegexPattern = NameAfterRelativeClassNamespaceAndClassNameMapper.DefaultWordRegexPattern)
		{
			_nameMappers.Add(new NameAfterRelativeClassNamespaceAndClassNameMapper(rootNamespace, makeLowercase, wordSeparator, wordRegexPattern));

			return this;
		}

		public AutoRouteCollection NameUsingAttribute()
		{
			_nameMappers.Add(new NameAttributeMapper());

			return this;
		}

		public AutoRouteCollection NameMappers(IEnumerable<INameMapper> strategies)
		{
			strategies.ThrowIfNull("strategies");

			_nameMappers.AddRange(strategies);

			return this;
		}

		public AutoRouteCollection NameMappers(params INameMapper[] strategies)
		{
			return NameMappers((IEnumerable<INameMapper>)strategies);
		}

		#endregion

		#region ID mappers

		public AutoRouteCollection IdRandomly(IGuidFactory guidFactory)
		{
			_idMappers.Add(new RandomIdMapper(guidFactory));

			return this;
		}

		public AutoRouteCollection IdUsingAttribute()
		{
			_idMappers.Add(new IdAttributeMapper());

			return this;
		}

		public AutoRouteCollection IdMappers(IEnumerable<IIdMapper> mappers)
		{
			mappers.ThrowIfNull("mappers");

			_idMappers.AddRange(mappers);

			return this;
		}

		public AutoRouteCollection IdMappers(params IIdMapper[] mappers)
		{
			return IdMappers((IEnumerable<IIdMapper>)mappers);
		}

		#endregion

		#region Scheme mappers

		public AutoRouteCollection SchemeNotSpecified()
		{
			_schemeMappers.Add(new NotSpecifiedMapper());

			return this;
		}

		public AutoRouteCollection SchemeHttp()
		{
			_schemeMappers.Add(new HttpMapper());

			return this;
		}

		public AutoRouteCollection SchemeHttps()
		{
			_schemeMappers.Add(new HttpsMapper());

			return this;
		}

		public AutoRouteCollection SchemeUsingAttribute()
		{
			_schemeMappers.Add(new SchemeAttributeMapper());

			return this;
		}

		public AutoRouteCollection SchemeMappers(IEnumerable<ISchemeMapper> mappers)
		{
			mappers.ThrowIfNull("mappers");

			_schemeMappers.AddRange(mappers);

			return this;
		}

		public AutoRouteCollection SchemeMappers(params ISchemeMapper[] mappers)
		{
			return SchemeMappers((IEnumerable<ISchemeMapper>)mappers);
		}

		#endregion

		#region Resolved relative URL mappers

		public AutoRouteCollection ResolvedRelativeUrlFromRelativeClassNamespaceAndClassName(
			string rootNamespace,
			bool makeLowercase = true,
			string wordSeparator = "_",
			string wordRegexPattern = ResolvedRelativeUrlFromRelativeClassNamespaceAndClassNameMapper.DefaultWordRegexPattern)
		{
			_resolvedRelativeUrlMappers.Add(new ResolvedRelativeUrlFromRelativeClassNamespaceAndClassNameMapper(rootNamespace, makeLowercase, wordSeparator));

			return this;
		}

		public AutoRouteCollection ResolvedRelativeUrlUsingAttribute()
		{
			_resolvedRelativeUrlMappers.Add(new ResolvedRelativeUrlAttributeMapper());

			return this;
		}

		public AutoRouteCollection ResolvedRelativeUrlMappers(IEnumerable<IResolvedRelativeUrlMapper> mappers)
		{
			mappers.ThrowIfNull("mappers");

			_resolvedRelativeUrlMappers.AddRange(mappers);

			return this;
		}

		public AutoRouteCollection ResolvedRelativeUrlMappers(params IResolvedRelativeUrlMapper[] mappers)
		{
			return ResolvedRelativeUrlMappers((IEnumerable<IResolvedRelativeUrlMapper>)mappers);
		}

		#endregion

		#region Restriction mappers

		public AutoRouteCollection RestrictHttpMethodsToMethodsNamedAfterStandardHttpMethods()
		{
			_restrictionMappers.Add(new HttpMethodFromMethodsNamedAfterStandardHttpMethodsMapper());

			return this;
		}

		public AutoRouteCollection RestrictRelativePathsToRelativeClassNamespaceAndClassName(
			string rootNamespace,
			bool caseSensitive = false,
			bool makeLowercase = true,
			string wordSeparator = "_",
			string wordRegexPattern = UrlRelativePathFromRelativeClassNamespaceAndClassNameMapper.DefaultWordRegexPattern)
		{
			_restrictionMappers.Add(new UrlRelativePathFromRelativeClassNamespaceAndClassNameMapper(rootNamespace, caseSensitive, makeLowercase, wordSeparator, wordRegexPattern));

			return this;
		}

		public AutoRouteCollection RestrictUsingAttributes<T>()
			where T : RestrictionAttribute
		{
			_restrictionMappers.Add(new RestrictionsFromAttributesMapper<T>());

			return this;
		}

		public AutoRouteCollection RestrictUsingAttributes(IEnumerable<Type> attributeTypes)
		{
			attributeTypes.ThrowIfNull("attributeTypes");

			foreach (Type attributeType in attributeTypes)
			{
				_restrictionMappers.Add(new RestrictionsFromAttributesMapper(attributeType));
			}

			return this;
		}

		public AutoRouteCollection RestrictUsingAttributes(params Type[] attributeTypes)
		{
			return RestrictUsingAttributes((IEnumerable<Type>)attributeTypes);
		}

		public AutoRouteCollection RestrictUsingAllAttributeTypes()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			IEnumerable<Type> mappingAttributeTypes = assembly.GetTypes().Where(arg => arg.IsSubclassOf(typeof(RestrictionAttribute)));

			return RestrictUsingAttributes(mappingAttributeTypes);
		}

		public AutoRouteCollection RestrictionMappers(IEnumerable<IRestrictionMapper> mappers)
		{
			mappers.ThrowIfNull("mappers");

			_restrictionMappers.AddRange(mappers);

			return this;
		}

		public AutoRouteCollection RestrictionMappers(params IRestrictionMapper[] mappers)
		{
			return RestrictionMappers((IEnumerable<IRestrictionMapper>)mappers);
		}

		#endregion

		#region Response mappers

		public AutoRouteCollection RespondWithMethodReturnValuesThatImplementIResponse(IEnumerable<IParameterMapper> mappers, IEnumerable<IMappedDelegateContextFactory> contexts)
		{
			mappers.ThrowIfNull("mappers");
			contexts.ThrowIfNull("contexts");

			_responseMapper = new ResponseMethodReturnTypeMapper(mappers, contexts);

			return this;
		}

		public AutoRouteCollection ResponseMapper(IResponseMapper mapper)
		{
			mapper.ThrowIfNull("mapper");

			_responseMapper = mapper;

			return this;
		}

		#endregion

		#region Authentication

		public AutoRouteCollection AuthenticateWhenAttributePresent(IAuthenticationProvider provider)
		{
			return Authenticate(provider, new AuthenticateAttributeStrategy());
		}

		public AutoRouteCollection FormsAuthenticationWithNoRedirectWhenAttributePresent(IFormsAuthenticationHelper helper)
		{
			return Authenticate(FormsAuthenticationProvider.CreateWithNoRedirectOnFailedAuthentication(helper), new AuthenticateAttributeStrategy());
		}

		public AutoRouteCollection FormsAuthenticationWithRelativeUrlRedirectWhenAttributePresent(IFormsAuthenticationHelper helper, IUrlResolver urlResolver, string relativeUrl, string returnUrlQueryStringField = "ReturnURL")
		{
			return Authenticate(FormsAuthenticationProvider.CreateWithRelativeUrlRedirectOnFailedAuthentication(helper, urlResolver, relativeUrl, returnUrlQueryStringField), new AuthenticateAttributeStrategy());
		}

		public AutoRouteCollection FormsAuthenticationWithRouteRedirectWhenAttributePresent(IFormsAuthenticationHelper helper, IUrlResolver urlResolver, string routeName, string returnUrlQueryStringField = "ReturnURL")
		{
			return Authenticate(FormsAuthenticationProvider.CreateWithRouteRedirectOnFailedAuthentication(helper, urlResolver, routeName, returnUrlQueryStringField), new AuthenticateAttributeStrategy());
		}

		public AutoRouteCollection FormsAuthenticationWithRouteRedirectWhenAttributePresent(IFormsAuthenticationHelper helper, IUrlResolver urlResolver, Guid routeId, string returnUrlQueryStringField = "ReturnURL")
		{
			return Authenticate(FormsAuthenticationProvider.CreateWithRouteRedirectOnFailedAuthentication(helper, urlResolver, routeId, returnUrlQueryStringField), new AuthenticateAttributeStrategy());
		}

		public AutoRouteCollection Authenticate(IAuthenticationProvider provider, IEnumerable<IAuthenticationStrategy> strategies)
		{
			provider.ThrowIfNull("provider");
			strategies.ThrowIfNull("strategies");

			strategies = strategies.ToArray();

			if (!strategies.Any())
			{
				throw new ArgumentException("Must provide at least one strategy.", "strategies");
			}

			_authenticationProvider = provider;
			_authenticationStrategies.Clear();
			_authenticationStrategies.AddRange(strategies);

			return this;
		}

		public AutoRouteCollection Authenticate(IAuthenticationProvider provider, params IAuthenticationStrategy[] strategies)
		{
			return Authenticate(provider, (IEnumerable<IAuthenticationStrategy>)strategies);
		}

		public AutoRouteCollection DoNotAuthenticate()
		{
			_authenticationProvider = null;
			_authenticationStrategies.Clear();

			return this;
		}

		#endregion

		#region Bundles

		public AutoRouteCollection CssBundle(Bundle bundle, string routeName, Scheme scheme, string relativePath, IComparer<AssetFile> assetOrder, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			ThrowIfNoBundleDependencyContainer();

			bundle.ThrowIfNull("bundle");
			routeName.ThrowIfNull("routeName");
			relativePath.ThrowIfNull("relativePath");
			assetOrder.ThrowIfNull("assetOrder");
			concatenator.ThrowIfNull("concatenator");
			transformers.ThrowIfNull("transformers");

			var watcher = new BundleWatcher(bundle, _bundleDependencyContainer.GetInstance<IFileSystem>(), assetOrder, concatenator, transformers);
			var guidFactory = _bundleDependencyContainer.GetInstance<IGuidFactory>();
			var httpRuntime = _bundleDependencyContainer.GetInstance<IHttpRuntime>();
			var systemClock = _bundleDependencyContainer.GetInstance<ISystemClock>();
			var route = new CssBundleWatcherRoute(routeName, guidFactory.Random(), scheme, relativePath, watcher, httpRuntime, systemClock);

			return AdditionalRoutes(route);
		}

		public AutoRouteCollection CssBundle(Bundle bundle, string routeName, Scheme scheme, string relativePath, IComparer<AssetFile> assetOrder, IAssetConcatenator concatenator, params IAssetTransformer[] transformers)
		{
			return CssBundle(bundle, routeName, scheme, relativePath, assetOrder, concatenator, (IEnumerable<IAssetTransformer>)transformers);
		}

		public AutoRouteCollection CssBundle(Bundle bundle, string routeName, Scheme scheme, string relativePath, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			ThrowIfNoBundleDependencyContainer();

			bundle.ThrowIfNull("bundle");
			routeName.ThrowIfNull("routeName");
			relativePath.ThrowIfNull("relativePath");
			concatenator.ThrowIfNull("concatenator");
			transformers.ThrowIfNull("transformers");

			var watcher = new BundleWatcher(bundle, _bundleDependencyContainer.GetInstance<IFileSystem>(), concatenator, transformers);
			var guidFactory = _bundleDependencyContainer.GetInstance<IGuidFactory>();
			var httpRuntime = _bundleDependencyContainer.GetInstance<IHttpRuntime>();
			var systemClock = _bundleDependencyContainer.GetInstance<ISystemClock>();
			var route = new CssBundleWatcherRoute(routeName, guidFactory.Random(), scheme, relativePath, watcher, httpRuntime, systemClock);

			return AdditionalRoutes(route);
		}

		public AutoRouteCollection CssBundle(Bundle bundle, string routeName, Scheme scheme, string relativePath, IAssetConcatenator concatenator, params IAssetTransformer[] transformers)
		{
			return CssBundle(bundle, routeName, scheme, relativePath, concatenator, (IEnumerable<IAssetTransformer>)transformers);
		}

		public AutoRouteCollection CssBundles(IEnumerable<BundleRoute> bundleRoutes)
		{
			bundleRoutes.ThrowIfNull("bundleRoutes");

			foreach (BundleRoute bundleRoute in bundleRoutes)
			{
				if (bundleRoute.AssetOrder != null)
				{
					CssBundle(bundleRoute.Bundle, bundleRoute.RouteName, bundleRoute.Scheme, bundleRoute.RelativePath, bundleRoute.AssetOrder, bundleRoute.Concatenator, bundleRoute.Transformers);
				}
				else
				{
					CssBundle(bundleRoute.Bundle, bundleRoute.RouteName, bundleRoute.Scheme, bundleRoute.RelativePath, bundleRoute.Concatenator, bundleRoute.Transformers);
				}
			}

			return this;
		}

		public AutoRouteCollection JavaScriptBundle(Bundle bundle, string routeName, Scheme scheme, string relativePath, IComparer<AssetFile> assetOrder, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			ThrowIfNoBundleDependencyContainer();

			bundle.ThrowIfNull("bundle");
			routeName.ThrowIfNull("routeName");
			relativePath.ThrowIfNull("relativePath");
			assetOrder.ThrowIfNull("assetOrder");
			concatenator.ThrowIfNull("concatenator");
			transformers.ThrowIfNull("transformers");

			var watcher = new BundleWatcher(bundle, _bundleDependencyContainer.GetInstance<IFileSystem>(), assetOrder, concatenator, transformers);
			var guidFactory = _bundleDependencyContainer.GetInstance<IGuidFactory>();
			var httpRuntime = _bundleDependencyContainer.GetInstance<IHttpRuntime>();
			var systemClock = _bundleDependencyContainer.GetInstance<ISystemClock>();
			var route = new JavaScriptBundleWatcherRoute(routeName, guidFactory.Random(), scheme, relativePath, watcher, httpRuntime, systemClock);

			return AdditionalRoutes(route);
		}

		public AutoRouteCollection JavaScriptBundle(Bundle bundle, string routeName, Scheme scheme, string relativePath, IComparer<AssetFile> assetOrder, IAssetConcatenator concatenator, params IAssetTransformer[] transformers)
		{
			return JavaScriptBundle(bundle, routeName, scheme, relativePath, assetOrder, concatenator, (IEnumerable<IAssetTransformer>)transformers);
		}

		public AutoRouteCollection JavaScriptBundle(Bundle bundle, string routeName, Scheme scheme, string relativePath, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			ThrowIfNoBundleDependencyContainer();

			bundle.ThrowIfNull("bundle");
			routeName.ThrowIfNull("routeName");
			relativePath.ThrowIfNull("relativePath");
			concatenator.ThrowIfNull("concatenator");
			transformers.ThrowIfNull("transformers");

			var watcher = new BundleWatcher(bundle, _bundleDependencyContainer.GetInstance<IFileSystem>(), concatenator, transformers);
			var guidFactory = _bundleDependencyContainer.GetInstance<IGuidFactory>();
			var httpRuntime = _bundleDependencyContainer.GetInstance<IHttpRuntime>();
			var systemClock = _bundleDependencyContainer.GetInstance<ISystemClock>();
			var route = new JavaScriptBundleWatcherRoute(routeName, guidFactory.Random(), scheme, relativePath, watcher, httpRuntime, systemClock);

			return AdditionalRoutes(route);
		}

		public AutoRouteCollection JavaScriptBundle(Bundle bundle, string routeName, Scheme scheme, string relativePath, IAssetConcatenator concatenator, params IAssetTransformer[] transformers)
		{
			return JavaScriptBundle(bundle, routeName, scheme, relativePath, concatenator, (IEnumerable<IAssetTransformer>)transformers);
		}

		public AutoRouteCollection JavaScriptBundles(IEnumerable<BundleRoute> bundleRoutes)
		{
			bundleRoutes.ThrowIfNull("bundleRoutes");

			foreach (BundleRoute bundleRoute in bundleRoutes)
			{
				if (bundleRoute.AssetOrder != null)
				{
					JavaScriptBundle(bundleRoute.Bundle, bundleRoute.RouteName, bundleRoute.Scheme, bundleRoute.RelativePath, bundleRoute.AssetOrder, bundleRoute.Concatenator, bundleRoute.Transformers);
				}
				else
				{
					JavaScriptBundle(bundleRoute.Bundle, bundleRoute.RouteName, bundleRoute.Scheme, bundleRoute.RelativePath, bundleRoute.Concatenator, bundleRoute.Transformers);
				}
			}

			return this;
		}

		#endregion

		#region Additional routes

		public AutoRouteCollection AdditionalRoutes(IEnumerable<Func<IRouteCollection, IEnumerable<Routing.Route>>> additionalRoutesForRegistration)
		{
			additionalRoutesForRegistration.ThrowIfNull("additionalRoutesForRegistration");

			_additionalRoutes.AddRange(additionalRoutesForRegistration);

			return this;
		}

		public AutoRouteCollection AdditionalRoutes(params Func<IRouteCollection, IEnumerable<Routing.Route>>[] additionalRoutesForRegistration)
		{
			return AdditionalRoutes((IEnumerable<Func<IRouteCollection, IEnumerable<Routing.Route>>>)additionalRoutesForRegistration);
		}

		public AutoRouteCollection AdditionalRoutes(IEnumerable<Routing.Route> additionalRoutesForRegistration)
		{
			additionalRoutesForRegistration.ThrowIfNull("additionalRoutesForRegistration");

			_additionalRoutes.Add(routes => additionalRoutesForRegistration);

			return this;
		}

		public AutoRouteCollection AdditionalRoutes(params Routing.Route[] additionalRoutesForRegistration)
		{
			return AdditionalRoutes((IEnumerable<Routing.Route>)additionalRoutesForRegistration);
		}

		#endregion
	}
}
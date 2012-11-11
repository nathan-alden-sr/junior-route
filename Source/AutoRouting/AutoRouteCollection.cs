using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using Junior.Common;
using Junior.Route.Assets.FileSystem;
using Junior.Route.AutoRouting.AuthenticationStrategies;
using Junior.Route.AutoRouting.ClassFilters;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.IdMappers;
using Junior.Route.AutoRouting.MethodFilters;
using Junior.Route.AutoRouting.NameMappers;
using Junior.Route.AutoRouting.ParameterMappers;
using Junior.Route.AutoRouting.ResolvedRelativeUrlMappers;
using Junior.Route.AutoRouting.ResponseMappers;
using Junior.Route.AutoRouting.RestrictionMappers;
using Junior.Route.AutoRouting.RestrictionMappers.Attributes;
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

		public IRouteCollection GenerateRouteCollection()
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

			IEnumerable<Type> matchingTypes = _assemblies
				.SelectMany(arg => arg.GetTypes())
				.Where(type => type.Namespace != null && !type.IsAbstract && !type.IsValueType && _classFilters.All(filter => filter.Matches(type)))
				.ToArray();

			foreach (Type matchingType in matchingTypes)
			{
				IEnumerable<MethodInfo> matchingMethods = matchingType
					.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
					.Where(method => !method.IsSpecialName && _methodFilters.All(filter => filter.Matches(method)));

				foreach (MethodInfo matchingMethod in matchingMethods)
				{
					Type closureMatchingType = matchingType;
					MethodInfo closureMatchingMethod = matchingMethod;
					string name = _nameMappers
						.Select(arg => new { Mapper = arg, Result = arg.Map(closureMatchingType, closureMatchingMethod) })
						.FirstOrDefault(arg => arg.Result.ResultType == NameResultType.NameMapped)
						.IfNotNull(arg => arg.Result.Name);

					if (name == null)
					{
						throw new ApplicationException(String.Format("Unable to determine a route name for '{0}.{1}'.", matchingType.FullName, matchingMethod.Name));
					}

					Guid? id = _idMappers
						.Select(arg => new { Mapper = arg, Result = arg.Map(closureMatchingType, closureMatchingMethod) })
						.FirstOrDefault(arg => arg.Result.ResultType == IdResultType.IdMapped)
						.IfNotNull(arg => arg.Result.Id);

					if (id == null)
					{
						throw new ApplicationException(String.Format("Unable to determine a route ID for '{0}.{1}'.", matchingType.FullName, matchingMethod.Name));
					}

					string resolvedRelativeUrl = _resolvedRelativeUrlMappers
						.Select(arg => new { Mapper = arg, Result = arg.Map(closureMatchingType, closureMatchingMethod) })
						.FirstOrDefault(arg => arg.Result.ResultType == ResolvedRelativeUrlResultType.ResolvedRelativeUrlMapped)
						.IfNotNull(arg => arg.Result.ResolvedRelativeUrl);

					if (resolvedRelativeUrl == null)
					{
						throw new ApplicationException(String.Format("Unable to determine a route resolved relative URL for '{0}.{1}'.", matchingType.FullName, matchingMethod.Name));
					}

					var route = new Routing.Route(name, id.Value, resolvedRelativeUrl);

					foreach (IRestrictionMapper restrictionMapper in _restrictionMappers)
					{
						restrictionMapper.Map(matchingType, matchingMethod, route, _restrictionContainer);
					}

					_responseMapper.Map(() => _endpointContainer, matchingType, matchingMethod, route);

					if (_authenticationProvider != null && _authenticationStrategies.Any(arg => arg.MustAuthenticate(closureMatchingType, closureMatchingMethod)))
					{
						route.AuthenticationProvider(_authenticationProvider);
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

		[Obsolete("This inconsistent method name will be removed in a future release; please use ResolvedRelativeUrlFromRelativeClassNamespaceAndClassName")]
		public AutoRouteCollection ResolvedRelativeUrlFromRelativeClassNamespacesAndClassNames(
			string rootNamespace,
			bool makeLowercase = true,
			string wordSeparator = "_",
			string wordRegexPattern = ResolvedRelativeUrlFromRelativeClassNamespaceAndClassNameMapper.DefaultWordRegexPattern)
		{
			return ResolvedRelativeUrlFromRelativeClassNamespaceAndClassName(rootNamespace, makeLowercase, wordSeparator, wordRegexPattern);
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

		public AutoRouteCollection RestrictRelativeUrlsToRelativeClassNamespaceAndClassName(
			string rootNamespace,
			bool caseSensitive = false,
			bool makeLowercase = true,
			string wordSeparator = "_",
			string wordRegexPattern = UrlRelativePathFromRelativeClassNamespaceAndClassNameMapper.DefaultWordRegexPattern)
		{
			_restrictionMappers.Add(new UrlRelativePathFromRelativeClassNamespaceAndClassNameMapper(rootNamespace, caseSensitive, makeLowercase, wordSeparator, wordRegexPattern));

			return this;
		}

		[Obsolete("This inconsistent method name will be removed in a future release; please use RestrictRelativeUrlsToRelativeClassNamespaceAndClassName")]
		public AutoRouteCollection RestrictRelativeUrlsToRelativeClassNamespacesAndClassNames(
			string rootNamespace,
			bool caseSensitive = false,
			bool makeLowercase = true,
			string wordSeparator = "_",
			string wordRegexPattern = UrlRelativePathFromRelativeClassNamespaceAndClassNameMapper.DefaultWordRegexPattern)
		{
			return RestrictRelativeUrlsToRelativeClassNamespaceAndClassName(rootNamespace, caseSensitive, makeLowercase, wordSeparator, wordRegexPattern);
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

		public AutoRouteCollection RespondWithMethodReturnValuesThatImplementIResponse(IEnumerable<IParameterMapper> mappers)
		{
			mappers.ThrowIfNull("mappers");

			_responseMapper = new ResponseMethodReturnTypeMapper(mappers);

			return this;
		}

		public AutoRouteCollection RespondWithMethodReturnValuesThatImplementIResponse(params IParameterMapper[] mappers)
		{
			return RespondWithMethodReturnValuesThatImplementIResponse((IEnumerable<IParameterMapper>)mappers);
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

		public AutoRouteCollection FormsAuthenticationWithNoRedirectWhenAttributePresent()
		{
			return Authenticate(FormsAuthenticationProvider.CreateWithNoRedirectOnFailedAuthentication(), new AuthenticateAttributeStrategy());
		}

		public AutoRouteCollection FormsAuthenticationWithRelativeUrlRedirectWhenAttributePresent(IUrlResolver urlResolver, string relativeUrl, bool appendReturnUrl = false, string returnUrlQueryStringField = "ReturnURL")
		{
			return Authenticate(FormsAuthenticationProvider.CreateWithRelativeUrlRedirectOnFailedAuthentication(urlResolver, relativeUrl, appendReturnUrl, returnUrlQueryStringField), new AuthenticateAttributeStrategy());
		}

		public AutoRouteCollection FormsAuthenticationWithRouteRedirectWhenAttributePresent(IUrlResolver urlResolver, string routeName, bool appendReturnUrl = false, string returnUrlQueryStringField = "ReturnURL")
		{
			return Authenticate(FormsAuthenticationProvider.CreateWithRouteRedirectOnFailedAuthentication(urlResolver, routeName, appendReturnUrl, returnUrlQueryStringField), new AuthenticateAttributeStrategy());
		}

		public AutoRouteCollection FormsAuthenticationWithRouteRedirectWhenAttributePresent(IUrlResolver urlResolver, Guid routeId, bool appendReturnUrl = false, string returnUrlQueryStringField = "ReturnURL")
		{
			return Authenticate(FormsAuthenticationProvider.CreateWithRouteRedirectOnFailedAuthentication(urlResolver, routeId, appendReturnUrl, returnUrlQueryStringField), new AuthenticateAttributeStrategy());
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

		public AutoRouteCollection CssBundle(Bundle bundle, string routeName, string relativePath, IComparer<AssetFile> assetOrder, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			ThrowIfNoBundleDependencyContainer();

			bundle.ThrowIfNull("bundle");
			routeName.ThrowIfNull("routeName");
			relativePath.ThrowIfNull("relativePath");
			assetOrder.ThrowIfNull("assetOrder");
			concatenator.ThrowIfNull("concatenator");
			transformers.ThrowIfNull("transformers");

			var watcher = new BundleWatcher(bundle, _bundleDependencyContainer.GetInstance<IFileSystem>(), assetOrder, concatenator, transformers);
			var route = new CssBundleWatcherRoute(
				routeName,
				_bundleDependencyContainer.GetInstance<IGuidFactory>(),
				relativePath,
				watcher,
				_bundleDependencyContainer.GetInstance<IHttpRuntime>(),
				_bundleDependencyContainer.GetInstance<ISystemClock>());

			return AdditionalRoutes(route);
		}

		public AutoRouteCollection CssBundle(Bundle bundle, string routeName, string relativePath, IComparer<AssetFile> assetOrder, IAssetConcatenator concatenator, params IAssetTransformer[] transformers)
		{
			return CssBundle(bundle, routeName, relativePath, assetOrder, concatenator, (IEnumerable<IAssetTransformer>)transformers);
		}

		public AutoRouteCollection CssBundle(Bundle bundle, string routeName, string relativePath, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			ThrowIfNoBundleDependencyContainer();

			bundle.ThrowIfNull("bundle");
			routeName.ThrowIfNull("routeName");
			relativePath.ThrowIfNull("relativePath");
			concatenator.ThrowIfNull("concatenator");
			transformers.ThrowIfNull("transformers");

			var watcher = new BundleWatcher(bundle, _bundleDependencyContainer.GetInstance<IFileSystem>(), concatenator, transformers);
			var route = new CssBundleWatcherRoute(
				routeName,
				_bundleDependencyContainer.GetInstance<IGuidFactory>(),
				relativePath,
				watcher,
				_bundleDependencyContainer.GetInstance<IHttpRuntime>(),
				_bundleDependencyContainer.GetInstance<ISystemClock>());

			return AdditionalRoutes(route);
		}

		public AutoRouteCollection CssBundle(Bundle bundle, string routeName, string relativePath, IAssetConcatenator concatenator, params IAssetTransformer[] transformers)
		{
			return CssBundle(bundle, routeName, relativePath, concatenator, (IEnumerable<IAssetTransformer>)transformers);
		}

		public AutoRouteCollection JavaScriptBundle(Bundle bundle, string routeName, string relativePath, IComparer<AssetFile> assetOrder, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			ThrowIfNoBundleDependencyContainer();

			bundle.ThrowIfNull("bundle");
			routeName.ThrowIfNull("routeName");
			relativePath.ThrowIfNull("relativePath");
			assetOrder.ThrowIfNull("assetOrder");
			concatenator.ThrowIfNull("concatenator");
			transformers.ThrowIfNull("transformers");

			var watcher = new BundleWatcher(bundle, _bundleDependencyContainer.GetInstance<IFileSystem>(), assetOrder, concatenator, transformers);
			var route = new JavaScriptBundleWatcherRoute(
				routeName,
				_bundleDependencyContainer.GetInstance<IGuidFactory>(),
				relativePath,
				watcher,
				_bundleDependencyContainer.GetInstance<IHttpRuntime>(),
				_bundleDependencyContainer.GetInstance<ISystemClock>());

			return AdditionalRoutes(route);
		}

		public AutoRouteCollection JavaScriptBundle(Bundle bundle, string routeName, string relativePath, IComparer<AssetFile> assetOrder, IAssetConcatenator concatenator, params IAssetTransformer[] transformers)
		{
			return JavaScriptBundle(bundle, routeName, relativePath, assetOrder, concatenator, (IEnumerable<IAssetTransformer>)transformers);
		}

		public AutoRouteCollection JavaScriptBundle(Bundle bundle, string routeName, string relativePath, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			ThrowIfNoBundleDependencyContainer();

			bundle.ThrowIfNull("bundle");
			routeName.ThrowIfNull("routeName");
			relativePath.ThrowIfNull("relativePath");
			concatenator.ThrowIfNull("concatenator");
			transformers.ThrowIfNull("transformers");

			var watcher = new BundleWatcher(bundle, _bundleDependencyContainer.GetInstance<IFileSystem>(), concatenator, transformers);
			var route = new JavaScriptBundleWatcherRoute(
				routeName,
				_bundleDependencyContainer.GetInstance<IGuidFactory>(),
				relativePath,
				watcher,
				_bundleDependencyContainer.GetInstance<IHttpRuntime>(),
				_bundleDependencyContainer.GetInstance<ISystemClock>());

			return AdditionalRoutes(route);
		}

		public AutoRouteCollection JavaScriptBundle(Bundle bundle, string routeName, string relativePath, IAssetConcatenator concatenator, params IAssetTransformer[] transformers)
		{
			return JavaScriptBundle(bundle, routeName, relativePath, concatenator, (IEnumerable<IAssetTransformer>)transformers);
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
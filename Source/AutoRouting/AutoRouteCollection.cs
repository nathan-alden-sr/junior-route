using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using Junior.Common;
using Junior.Route.Assets.FileSystem;
using Junior.Route.AutoRouting.ClassFilters;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.AutoRouting.IdMappers;
using Junior.Route.AutoRouting.MethodFilters;
using Junior.Route.AutoRouting.NamingStrategies;
using Junior.Route.AutoRouting.ParameterMappers;
using Junior.Route.AutoRouting.ResolvedRelativeUrlMappers;
using Junior.Route.AutoRouting.ResponseMappers;
using Junior.Route.AutoRouting.RestrictionMappers;
using Junior.Route.AutoRouting.RestrictionMappers.Attributes;
using Junior.Route.Common;
using Junior.Route.Routing;

using DelegateFilter = Junior.Route.AutoRouting.ClassFilters.DelegateFilter;

namespace Junior.Route.AutoRouting
{
	public class AutoRouteCollection
	{
		private readonly HashSet<Func<IRouteCollection, IEnumerable<Routing.Route>>> _additionalRoutes = new HashSet<Func<IRouteCollection, IEnumerable<Routing.Route>>>();
		private readonly HashSet<Assembly> _assemblies = new HashSet<Assembly>();
		private readonly HashSet<IClassFilter> _classFilters = new HashSet<IClassFilter>();
		private readonly HashSet<IIdMapper> _idMappers = new HashSet<IIdMapper>();
		private readonly HashSet<IMethodFilter> _methodFilters = new HashSet<IMethodFilter>();
		private readonly HashSet<INamingStrategy> _namingStrategies = new HashSet<INamingStrategy>();
		private readonly HashSet<IResolvedRelativeUrlMapper> _resolvedRelativeUrlMappers = new HashSet<IResolvedRelativeUrlMapper>();
		private readonly HashSet<IRouteRestrictionMapper> _routeRestrictionMappers = new HashSet<IRouteRestrictionMapper>();
		private IContainer _bundleDependencyContainer;
		private bool _duplicateRouteNamesAllowed;
		private IContainer _endpointContainer = new NewInstancePerRouteContainer();
		private IRouteResponseMapper _routeResponseMapper = new NoContentMapper();
		private RouteCollection _routes;

		public IRouteCollection Routes
		{
			get
			{
				if (_routes == null)
				{
					throw new InvalidOperationException("RegisterRoutes must be called before accessing this property.");
				}

				return _routes;
			}
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
			ThrowIfRoutesAreRegistered();

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
			ThrowIfRoutesAreRegistered();

			_endpointContainer = new NewInstancePerRouteContainer();

			return this;
		}

		public AutoRouteCollection SingleInstancePerRouteContainer()
		{
			ThrowIfRoutesAreRegistered();

			_endpointContainer = new SingleInstancePerRouteContainer();

			return this;
		}

		public AutoRouteCollection EndpointContainer(IContainer container)
		{
			ThrowIfRoutesAreRegistered();

			container.ThrowIfNull("container");

			_endpointContainer = container;

			return this;
		}

		public AutoRouteCollection BundleDependencyContainer(IContainer container)
		{
			ThrowIfRoutesAreRegistered();

			container.ThrowIfNull("container");

			_bundleDependencyContainer = container;

			return this;
		}

		public AutoRouteCollection RegisterRoutes()
		{
			ThrowIfRoutesAreRegistered();

			if (!_assemblies.Any())
			{
				throw new InvalidOperationException("At least one assembly must be provided.");
			}
			if (!_classFilters.Any())
			{
				throw new InvalidOperationException("At least one class filter must be provided.");
			}
			if (!_idMappers.Any())
			{
				throw new InvalidOperationException("At least one ID mapper must be provided.");
			}
			if (!_namingStrategies.Any())
			{
				throw new InvalidOperationException("At least one naming strategy must be provided.");
			}
			if (!_resolvedRelativeUrlMappers.Any())
			{
				throw new InvalidOperationException("At least one resolved relative URL mapper must be provided.");
			}

			_routes = new RouteCollection(_duplicateRouteNamesAllowed);

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
					string name = _namingStrategies
						.Select(arg => new { Strategy = arg, Result = arg.Name(closureMatchingType, closureMatchingMethod) })
						.FirstOrDefault(arg => arg.Result.ResultType == NamingResultType.RouteNamed)
						.IfNotNull(arg => arg.Result.Name);

					if (name == null)
					{
						throw new ApplicationException(String.Format("Unable to determine a route name for '{0}.{1}'.", matchingType.FullName, matchingMethod.Name));
					}

					Guid? id = _idMappers
						.Select(arg => new { Mapper = arg, Result = arg.Id(closureMatchingType, closureMatchingMethod) })
						.FirstOrDefault(arg => arg.Result.ResultType == IdResultType.IdMapped)
						.IfNotNull(arg => arg.Result.Id);

					if (id == null)
					{
						throw new ApplicationException(String.Format("Unable to determine a route ID for '{0}.{1}'.", matchingType.FullName, matchingMethod.Name));
					}

					string resolvedRelativeUrl = _resolvedRelativeUrlMappers
						.Select(arg => new { Mapper = arg, Result = arg.ResolvedRelativeUrl(closureMatchingType, closureMatchingMethod) })
						.FirstOrDefault(arg => arg.Result.ResultType == ResolvedRelativeUrlResultType.ResolvedRelativeUrlMapped)
						.IfNotNull(arg => arg.Result.ResolvedRelativeUrl);

					if (resolvedRelativeUrl == null)
					{
						throw new ApplicationException(String.Format("Unable to determine a route resolved relative URL for '{0}.{1}'.", matchingType.FullName, matchingMethod.Name));
					}

					var route = new Routing.Route(name, id.Value, resolvedRelativeUrl);

					foreach (IRouteRestrictionMapper restrictionMapper in _routeRestrictionMappers)
					{
						restrictionMapper.Map(matchingType, matchingMethod, route);
					}

					_routeResponseMapper.Map(() => _endpointContainer, matchingType, matchingMethod, route);

					_routes.Add(route);
				}
			}
			foreach (Func<IRouteCollection, IEnumerable<Routing.Route>> @delegate in _additionalRoutes)
			{
				_routes.Add(@delegate(_routes));
			}

			return this;
		}

		public void Reset()
		{
			_additionalRoutes.Clear();
			_assemblies.Clear();
			_classFilters.Clear();
			_idMappers.Clear();
			_methodFilters.Clear();
			_namingStrategies.Clear();
			_resolvedRelativeUrlMappers.Clear();
			_routeRestrictionMappers.Clear();
			_endpointContainer = new NewInstancePerRouteContainer();
			_routeResponseMapper = new NoContentMapper();
			_routes = null;
			_bundleDependencyContainer = null;
		}

		private void ThrowIfRoutesAreRegistered()
		{
			if (_routes != null)
			{
				throw new InvalidOperationException("Cannot modify the instance after RegisterRoutes has been called. To reset the instance, call Reset.");
			}
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
			ThrowIfRoutesAreRegistered();

			_classFilters.Add(new InNamespaceFilter(@namespace));

			return this;
		}

		public AutoRouteCollection ClassesWithNamespaceAncestor(string @namespace)
		{
			ThrowIfRoutesAreRegistered();

			_classFilters.Add(new HasNamespaceAncestorFilter(@namespace));

			return this;
		}

		public AutoRouteCollection ClassesImplementingInterface<T>()
			where T : class
		{
			ThrowIfRoutesAreRegistered();

			_classFilters.Add(new ImplementsInterfaceFilter<T>());

			return this;
		}

		public AutoRouteCollection ClassesImplementingInterface(Type interfaceType)
		{
			ThrowIfRoutesAreRegistered();

			_classFilters.Add(new ImplementsInterfaceFilter(interfaceType));

			return this;
		}

		public AutoRouteCollection ClassesDerivingAnotherClass<T>()
			where T : class
		{
			ThrowIfRoutesAreRegistered();

			_classFilters.Add(new DerivesFilter<T>());

			return this;
		}

		public AutoRouteCollection ClassesDerivingAnotherClass(Type baseType)
		{
			ThrowIfRoutesAreRegistered();

			_classFilters.Add(new DerivesFilter(baseType));

			return this;
		}

		public AutoRouteCollection ClassesWithNamesStartingWith(string value, StringComparison comparison = StringComparison.Ordinal)
		{
			ThrowIfRoutesAreRegistered();

			_classFilters.Add(new NameStartsWithFilter(value, comparison));

			return this;
		}

		public AutoRouteCollection ClassesWithNamesEndingWith(string value, StringComparison comparison = StringComparison.Ordinal)
		{
			ThrowIfRoutesAreRegistered();

			_classFilters.Add(new NameEndsWithFilter(value, comparison));

			return this;
		}

		public AutoRouteCollection ClassesWithNamesMatchingRegexPattern(string pattern, RegexOptions options = RegexOptions.None)
		{
			ThrowIfRoutesAreRegistered();

			_classFilters.Add(new NameMatchesRegexPatternFilter(pattern, options));

			return this;
		}

		public AutoRouteCollection ClassesMatching(Func<Type, bool> matchDelegate)
		{
			ThrowIfRoutesAreRegistered();

			_classFilters.Add(new DelegateFilter(matchDelegate));

			return this;
		}

		public AutoRouteCollection AllClasses()
		{
			ThrowIfRoutesAreRegistered();

			_classFilters.Add(new AllFilter());

			return this;
		}

		public AutoRouteCollection ClassFilters(IEnumerable<IClassFilter> filters)
		{
			ThrowIfRoutesAreRegistered();

			filters.ThrowIfNull("filters");

			_classFilters.AddRange(filters);

			return this;
		}

		public AutoRouteCollection ClassFilters(params IClassFilter[] filters)
		{
			ThrowIfRoutesAreRegistered();

			return ClassFilters((IEnumerable<IClassFilter>)filters);
		}

		#endregion

		#region Method filters

		public AutoRouteCollection MethodsMatching(Func<MethodInfo, bool> matchDelegate)
		{
			ThrowIfRoutesAreRegistered();

			_methodFilters.Add(new MethodFilters.DelegateFilter(matchDelegate));

			return this;
		}

		public AutoRouteCollection MethodFilters(IEnumerable<IMethodFilter> filters)
		{
			ThrowIfRoutesAreRegistered();

			filters.ThrowIfNull("filters");

			_methodFilters.AddRange(filters);

			return this;
		}

		public AutoRouteCollection MethodFilters(params IMethodFilter[] filters)
		{
			ThrowIfRoutesAreRegistered();

			return MethodFilters((IEnumerable<IMethodFilter>)filters);
		}

		#endregion

		#region ID mappers

		public AutoRouteCollection IdUsingAttribute()
		{
			ThrowIfRoutesAreRegistered();

			_idMappers.Add(new IdAttributeMapper());

			return this;
		}

		public AutoRouteCollection IdMappers(IEnumerable<IIdMapper> mappers)
		{
			ThrowIfRoutesAreRegistered();

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

		public AutoRouteCollection ResolvedRelativeUrlFromRelativeClassNamespacesAndClassNames(string rootNamespace, bool makeLowercase = true)
		{
			ThrowIfRoutesAreRegistered();

			_resolvedRelativeUrlMappers.Add(new ResolvedRelativeUrlFromRelativeClassNamespaceAndClassNameMapper(rootNamespace, makeLowercase));

			return this;
		}

		public AutoRouteCollection ResolvedRelativeUrlUsingAttribute()
		{
			ThrowIfRoutesAreRegistered();

			_resolvedRelativeUrlMappers.Add(new ResolvedRelativeUrlAttributeMapper());

			return this;
		}

		public AutoRouteCollection ResolvedRelativeUrlMappers(IEnumerable<IResolvedRelativeUrlMapper> mappers)
		{
			ThrowIfRoutesAreRegistered();

			mappers.ThrowIfNull("mappers");

			_resolvedRelativeUrlMappers.AddRange(mappers);

			return this;
		}

		public AutoRouteCollection ResolvedRelativeUrlMappers(params IResolvedRelativeUrlMapper[] mappers)
		{
			return ResolvedRelativeUrlMappers((IEnumerable<IResolvedRelativeUrlMapper>)mappers);
		}

		#endregion

		#region Naming strategies

		public AutoRouteCollection NameAfterClassNamespaceAndClassNameAndMethodName()
		{
			ThrowIfRoutesAreRegistered();

			_namingStrategies.Add(new ClassNamespaceAndClassNameAndMethodNameStrategy());

			return this;
		}

		public AutoRouteCollection NameUsingAttribute()
		{
			ThrowIfRoutesAreRegistered();

			_namingStrategies.Add(new NameAttributeStrategy());

			return this;
		}

		public AutoRouteCollection NamingStrategies(IEnumerable<INamingStrategy> strategies)
		{
			ThrowIfRoutesAreRegistered();

			strategies.ThrowIfNull("strategies");

			_namingStrategies.AddRange(strategies);

			return this;
		}

		public AutoRouteCollection NamingStrategies(params INamingStrategy[] strategies)
		{
			return NamingStrategies((IEnumerable<INamingStrategy>)strategies);
		}

		#endregion

		#region Restriction mappers

		public AutoRouteCollection RestrictHttpMethodsToMethodsNamedAfterStandardHttpMethods()
		{
			ThrowIfRoutesAreRegistered();

			_routeRestrictionMappers.Add(new HttpMethodFromMethodsNamedAfterStandardHttpMethodsMapper());

			return this;
		}

		public AutoRouteCollection RestrictRelativeUrlsToRelativeClassNamespacesAndClassNames(string rootNamespace, bool makeLowercase = true)
		{
			ThrowIfRoutesAreRegistered();

			_routeRestrictionMappers.Add(new RelativeUrlFromRelativeClassNamespaceAndClassNameMapper(rootNamespace, makeLowercase));

			return this;
		}

		public AutoRouteCollection RestrictUsingAttributes<T>()
			where T : RestrictionAttribute
		{
			ThrowIfRoutesAreRegistered();

			_routeRestrictionMappers.Add(new RestrictionsFromAttributesMapper<T>());

			return this;
		}

		public AutoRouteCollection RestrictUsingAttributes(IEnumerable<Type> attributeTypes)
		{
			ThrowIfRoutesAreRegistered();

			attributeTypes.ThrowIfNull("attributeTypes");

			foreach (Type attributeType in attributeTypes)
			{
				_routeRestrictionMappers.Add(new RestrictionsFromAttributesMapper(attributeType));
			}

			return this;
		}

		public AutoRouteCollection RestrictUsingAttributes(params Type[] attributeTypes)
		{
			return RestrictUsingAttributes((IEnumerable<Type>)attributeTypes);
		}

		public AutoRouteCollection RestrictUsingAllAttributeTypes()
		{
			ThrowIfRoutesAreRegistered();

			Assembly assembly = Assembly.GetExecutingAssembly();
			IEnumerable<Type> mappingAttributeTypes = assembly.GetTypes().Where(arg => arg.IsSubclassOf(typeof(RestrictionAttribute)));

			return RestrictUsingAttributes(mappingAttributeTypes);
		}

		public AutoRouteCollection RestrictionMappers(IEnumerable<IRouteRestrictionMapper> mappers)
		{
			ThrowIfRoutesAreRegistered();

			mappers.ThrowIfNull("mappers");

			_routeRestrictionMappers.AddRange(mappers);

			return this;
		}

		public AutoRouteCollection RestrictionMappers(params IRouteRestrictionMapper[] mappers)
		{
			return RestrictionMappers((IEnumerable<IRouteRestrictionMapper>)mappers);
		}

		#endregion

		#region Response mappers

		public AutoRouteCollection RespondWithMethodReturnValuesThatImplementIRouteResponse(IEnumerable<IParameterMapper> mappers)
		{
			ThrowIfRoutesAreRegistered();

			mappers.ThrowIfNull("mappers");

			_routeResponseMapper = new RouteResponseMethodReturnTypeMapper(mappers);

			return this;
		}

		public AutoRouteCollection RespondWithMethodReturnValuesThatImplementIRouteResponse(params IParameterMapper[] mappers)
		{
			return RespondWithMethodReturnValuesThatImplementIRouteResponse((IEnumerable<IParameterMapper>)mappers);
		}

		public AutoRouteCollection ResponseMapper(IRouteResponseMapper mapper)
		{
			ThrowIfRoutesAreRegistered();

			mapper.ThrowIfNull("mapper");

			_routeResponseMapper = mapper;

			return this;
		}

		#endregion

		#region Bundles

		public AutoRouteCollection CssBundle(Bundle bundle, string routeName, string relativeUrl, IComparer<AssetFile> assetOrder, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			ThrowIfRoutesAreRegistered();
			ThrowIfNoBundleDependencyContainer();

			bundle.ThrowIfNull("bundle");
			routeName.ThrowIfNull("routeName");
			relativeUrl.ThrowIfNull("relativeUrl");
			assetOrder.ThrowIfNull("assetOrder");
			concatenator.ThrowIfNull("concatenator");
			transformers.ThrowIfNull("transformers");

			var watcher = new BundleWatcher(bundle, _bundleDependencyContainer.GetInstance<IFileSystem>(), assetOrder, concatenator, transformers);
			var route = new CssBundleWatcherRoute(routeName, _bundleDependencyContainer.GetInstance<IGuidFactory>(), relativeUrl, watcher, _bundleDependencyContainer.GetInstance<ISystemClock>());

			return AdditionalRoutes(route);
		}

		public AutoRouteCollection CssBundle(Bundle bundle, string routeName, string relativeUrl, IComparer<AssetFile> assetOrder, IAssetConcatenator concatenator, params IAssetTransformer[] transformers)
		{
			return CssBundle(bundle, routeName, relativeUrl, assetOrder, concatenator, (IEnumerable<IAssetTransformer>)transformers);
		}

		public AutoRouteCollection CssBundle(Bundle bundle, string routeName, string relativeUrl, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			ThrowIfRoutesAreRegistered();
			ThrowIfNoBundleDependencyContainer();

			bundle.ThrowIfNull("bundle");
			routeName.ThrowIfNull("routeName");
			relativeUrl.ThrowIfNull("relativeUrl");
			concatenator.ThrowIfNull("concatenator");
			transformers.ThrowIfNull("transformers");

			var watcher = new BundleWatcher(bundle, _bundleDependencyContainer.GetInstance<IFileSystem>(), concatenator, transformers);
			var route = new CssBundleWatcherRoute(routeName, _bundleDependencyContainer.GetInstance<IGuidFactory>(), relativeUrl, watcher, _bundleDependencyContainer.GetInstance<ISystemClock>());

			return AdditionalRoutes(route);
		}

		public AutoRouteCollection CssBundle(Bundle bundle, string routeName, string relativeUrl, IAssetConcatenator concatenator, params IAssetTransformer[] transformers)
		{
			return CssBundle(bundle, routeName, relativeUrl, concatenator, (IEnumerable<IAssetTransformer>)transformers);
		}

		public AutoRouteCollection JavaScriptBundle(Bundle bundle, string routeName, string relativeUrl, IComparer<AssetFile> assetOrder, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			ThrowIfRoutesAreRegistered();
			ThrowIfNoBundleDependencyContainer();

			bundle.ThrowIfNull("bundle");
			routeName.ThrowIfNull("routeName");
			relativeUrl.ThrowIfNull("relativeUrl");
			assetOrder.ThrowIfNull("assetOrder");
			concatenator.ThrowIfNull("concatenator");
			transformers.ThrowIfNull("transformers");

			var watcher = new BundleWatcher(bundle, _bundleDependencyContainer.GetInstance<IFileSystem>(), assetOrder, concatenator, transformers);
			var route = new JavaScriptBundleWatcherRoute(routeName, _bundleDependencyContainer.GetInstance<IGuidFactory>(), relativeUrl, watcher, _bundleDependencyContainer.GetInstance<ISystemClock>());

			return AdditionalRoutes(route);
		}

		public AutoRouteCollection JavaScriptBundle(Bundle bundle, string routeName, string relativeUrl, IComparer<AssetFile> assetOrder, IAssetConcatenator concatenator, params IAssetTransformer[] transformers)
		{
			return JavaScriptBundle(bundle, routeName, relativeUrl, assetOrder, concatenator, (IEnumerable<IAssetTransformer>)transformers);
		}

		public AutoRouteCollection JavaScriptBundle(Bundle bundle, string routeName, string relativeUrl, IAssetConcatenator concatenator, IEnumerable<IAssetTransformer> transformers)
		{
			ThrowIfRoutesAreRegistered();
			ThrowIfNoBundleDependencyContainer();

			bundle.ThrowIfNull("bundle");
			routeName.ThrowIfNull("routeName");
			relativeUrl.ThrowIfNull("relativeUrl");
			concatenator.ThrowIfNull("concatenator");
			transformers.ThrowIfNull("transformers");

			var watcher = new BundleWatcher(bundle, _bundleDependencyContainer.GetInstance<IFileSystem>(), concatenator, transformers);
			var route = new JavaScriptBundleWatcherRoute(routeName, _bundleDependencyContainer.GetInstance<IGuidFactory>(), relativeUrl, watcher, _bundleDependencyContainer.GetInstance<ISystemClock>());

			return AdditionalRoutes(route);
		}

		public AutoRouteCollection JavaScriptBundle(Bundle bundle, string routeName, string relativeUrl, IAssetConcatenator concatenator, params IAssetTransformer[] transformers)
		{
			return JavaScriptBundle(bundle, routeName, relativeUrl, concatenator, (IEnumerable<IAssetTransformer>)transformers);
		}

		#endregion

		#region Additional routes

		public AutoRouteCollection AdditionalRoutes(IEnumerable<Func<IRouteCollection, IEnumerable<Routing.Route>>> additionalRoutesForRegistration)
		{
			ThrowIfRoutesAreRegistered();

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
			ThrowIfRoutesAreRegistered();

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Junior.Common;
using Junior.Route.AutoRouting;
using Junior.Route.AutoRouting.AntiCsrf.HtmlGenerators;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.Routing.AntiCsrf;
using Junior.Route.Routing.AntiCsrf.CookieManagers;
using Junior.Route.Routing.AntiCsrf.HtmlGenerators;
using Junior.Route.Routing.AntiCsrf.NonceRepositories;
using Junior.Route.Routing.AntiCsrf.NonceValidators;
using Junior.Route.Routing.AntiCsrf.ResponseGenerators;

using JuniorRouteWebApplication.Endpoints;

namespace JuniorRouteWebApplication.Containers
{
	// This container does not support concrete types with generic type arguments
	// Developers are advised to replace this container with a more robust container, if necessary
	public class EndpointContainer : IContainer
	{
		private readonly Dictionary<Type, Type> _concreteTypesByRequestedType = new Dictionary<Type, Type>();
		private readonly Dictionary<Type, object> _instancesByRequestedType = new Dictionary<Type, object>();
		private readonly object _lockObject = new object();

		public EndpointContainer()
		{
			// Common
			AddMapping<IGuidFactory, GuidFactory>();
			AddMapping<ISystemClock, SystemClock>();
			AddMapping<IResponseContext, ResponseContext>();

			// Anti-CSRF
			AddMapping<IAntiCsrfConfiguration, ConfigurationSectionAntiCsrfConfiguration>();
			AddMapping<IAntiCsrfCookieManager, DefaultCookieManager>();
			AddMapping<IAntiCsrfHtmlGenerator, DefaultHtmlGenerator>();
			AddMapping<IAntiCsrfNonceRepository, MemoryCacheNonceRepository>();
			AddMapping<IAntiCsrfNonceValidator, DefaultNonceValidator>();
			AddMapping<IAntiCsrfResponseGenerator, DefaultResponseGenerator>();

			// Endpoints
			AddMapping<HelloWorld, HelloWorld>();
		}

		public T GetInstance<T>()
		{
			return (T)GetInstance(typeof(T));
		}

		public object GetInstance(Type type)
		{
			type.ThrowIfNull("type");

			lock (_lockObject)
			{
				object existingInstance;

				if (_instancesByRequestedType.TryGetValue(type, out existingInstance))
				{
					return existingInstance;
				}

				Type concreteType;

				if (!_concreteTypesByRequestedType.TryGetValue(type, out concreteType))
				{
					return null;
				}

				ConstructorInfo[] constructorInfos = concreteType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

				if (constructorInfos.Length == 0)
				{
					throw new ArgumentException(String.Format("No public instance constructors found for type {0}.", concreteType.FullName));
				}

				KeyValuePair<ConstructorInfo, ParameterInfo[]> pair = constructorInfos
					.Select(arg => new { constructorInfo = arg, parameterInfos = arg.GetParameters() })
					.ToDictionary(arg => arg.constructorInfo, arg => arg.parameterInfos)
					.OrderByDescending(arg => arg.Value.Length)
					.Single();
				IEnumerable<object> parameterValues = pair.Value.Select(parameterInfo =>
					{
						object parameterValue = GetInstance(parameterInfo.ParameterType) ?? (parameterInfo.HasDefaultValue ? parameterInfo.DefaultValue : null);

						if (parameterValue == null)
						{
							throw new ApplicationException(String.Format("Unable to map parameter '{0}' of type {1}.", parameterInfo, pair.Key.DeclaringType.FullName));
						}

						return parameterValue;
					});
				NewExpression newExpression = Expression.New(pair.Key, parameterValues.Select(Expression.Constant));
				object newInstance = Expression.Lambda(newExpression).Compile().DynamicInvoke();

				_instancesByRequestedType.Add(type, newInstance);

				return newInstance;
			}
		}

		private void AddMapping<TRequestedType, TConcreteType>()
			where TConcreteType : TRequestedType
		{
			lock (_lockObject)
			{
				_concreteTypesByRequestedType.Add(typeof(TRequestedType), typeof(TConcreteType));
			}
		}
	}
}
using System;
using System.Collections.Generic;

using Junior.Common;
using Junior.Route.Diagnostics.Web;

namespace Junior.Route.AspNetIntegration.Diagnostics.Web
{
	public abstract class AspNetView : View
	{
		public override string Title
		{
			get
			{
				return "ASP.net Integration Information - JuniorRoute";
			}
		}

		public Type CacheType
		{
			get;
			private set;
		}

		public IEnumerable<Type> ResponseGeneratorTypes
		{
			get;
			private set;
		}

		public IEnumerable<Type> CachedResponseHandlerTypes
		{
			get;
			private set;
		}

		public IEnumerable<Type> NonCachedResponseHandlerTypes
		{
			get;
			private set;
		}

		public void Populate(Type cacheType, IEnumerable<Type> responseGeneratorTypes, IEnumerable<Type> cachedResponseHandlerTypes, IEnumerable<Type> nonCachedResponseHandlerTypes)
		{
			cacheType.ThrowIfNull("cacheType");
			responseGeneratorTypes.ThrowIfNull("responseGeneratorTypes");
			cachedResponseHandlerTypes.ThrowIfNull("cachedResponseHandlerTypes");
			nonCachedResponseHandlerTypes.ThrowIfNull("nonCachedResponseHandlerTypes");

			CacheType = cacheType;
			ResponseGeneratorTypes = responseGeneratorTypes;
			CachedResponseHandlerTypes = cachedResponseHandlerTypes;
			NonCachedResponseHandlerTypes = nonCachedResponseHandlerTypes;
		}
	}
}
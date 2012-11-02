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

		public IEnumerable<Type> ResponseHandlerTypes
		{
			get;
			private set;
		}

		public void Populate(Type cacheType, IEnumerable<Type> responseGeneratorTypes, IEnumerable<Type> responseHandlerTypes)
		{
			cacheType.ThrowIfNull("cacheType");
			responseGeneratorTypes.ThrowIfNull("responseGeneratorTypes");
			responseHandlerTypes.ThrowIfNull("responseHandlerTypes");

			CacheType = cacheType;
			ResponseGeneratorTypes = responseGeneratorTypes;
			ResponseHandlerTypes = responseHandlerTypes;
		}
	}
}
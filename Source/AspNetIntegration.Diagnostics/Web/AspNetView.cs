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

		public IEnumerable<Type> RequestFilterTypes
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

		public IEnumerable<Type> ErrorHandlerTypes
		{
			get;
			private set;
		}

		public void Populate(Type cacheType, IEnumerable<Type> requestFilterTypes, IEnumerable<Type> responseGeneratorTypes, IEnumerable<Type> responseHandlerTypes, IEnumerable<Type> errorHandlerTypes)
		{
			cacheType.ThrowIfNull("cacheType");
			requestFilterTypes.ThrowIfNull("requestFilterTypes");
			responseGeneratorTypes.ThrowIfNull("responseGeneratorTypes");
			responseHandlerTypes.ThrowIfNull("responseHandlerTypes");
			errorHandlerTypes.ThrowIfNull("errorHandlerTypes");

			CacheType = cacheType;
			RequestFilterTypes = requestFilterTypes;
			ResponseGeneratorTypes = responseGeneratorTypes;
			ResponseHandlerTypes = responseHandlerTypes;
			ErrorHandlerTypes = errorHandlerTypes;
		}
	}
}
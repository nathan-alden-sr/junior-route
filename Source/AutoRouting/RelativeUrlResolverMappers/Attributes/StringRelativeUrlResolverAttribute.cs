using System;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RelativeUrlResolverMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class StringRelativeUrlResolverAttribute : RelativeUrlResolverAttribute
	{
		private readonly string _relativeUrl;

		public StringRelativeUrlResolverAttribute(string relativeUrl)
		{
			relativeUrl.ThrowIfNull("relativeUrl");

			_relativeUrl = relativeUrl;
		}

		public override void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			route.ResolveRelativeUrlsUsingString(_relativeUrl);
		}
	}
}
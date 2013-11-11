using System;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RelativeUrlResolverMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class FormatStringRelativeUrlResolverAttribute : RelativeUrlResolverAttribute
	{
		private readonly string _format;

		public FormatStringRelativeUrlResolverAttribute(string format)
		{
			format.ThrowIfNull("format");

			_format = format;
		}

		public override void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			route.ResolveRelativeUrlsUsingFormatString(_format);
		}
	}
}
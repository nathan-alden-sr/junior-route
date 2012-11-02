using System;
using System.Collections.Generic;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.Routing;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class UrlRelativePathAttribute : RestrictionAttribute
	{
		private readonly RequestValueComparer? _comparer;
		private readonly IEnumerable<string> _relativePaths;

		public UrlRelativePathAttribute(string relativePath, RequestValueComparer comparer = RequestValueComparer.CaseInsensitivePlain)
		{
			relativePath.ThrowIfNull("relativePath");

			_relativePaths = relativePath.ToEnumerable();
			_comparer = comparer;
		}

		public UrlRelativePathAttribute(IEnumerable<string> relativePaths)
		{
			relativePaths.ThrowIfNull("relativePaths");

			_relativePaths = relativePaths;
		}

		public UrlRelativePathAttribute(params string[] relativePaths)
			: this((IEnumerable<string>)relativePaths)
		{
		}

		public override void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			var httpRuntime = container.GetInstance<IHttpRuntime>();

			if (_comparer != null)
			{
				route.RestrictByRelativePaths(_relativePaths, GetComparer(_comparer.Value), httpRuntime);
			}
			else
			{
				route.RestrictByRelativePaths(_relativePaths, httpRuntime);
			}
		}
	}
}
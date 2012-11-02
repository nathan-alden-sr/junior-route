using System;
using System.Collections.Generic;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class RefererUrlAbsolutePathAttribute : RestrictionAttribute
	{
		private readonly IEnumerable<string> _absolutePaths;
		private readonly RequestValueComparer? _comparer;

		public RefererUrlAbsolutePathAttribute(string absolutePath, RequestValueComparer comparer)
		{
			_absolutePaths = absolutePath.ToEnumerable();
			_comparer = comparer;
		}

		public RefererUrlAbsolutePathAttribute(params string[] absolutePaths)
		{
			absolutePaths.ThrowIfNull("absolutePaths");

			_absolutePaths = absolutePaths;
		}

		public override void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			if (_comparer != null)
			{
				route.RestrictByRefererUrlAbsolutePaths(_absolutePaths, GetComparer(_comparer.Value));
			}
			else
			{
				route.RestrictByRefererUrlAbsolutePaths(_absolutePaths);
			}
		}
	}
}
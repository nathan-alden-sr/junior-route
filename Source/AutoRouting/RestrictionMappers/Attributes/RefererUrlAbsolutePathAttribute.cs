using System;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class RefererUrlAbsolutePathAttribute : RestrictionAttribute
	{
		private readonly string[] _absolutePaths;
		private readonly RequestValueComparer? _comparer;

		public RefererUrlAbsolutePathAttribute(string absolutePath, RequestValueComparer comparer)
		{
			absolutePath.ThrowIfNull("absolutePath");

			_absolutePaths = new[] { absolutePath };
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
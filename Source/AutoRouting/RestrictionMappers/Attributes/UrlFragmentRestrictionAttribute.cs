using System;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class UrlFragmentRestrictionAttribute : RestrictionAttribute
	{
		private readonly RequestValueComparer? _comparer;
		private readonly string[] _fragments;

		public UrlFragmentRestrictionAttribute(string fragment, RequestValueComparer comparer)
		{
			fragment.ThrowIfNull("fragment");

			_fragments = new[] { fragment };
			_comparer = comparer;
		}

		public UrlFragmentRestrictionAttribute(params string[] fragments)
		{
			fragments.ThrowIfNull("fragments");

			_fragments = fragments;
		}

		public override void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			if (_comparer != null)
			{
				route.RestrictByUrlFragments(_fragments, GetComparer(_comparer.Value));
			}
			else
			{
				route.RestrictByUrlFragments(_fragments);
			}
		}
	}
}
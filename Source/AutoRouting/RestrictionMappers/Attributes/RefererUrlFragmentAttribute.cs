using System;
using System.Collections.Generic;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class RefererUrlFragmentAttribute : RestrictionAttribute
	{
		private readonly RequestValueComparer? _comparer;
		private readonly IEnumerable<string> _fragments;

		public RefererUrlFragmentAttribute(string fragment, RequestValueComparer comparer)
		{
			_fragments = fragment.ToEnumerable();
			_comparer = comparer;
		}

		public RefererUrlFragmentAttribute(params string[] fragments)
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
				route.RestrictByRefererUrlFragments(_fragments, GetComparer(_comparer.Value));
			}
			else
			{
				route.RestrictByRefererUrlFragments(_fragments);
			}
		}
	}
}
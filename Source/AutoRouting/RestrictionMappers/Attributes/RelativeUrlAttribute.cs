using System;
using System.Collections.Generic;

using Junior.Common;
using Junior.Route.Routing.RequestValueComparers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class RelativeUrlAttribute : RestrictionAttribute
	{
		private readonly RequestValueComparer? _comparer;
		private readonly IEnumerable<string> _relativeUrls;

		public RelativeUrlAttribute(string relativeUrl, RequestValueComparer comparer = RequestValueComparer.CaseInsensitivePlain)
		{
			relativeUrl.ThrowIfNull("relativeUrl");

			_relativeUrls = relativeUrl.ToEnumerable();
			_comparer = comparer;
		}

		public RelativeUrlAttribute(IEnumerable<string> relativeUrls)
		{
			relativeUrls.ThrowIfNull("relativeUrls");

			_relativeUrls = relativeUrls;
		}

		public RelativeUrlAttribute(params string[] relativeUrls)
			: this((IEnumerable<string>)relativeUrls)
		{
		}

		public override void Map(Routing.Route route)
		{
			route.ThrowIfNull("route");

			if (_comparer != null)
			{
				IRequestValueComparer comparer = GetComparer(_comparer.Value);

				foreach (string relativeUrl in _relativeUrls)
				{
					route.RestrictByRelativeUrl(relativeUrl, comparer);
				}
			}
			else
			{
				route.RestrictByRelativeUrls(_relativeUrls);
			}
		}
	}
}
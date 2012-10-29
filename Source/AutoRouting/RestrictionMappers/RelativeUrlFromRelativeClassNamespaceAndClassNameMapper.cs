using System;
using System.Reflection;
using System.Text.RegularExpressions;

using Junior.Common;
using Junior.Route.Routing;
using Junior.Route.Routing.RequestValueComparers;

namespace Junior.Route.AutoRouting.RestrictionMappers
{
	public class RelativeUrlFromRelativeClassNamespaceAndClassNameMapper : IRouteRestrictionMapper
	{
		private readonly bool _makeLowercase;
		private readonly string _rootNamespace;

		public RelativeUrlFromRelativeClassNamespaceAndClassNameMapper(string rootNamespace, bool makeLowercase = true)
		{
			rootNamespace.ThrowIfNull("rootNamespace");

			_rootNamespace = rootNamespace;
			_makeLowercase = makeLowercase;
		}

		public void Map(Type type, MethodInfo method, Routing.Route route)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			route.ThrowIfNull("route");

			if (!type.NamespaceStartsWith(_rootNamespace))
			{
				return;
			}

			string relativeNamespace = Regex.Replace(type.Namespace, String.Format(@"^{0}\.?(?<RelativeNamespace>.*)", Regex.Escape(_rootNamespace)), "${RelativeNamespace}");
			string relativeUrl = String.Format(
				"{0}{1}{2}",
				_makeLowercase ? relativeNamespace.ToLowerInvariant() : relativeNamespace,
				relativeNamespace.Length > 0 ? "/" : "",
				_makeLowercase ? type.Name.ToLowerInvariant() : type.Name);

			route.RestrictByRelativeUrl(relativeUrl, CaseInsensitivePlainRequestValueComparer.Instance);
		}
	}
}
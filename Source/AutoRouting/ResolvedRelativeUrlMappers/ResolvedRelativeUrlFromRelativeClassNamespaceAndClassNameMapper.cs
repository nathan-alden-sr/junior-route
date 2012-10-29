using System;
using System.Reflection;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.AutoRouting.ResolvedRelativeUrlMappers
{
	public class ResolvedRelativeUrlFromRelativeClassNamespaceAndClassNameMapper : IResolvedRelativeUrlMapper
	{
		private readonly bool _makeLowercase;
		private readonly string _rootNamespace;

		public ResolvedRelativeUrlFromRelativeClassNamespaceAndClassNameMapper(string rootNamespace, bool makeLowercase = true)
		{
			rootNamespace.ThrowIfNull("rootNamespace");

			_rootNamespace = rootNamespace;
			_makeLowercase = makeLowercase;
		}

		public ResolvedRelativeUrlResult ResolvedRelativeUrl(Type type, MethodInfo method)
		{
			if (!type.NamespaceStartsWith(_rootNamespace))
			{
				return ResolvedRelativeUrlResult.ResolvedRelativeUrlNotMapped();
			}

			string relativeNamespace = Regex.Replace(type.Namespace, String.Format(@"^{0}\.?(?<RelativeNamespace>.*)", Regex.Escape(_rootNamespace)), "${RelativeNamespace}");
			string resolvedRelativeUrl = String.Format(
				"{0}{1}{2}",
				_makeLowercase ? relativeNamespace.ToLowerInvariant() : relativeNamespace,
				relativeNamespace.Length > 0 ? "/" : "",
				_makeLowercase ? type.Name.ToLowerInvariant() : type.Name);

			return ResolvedRelativeUrlResult.ResolvedRelativeUrlMapped(resolvedRelativeUrl);
		}
	}
}
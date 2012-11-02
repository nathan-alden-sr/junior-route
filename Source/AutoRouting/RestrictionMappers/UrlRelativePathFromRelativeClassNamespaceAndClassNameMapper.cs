using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.Routing;
using Junior.Route.Routing.RequestValueComparers;

namespace Junior.Route.AutoRouting.RestrictionMappers
{
	public class UrlRelativePathFromRelativeClassNamespaceAndClassNameMapper : IRestrictionMapper
	{
		public const string DefaultWordRegexPattern = @"\.|_|(?:(?<=[a-z])(?=[A-Z\d]))|(?:(?<=\d)(?=[A-Z]))|(?:(?<=[A-Z])(?=[A-Z][a-z]))";
		private readonly bool _caseSensitive;
		private readonly bool _makeLowercase;
		private readonly string _rootNamespace;
		private readonly string _wordRegexPattern;
		private readonly string _wordSeparator;

		public UrlRelativePathFromRelativeClassNamespaceAndClassNameMapper(string rootNamespace, bool caseSensitive = false, bool makeLowercase = true, string wordSeparator = "_", string wordRegexPattern = DefaultWordRegexPattern)
		{
			rootNamespace.ThrowIfNull("rootNamespace");
			wordSeparator.ThrowIfNull("wordSeparator");
			wordRegexPattern.ThrowIfNull("wordRegexPattern");

			_rootNamespace = rootNamespace;
			_caseSensitive = caseSensitive;
			_makeLowercase = makeLowercase;
			_wordSeparator = wordSeparator;
			_wordRegexPattern = wordRegexPattern;
		}

		public void Map(Type type, MethodInfo method, Routing.Route route, IContainer container)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			if (!type.NamespaceStartsWith(_rootNamespace))
			{
				return;
			}

			var pathParts = new List<string>();
			string relativeNamespace = Regex.Replace(type.Namespace, String.Format(@"^{0}\.?(?<RelativeNamespace>.*)", Regex.Escape(_rootNamespace)), "${RelativeNamespace}");

			pathParts.AddRange(ParseWords(relativeNamespace));
			pathParts.AddRange(ParseWords(type.Name));
			string relativePath = String.Join("/", pathParts);
			var httpRuntime = container.GetInstance<IHttpRuntime>();

			route.RestrictByRelativePath(relativePath, _caseSensitive ? (IRequestValueComparer)CaseSensitivePlainRequestValueComparer.Instance : CaseInsensitivePlainRequestValueComparer.Instance, httpRuntime);
		}

		private IEnumerable<string> ParseWords(string value)
		{
			return value.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(arg1 => Regex.Split(arg1, _wordRegexPattern).Where(arg2 => arg2.Length > 0))
				.Select(words => String.Join(_wordSeparator, words)).Select(path => _makeLowercase ? path.ToLowerInvariant() : path);
		}
	}
}
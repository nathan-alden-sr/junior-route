using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Junior.Common;

namespace Junior.Route.AutoRouting.ResolvedRelativeUrlMappers
{
	public class ResolvedRelativeUrlFromRelativeClassNamespaceAndClassNameMapper : IResolvedRelativeUrlMapper
	{
		public const string DefaultWordRegexPattern = @"\.|_|(?:(?<=[a-z])(?=[A-Z\d]))|(?:(?<=\d)(?=[A-Z]))|(?:(?<=[A-Z])(?=[A-Z][a-z]))";
		private readonly bool _makeLowercase;
		private readonly string _rootNamespace;
		private readonly string _wordRegexPattern;
		private readonly string _wordSeparator;

		public ResolvedRelativeUrlFromRelativeClassNamespaceAndClassNameMapper(string rootNamespace, bool makeLowercase = true, string wordSeparator = "_", string wordRegexPattern = DefaultWordRegexPattern)
		{
			rootNamespace.ThrowIfNull("rootNamespace");
			wordSeparator.ThrowIfNull("wordSeparator");

			_rootNamespace = rootNamespace;
			_makeLowercase = makeLowercase;
			_wordSeparator = wordSeparator;
			_wordRegexPattern = wordRegexPattern;
		}

		public Task<ResolvedRelativeUrlResult> MapAsync(Type type, MethodInfo method)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");

			if (!type.NamespaceStartsWith(_rootNamespace))
			{
				return ResolvedRelativeUrlResult.ResolvedRelativeUrlNotMapped().AsCompletedTask();
			}

			var pathParts = new List<string>();
			string relativeNamespace = Regex.Replace(type.Namespace, String.Format(@"^{0}\.?(?<RelativeNamespace>.*)", Regex.Escape(_rootNamespace)), "${RelativeNamespace}");

			pathParts.AddRange(ParseWords(relativeNamespace));
			pathParts.AddRange(ParseWords(type.Name));
			string resolvedRelativeUrl = String.Join("/", pathParts);

			return ResolvedRelativeUrlResult.ResolvedRelativeUrlMapped(resolvedRelativeUrl).AsCompletedTask();
		}

		private IEnumerable<string> ParseWords(string value)
		{
			return value.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(arg1 => Regex.Split(arg1, _wordRegexPattern).Where(arg2 => arg2.Length > 0))
				.Select(words => String.Join(_wordSeparator, words)).Select(path => _makeLowercase ? path.ToLowerInvariant() : path);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Junior.Common;

namespace Junior.Route.AutoRouting.NameMappers
{
	public class NameAfterRelativeClassNamespaceAndClassNameAndMethodNameMapper : INameMapper
	{
		public const string DefaultWordRegexPattern = @"\.|_|(?:(?<=[a-z])(?=[A-Z\d]))|(?:(?<=\d)(?=[A-Z]))|(?:(?<=[A-Z])(?=[A-Z][a-z]))";
		private readonly bool _makeLowercase;
		private readonly string _rootNamespace;
		private readonly string _wordRegexPattern;
		private readonly string _wordSeparator;

		public NameAfterRelativeClassNamespaceAndClassNameAndMethodNameMapper(string rootNamespace, bool makeLowercase = false, string wordSeparator = " ", string wordRegexPattern = DefaultWordRegexPattern)
		{
			rootNamespace.ThrowIfNull("rootNamespace");
			wordSeparator.ThrowIfNull("wordSeparator");
			wordRegexPattern.ThrowIfNull("wordRegexPattern");

			_rootNamespace = rootNamespace;
			_makeLowercase = makeLowercase;
			_wordSeparator = wordSeparator;
			_wordRegexPattern = wordRegexPattern;
		}

		public Task<NameResult> MapAsync(Type type, MethodInfo method)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");

			if (!type.NamespaceStartsWith(_rootNamespace))
			{
				return NameResult.NameNotMapped().AsCompletedTask();
			}

			var pathParts = new List<string>();
			string relativeNamespace = Regex.Replace(type.Namespace, String.Format(@"^{0}\.?(?<RelativeNamespace>.*)", Regex.Escape(_rootNamespace)), "${RelativeNamespace}");

			pathParts.AddRange(ParseWords(relativeNamespace));
			pathParts.AddRange(ParseWords(type.Name));
			pathParts.AddRange(ParseWords(method.Name.TrimEnd("Async")));
			string name = String.Join(_wordSeparator, pathParts);

			return NameResult.NameMapped(name).AsCompletedTask();
		}

		private IEnumerable<string> ParseWords(string value)
		{
			return value.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(arg1 => Regex.Split(arg1, _wordRegexPattern).Where(arg2 => arg2.Length > 0))
				.Select(words => String.Join(_wordSeparator, words)).Select(path => _makeLowercase ? path.ToLowerInvariant() : path);
		}
	}
}
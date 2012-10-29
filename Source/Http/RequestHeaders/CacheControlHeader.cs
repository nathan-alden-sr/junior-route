using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class CacheControlHeader
	{
		private const string CacheExtensionRegexPattern = CommonRegexPatterns.Token + "(?:=(?:" + CommonRegexPatterns.Token + "|" + CommonRegexPatterns.QuotedString + "))?";
		private const string RegexPattern = "(?:no-cache|no-store|max-age=" + CommonRegexPatterns.DeltaSeconds + "|max-stale(?:=" + CommonRegexPatterns.DeltaSeconds + ")?|min-fresh=" + CommonRegexPatterns.DeltaSeconds + "|no-transform|only-if-cached|(?:" + CacheExtensionRegexPattern + "))";
		private static readonly string _elementsRegexPattern = String.Format("^{0}$", CommonRegexPatterns.ListOfElements(RegexPattern, 1));
		private readonly IEnumerable<Parameter> _cacheExtensions;
		private readonly TimeSpan? _maxAge;
		private readonly int? _maxAgeSeconds;
		private readonly TimeSpan? _maxStale;
		private readonly int? _maxStaleSeconds;
		private readonly TimeSpan? _minFresh;
		private readonly int? _minFreshSeconds;
		private readonly bool _noCache;
		private readonly bool _noStore;
		private readonly bool _noTransform;
		private readonly bool _onlyIfCached;

		private CacheControlHeader(bool noCache, bool noStore, int? maxAgeSeconds, int? maxStaleSeconds, int? minFreshSeconds, bool noTransform, bool onlyIfCached, IEnumerable<Parameter> cacheExtensions)
		{
			cacheExtensions.ThrowIfNull("cacheExtensions");

			_noCache = noCache;
			_noStore = noStore;
			_maxAgeSeconds = maxAgeSeconds;
			_maxAge = maxAgeSeconds.IfNotNull(arg => (TimeSpan?)TimeSpan.FromSeconds(arg));
			_maxStaleSeconds = maxStaleSeconds;
			_maxStale = maxStaleSeconds.IfNotNull(arg => (TimeSpan?)TimeSpan.FromSeconds(arg));
			_minFreshSeconds = minFreshSeconds;
			_minFresh = minFreshSeconds.IfNotNull(arg => (TimeSpan?)TimeSpan.FromSeconds(arg));
			_noTransform = noTransform;
			_onlyIfCached = onlyIfCached;
			_cacheExtensions = cacheExtensions;
		}

		public bool NoCache
		{
			get
			{
				return _noCache;
			}
		}

		public bool NoStore
		{
			get
			{
				return _noStore;
			}
		}

		public int? MaxAgeSeconds
		{
			get
			{
				return _maxAgeSeconds;
			}
		}

		public TimeSpan? MaxAge
		{
			get
			{
				return _maxAge;
			}
		}

		public int? MaxStaleSeconds
		{
			get
			{
				return _maxStaleSeconds;
			}
		}

		public TimeSpan? MaxStale
		{
			get
			{
				return _maxStale;
			}
		}

		public int? MinFreshSeconds
		{
			get
			{
				return _minFreshSeconds;
			}
		}

		public TimeSpan? MinFresh
		{
			get
			{
				return _minFresh;
			}
		}

		public bool NoTransform
		{
			get
			{
				return _noTransform;
			}
		}

		public bool OnlyIfCached
		{
			get
			{
				return _onlyIfCached;
			}
		}

		public IEnumerable<Parameter> CacheExtensions
		{
			get
			{
				return _cacheExtensions;
			}
		}

		public static CacheControlHeader Parse(string headerValue)
		{
			if (headerValue == null || !Regex.IsMatch(headerValue, _elementsRegexPattern))
			{
				return null;
			}

			string[] elements = headerValue.SplitElements();
			bool noCache = false;
			bool noStore = false;
			int? maxAgeSeconds = null;
			int? maxStaleSeconds = null;
			int? minFreshSeconds = null;
			bool noTransform = false;
			bool onlyIfCached = false;
			var cacheExtension = new HashSet<Parameter>();

			foreach (string element in elements)
			{
				if (element == "no-cache")
				{
					noCache = true;
				}
				else if (element == "no-store")
				{
					noStore = true;
				}
				else if (element.StartsWith("max-age="))
				{
					maxAgeSeconds = element.GetParameterValue<int>();
				}
				else if (element == "max-stale")
				{
				}
				else if (element.StartsWith("max-stale="))
				{
					maxStaleSeconds = element.GetParameterValue<int>();
				}
				else if (element.StartsWith("min-fresh"))
				{
					minFreshSeconds = element.GetParameterValue<int>();
				}
				else if (element == "no-transform")
				{
					noTransform = true;
				}
				else if (element == "only-if-cached")
				{
					onlyIfCached = true;
				}
				else if (Regex.IsMatch(element, CacheExtensionRegexPattern))
				{
					string name;
					string value;

					element.GetParameterParts(out name, out value, true);

					cacheExtension.Add(new Parameter(name, value));
				}
				else
				{
					return null;
				}
			}

			return new CacheControlHeader(noCache, noStore, maxAgeSeconds, maxStaleSeconds, minFreshSeconds, noTransform, onlyIfCached, cacheExtension);
		}
	}
}
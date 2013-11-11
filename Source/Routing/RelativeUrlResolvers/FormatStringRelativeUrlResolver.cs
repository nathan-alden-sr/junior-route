using System;

using Junior.Common;

namespace Junior.Route.Routing.RelativeUrlResolvers
{
	public class FormatStringRelativeUrlResolver : IRelativeUrlResolver
	{
		private readonly string _format;

		public FormatStringRelativeUrlResolver(string format)
		{
			format.ThrowIfNull("format");

			_format = format;
		}

		public ResolveResult Resolve(params object[] args)
		{
			return ResolveResult.UrlResolved(args != null ? String.Format(_format, args) : _format);
		}
	}
}
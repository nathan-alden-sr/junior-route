using System;

using Junior.Common;

namespace Junior.Route.AutoRouting.ResolvedRelativeUrlMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class ResolvedRelativeUrlAttribute : Attribute
	{
		private readonly string _resolvedRelativeUrl;

		public ResolvedRelativeUrlAttribute(string resolvedRelativeUrl)
		{
			resolvedRelativeUrl.ThrowIfNull("resolvedRelativeUrl");

			_resolvedRelativeUrl = resolvedRelativeUrl;
		}

		public string ResolvedRelativeUrl
		{
			get
			{
				return _resolvedRelativeUrl;
			}
		}
	}
}
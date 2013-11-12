using System;
using System.IO;

using Junior.Common;
using Junior.Route.Routing;

namespace Junior.Route.ViewEngines.Razor.Routing.TemplatePathResolvers
{
	public abstract class TemplatePathResolver : ITemplatePathResolver
	{
		private readonly string _extension;
		private readonly IHttpRuntime _httpRuntime;

		protected TemplatePathResolver(string extension, IHttpRuntime httpRuntime)
		{
			extension.ThrowIfNull("extension");
			httpRuntime.ThrowIfNull("httpRuntime");

			_extension = extension;
			_httpRuntime = httpRuntime;
		}

		public string Absolute(string relativePath)
		{
			relativePath.ThrowIfNull("relativePath");

			string extension = Path.GetExtension(relativePath);
			string absolutePathWithoutExtension = Path.Combine(_httpRuntime.AppDomainAppPath, Path.GetDirectoryName(relativePath), Path.GetFileNameWithoutExtension(relativePath));

			return absolutePathWithoutExtension + (!String.IsNullOrEmpty(extension) ? extension : _extension);
		}
	}
}
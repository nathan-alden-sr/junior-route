using System;

using Junior.Common;

namespace Junior.Route.ViewEngines.Razor.TemplateCodeBuilders
{
	public class TemplateCodeBuilderFactory : ITemplateCodeBuilderFactory
	{
		public ITemplateCodeBuilder CreateFromFileExtension(string extension)
		{
			extension.ThrowIfNull("extension");

			if (String.Equals(extension, ".cshtml", StringComparison.OrdinalIgnoreCase))
			{
				return new CSharpBuilder();
			}
			if (String.Equals(extension, ".vbhtml", StringComparison.OrdinalIgnoreCase))
			{
				return new VisualBasicBuilder();
			}

			throw new ArgumentException(String.Format("Unknown file extension '{0}'.", extension), "extension");
		}
	}
}
using System;
using System.CodeDom.Compiler;

using Junior.Common;

using Microsoft.CSharp;
using Microsoft.VisualBasic;

namespace Junior.Route.ViewEngines.Razor.CodeDomProviderFactories
{
	public class FileExtensionFactory : ICodeDomProviderFactory
	{
		public CodeDomProvider CreateFromFileExtension(string extension)
		{
			extension.ThrowIfNull("extension");

			if (String.Equals(extension, ".cshtml", StringComparison.OrdinalIgnoreCase))
			{
				return new CSharpCodeProvider();
			}
			if (String.Equals(extension, ".vbhtml", StringComparison.OrdinalIgnoreCase))
			{
				return new VBCodeProvider();
			}

			throw new ArgumentException(String.Format("Unknown file extension '{0}'.", extension), "extension");
		}
	}
}
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using Junior.Route.ViewEngines.Razor.TemplateCodeBuilders;

namespace Junior.Route.ViewEngines.Razor.TemplateCompilers
{
	public interface ITemplateCompiler
	{
		Type Compile<TTemplate>(
			string templateContents,
			string className,
			ITemplateCodeBuilder templateCodeBuilder,
			CodeDomProvider codeDomProvider,
			Action<CodeTypeDeclaration> typeConfigurationDelegate,
			IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate;

		Type Compile<TTemplate>(
			string templateContents,
			string className,
			ITemplateCodeBuilder templateCodeBuilder,
			CodeDomProvider codeDomProvider,
			Action<CodeTypeDeclaration> typeConfigurationDelegate,
			params string[] namespaceImports)
			where TTemplate : ITemplate;
	}
}
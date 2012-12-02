using System;
using System.CodeDom;
using System.Collections.Generic;

namespace Junior.Route.ViewEngines.Razor.TemplateCodeBuilders
{
	public interface ITemplateCodeBuilder
	{
		BuildCodeResult BuildCode<TTemplate>(string templateContents, string className, Action<CodeTypeDeclaration> typeConfigurationDelegate, IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate;

		BuildCodeResult BuildCode<TTemplate>(string templateContents, string className, Action<CodeTypeDeclaration> typeConfigurationDelegate, params string[] namespaceImports)
			where TTemplate : ITemplate;
	}
}
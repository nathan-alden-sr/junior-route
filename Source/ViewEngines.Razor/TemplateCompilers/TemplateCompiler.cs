using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

using Junior.Common;
using Junior.Route.ViewEngines.Razor.TemplateAssemblyReferenceResolvers;
using Junior.Route.ViewEngines.Razor.TemplateCodeBuilders;

namespace Junior.Route.ViewEngines.Razor.TemplateCompilers
{
	public class TemplateCompiler : ITemplateCompiler
	{
		private readonly ITemplateAssemblyReferenceResolver _referenceResolver;

		public TemplateCompiler(ITemplateAssemblyReferenceResolver referenceResolver)
		{
			referenceResolver.ThrowIfNull("referenceResolver");

			_referenceResolver = referenceResolver;
		}

		public Type Compile<TTemplate>(
			string templateContents,
			string className,
			ITemplateCodeBuilder templateCodeBuilder,
			CodeDomProvider codeDomProvider,
			Action<CodeTypeDeclaration> typeConfigurationDelegate,
			IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate
		{
			templateContents.ThrowIfNull("templateContents");
			className.ThrowIfNull("className");
			templateCodeBuilder.ThrowIfNull("templateCodeBuilder");
			codeDomProvider.ThrowIfNull("codeDomProvider");
			namespaceImports.ThrowIfNull("namespaceImports");

			BuildCodeResult buildCodeResult = templateCodeBuilder.BuildCode<TTemplate>(templateContents, className, typeConfigurationDelegate, namespaceImports);
			var parameters = new CompilerParameters
			{
				GenerateInMemory = true,
				GenerateExecutable = false,
				IncludeDebugInformation = false,
				CompilerOptions = "/target:library /optimize"
			};
			string[] assemblyLocations = _referenceResolver.ResolveAssemblyLocations().ToArray();

			parameters.ReferencedAssemblies.AddRange(assemblyLocations);

			CompilerResults compilerResults = codeDomProvider.CompileAssemblyFromDom(parameters, buildCodeResult.CompileUnit);

			if (compilerResults.Errors != null && compilerResults.Errors.Count > 0)
			{
				throw new TemplateCompilationException(compilerResults.Errors);
			}

			return compilerResults.CompiledAssembly.GetType(buildCodeResult.TypeFullName);
		}

		public Type Compile<TTemplate>(
			string templateContents,
			string className,
			ITemplateCodeBuilder templateCodeBuilder,
			CodeDomProvider codeDomProvider,
			Action<CodeTypeDeclaration> typeConfigurationDelegate,
			params string[] namespaceImports)
			where TTemplate : ITemplate
		{
			return Compile<TTemplate>(templateContents, className, templateCodeBuilder, codeDomProvider, typeConfigurationDelegate, (IEnumerable<string>)namespaceImports);
		}
	}
}
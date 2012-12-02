using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Junior.Common;
using Junior.Route.Common;
using Junior.Route.ViewEngines.Razor.CodeDomProviderFactories;
using Junior.Route.ViewEngines.Razor.CompiledTemplateFactories;
using Junior.Route.ViewEngines.Razor.Routing.TemplatePathResolvers;
using Junior.Route.ViewEngines.Razor.TemplateClassNameBuilders;
using Junior.Route.ViewEngines.Razor.TemplateCodeBuilders;
using Junior.Route.ViewEngines.Razor.TemplateCompilers;

namespace Junior.Route.ViewEngines.Razor.Routing.TemplateRepositories
{
	public class FileSystemRepository : ITemplateRepository
	{
		private readonly ITemplateClassNameBuilder _classNameBuilder;
		private readonly ITemplateCodeBuilder _codeBuilder;
		private readonly ICodeDomProviderFactory _codeDomProviderFactory;
		private readonly ICompiledTemplateFactory _compiledTemplateFactory;
		private readonly ITemplateCompiler _compiler;
		private readonly IFileSystem _fileSystem;
		private readonly object _lockObject = new object();
		private readonly ITemplatePathResolver _pathResolver;
		private readonly Dictionary<string, Type> _templateTypesByAbsolutePath = new Dictionary<string, Type>();

		public FileSystemRepository(
			ITemplatePathResolver pathResolver,
			IFileSystem fileSystem,
			ITemplateCompiler compiler,
			ITemplateClassNameBuilder classNameBuilder,
			ITemplateCodeBuilder codeBuilder,
			ICodeDomProviderFactory codeDomProviderFactory,
			ICompiledTemplateFactory compiledTemplateFactory)
		{
			pathResolver.ThrowIfNull("pathResolver");
			fileSystem.ThrowIfNull("fileSystem");
			compiler.ThrowIfNull("compiler");
			classNameBuilder.ThrowIfNull("classNameBuilder");
			codeBuilder.ThrowIfNull("codeBuilder");
			codeDomProviderFactory.ThrowIfNull("codeDomProviderFactory");
			compiledTemplateFactory.ThrowIfNull("compiledTemplateFactory");

			_pathResolver = pathResolver;
			_fileSystem = fileSystem;
			_compiler = compiler;
			_classNameBuilder = classNameBuilder;
			_codeBuilder = codeBuilder;
			_codeDomProviderFactory = codeDomProviderFactory;
			_compiledTemplateFactory = compiledTemplateFactory;
		}

		public string Execute<TTemplate>(string relativePath, IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate
		{
			relativePath.ThrowIfNull("relativePath");
			namespaceImports.ThrowIfNull("namespaceImports");

			string absolutePath = _pathResolver.Absolute(relativePath);
			Type type;

			lock (_lockObject)
			{
				if (!_templateTypesByAbsolutePath.TryGetValue(absolutePath, out type))
				{
					string extension = Path.GetExtension(absolutePath);
					string templateContents;

					using (var reader = new StreamReader(_fileSystem.OpenFile(absolutePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
					{
						templateContents = reader.ReadToEnd();
					}

					string className = _classNameBuilder.BuildFromRandomGuid();
					CodeDomProvider codeDomProvider = _codeDomProviderFactory.CreateFromFileExtension(extension);

					type = _compiler.Compile<TTemplate>(templateContents, className, _codeBuilder, codeDomProvider, null, namespaceImports);

					_templateTypesByAbsolutePath.Add(absolutePath, type);
				}
			}

			ITemplate template = _compiledTemplateFactory.CreateFromType(type);

			template.Execute();

			return template.Contents;
		}

		public string Execute<TTemplate>(string relativePath)
			where TTemplate : ITemplate
		{
			return Execute<TTemplate>(relativePath, Enumerable.Empty<string>());
		}

		public string Execute<TTemplate, TModel>(string relativePath, TModel model, IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate<TModel>
		{
			relativePath.ThrowIfNull("relativePath");
			namespaceImports.ThrowIfNull("namespaceImports");

			string absolutePath = _pathResolver.Absolute(relativePath);
			Type type;

			lock (_lockObject)
			{
				if (!_templateTypesByAbsolutePath.TryGetValue(absolutePath, out type))
				{
					string extension = Path.GetExtension(absolutePath);
					string templateContents;

					using (var reader = new StreamReader(_fileSystem.OpenFile(absolutePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
					{
						templateContents = reader.ReadToEnd();
					}

					string className = _classNameBuilder.BuildFromRandomGuid();
					CodeDomProvider codeDomProvider = _codeDomProviderFactory.CreateFromFileExtension(extension);
					bool isAnonymousType = typeof(TModel).IsAnonymousType();

					type = _compiler.Compile<TTemplate>(
						templateContents,
						className,
						_codeBuilder,
						codeDomProvider,
						isAnonymousType ? (Action<CodeTypeDeclaration>)(arg => arg.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(DynamicModelAttribute))))) : null,
						namespaceImports);

					_templateTypesByAbsolutePath.Add(absolutePath, type);
				}
			}

			ITemplate<TModel> template = _compiledTemplateFactory.CreateFromType<TModel>(type);

			if (template == null)
			{
				throw new ArgumentException(String.Format("Model type {0} does not match template type {1}.", typeof(TModel).FullName, typeof(ITemplate<TModel>)), "model");
			}

			template.Model = model;
			template.Execute();

			return template.Contents;
		}

		public string Execute<TTemplate, TModel>(string relativePath, TModel model)
			where TTemplate : ITemplate<TModel>
		{
			return Execute<TTemplate, TModel>(relativePath, model, Enumerable.Empty<string>());
		}

		public string Execute(string relativePath, IEnumerable<string> namespaceImports)
		{
			return Execute<Template>(relativePath, namespaceImports);
		}

		public string Execute(string relativePath)
		{
			return Execute<Template>(relativePath, Enumerable.Empty<string>());
		}

		public string Execute<TModel>(string relativePath, TModel model, IEnumerable<string> namespaceImports)
		{
			return Execute<Template<TModel>, TModel>(relativePath, model, namespaceImports);
		}

		public string Execute<TModel>(string relativePath, TModel model)
		{
			return Execute<Template<TModel>, TModel>(relativePath, model, Enumerable.Empty<string>());
		}
	}
}
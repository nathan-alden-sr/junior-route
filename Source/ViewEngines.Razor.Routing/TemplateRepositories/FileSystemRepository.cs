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
using Junior.Route.ViewEngines.Razor.TemplateRepositories;

namespace Junior.Route.ViewEngines.Razor.Routing.TemplateRepositories
{
	public class FileSystemRepository : ITemplateRepository
	{
		private readonly ITemplateClassNameBuilder _classNameBuilder;
		private readonly ITemplateCodeBuilder _codeBuilder;
		private readonly ICodeDomProviderFactory _codeDomProviderFactory;
		private readonly ICompiledTemplateFactory _compiledTemplateFactory;
		private readonly ITemplateCompiler _compiler;
		private readonly IFileSystemRepositoryConfiguration _configuration;
		private readonly IFileSystem _fileSystem;
		private readonly Dictionary<string, FileSystemWatcher> _fileSystemWatchersByAbsolutePath = new Dictionary<string, FileSystemWatcher>();
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
			ICompiledTemplateFactory compiledTemplateFactory,
			IFileSystemRepositoryConfiguration configuration)
		{
			pathResolver.ThrowIfNull("pathResolver");
			fileSystem.ThrowIfNull("fileSystem");
			compiler.ThrowIfNull("compiler");
			classNameBuilder.ThrowIfNull("classNameBuilder");
			codeBuilder.ThrowIfNull("codeBuilder");
			codeDomProviderFactory.ThrowIfNull("codeDomProviderFactory");
			compiledTemplateFactory.ThrowIfNull("compiledTemplateFactory");
			configuration.ThrowIfNull("configuration");

			_pathResolver = pathResolver;
			_fileSystem = fileSystem;
			_compiler = compiler;
			_classNameBuilder = classNameBuilder;
			_codeBuilder = codeBuilder;
			_codeDomProviderFactory = codeDomProviderFactory;
			_compiledTemplateFactory = compiledTemplateFactory;
			_configuration = configuration;
		}

		public TTemplate Get<TTemplate>(string relativePath, IEnumerable<string> namespaceImports)
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

					if (_configuration.ReloadChangedTemplateFiles)
					{
						AddFileSystemWatcher(absolutePath);
					}
				}
			}

			var template = (TTemplate)_compiledTemplateFactory.CreateFromType(type);

			template.TemplateRepository = this;

			return template;
		}

		public TTemplate Get<TTemplate>(string relativePath)
			where TTemplate : ITemplate
		{
			return Get<TTemplate>(relativePath, Enumerable.Empty<string>());
		}

		public TTemplate Get<TTemplate, TModel>(string relativePath, TModel model, IEnumerable<string> namespaceImports)
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

					if (_configuration.ReloadChangedTemplateFiles)
					{
						AddFileSystemWatcher(absolutePath);
					}
				}
			}

			ITemplate<TModel> template = (TTemplate)_compiledTemplateFactory.CreateFromType<TModel>(type);

			if (template == null)
			{
				throw new ArgumentException(String.Format("Model type {0} does not match template type {1}.", typeof(TModel).FullName, typeof(ITemplate<TModel>)), "model");
			}

			template.TemplateRepository = this;
			template.Model = model;

			return (TTemplate)template;
		}

		public TTemplate Get<TTemplate, TModel>(string relativePath, TModel model)
			where TTemplate : ITemplate<TModel>
		{
			return Get<TTemplate, TModel>(relativePath, model, Enumerable.Empty<string>());
		}

		public Template Get(string relativePath, IEnumerable<string> namespaceImports)
		{
			return Get<Template>(relativePath, namespaceImports);
		}

		public Template Get(string relativePath)
		{
			return Get<Template>(relativePath, Enumerable.Empty<string>());
		}

		public Template<TModel> Get<TModel>(string relativePath, TModel model, IEnumerable<string> namespaceImports)
		{
			return Get<Template<TModel>, TModel>(relativePath, model, namespaceImports);
		}

		public Template<TModel> Get<TModel>(string relativePath, TModel model)
		{
			return Get<Template<TModel>, TModel>(relativePath, model, Enumerable.Empty<string>());
		}

		public string Run<TTemplate>(string relativePath, IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate
		{
			return Get<TTemplate>(relativePath, namespaceImports).Run();
		}

		public string Run<TTemplate>(string relativePath)
			where TTemplate : ITemplate
		{
			return Get<TTemplate>(relativePath).Run();
		}

		public string Run<TTemplate, TModel>(string relativePath, TModel model, IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate<TModel>
		{
			return Get<TTemplate, TModel>(relativePath, model, namespaceImports).Run();
		}

		public string Run<TTemplate, TModel>(string relativePath, TModel model)
			where TTemplate : ITemplate<TModel>
		{
			return Get<TTemplate, TModel>(relativePath, model).Run();
		}

		public string Run(string relativePath, IEnumerable<string> namespaceImports)
		{
			return Get(relativePath, namespaceImports).Run();
		}

		public string Run(string relativePath)
		{
			return Get(relativePath).Run();
		}

		public string Run<TModel>(string relativePath, TModel model, IEnumerable<string> namespaceImports)
		{
			return Get(relativePath, model, namespaceImports).Run();
		}

		public string Run<TModel>(string relativePath, TModel model)
		{
			return Get(relativePath, model).Run();
		}

		private void AddFileSystemWatcher(string absolutePath)
		{
			var fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(absolutePath), Path.GetFileName(absolutePath))
				{
					EnableRaisingEvents = false,
					IncludeSubdirectories = false,
					NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size
				};

			_fileSystemWatchersByAbsolutePath.Add(absolutePath, fileSystemWatcher);

			fileSystemWatcher.Changed += FileSystemWatcherOnNotification;
			fileSystemWatcher.Created += FileSystemWatcherOnNotification;
			fileSystemWatcher.Deleted += FileSystemWatcherOnNotification;
			fileSystemWatcher.Renamed += FileSystemWatcherOnNotification;
			fileSystemWatcher.EnableRaisingEvents = true;
		}

		private void FileSystemWatcherOnNotification(object sender, FileSystemEventArgs e)
		{
			lock (_lockObject)
			{
				if (_fileSystemWatchersByAbsolutePath.ContainsKey(e.FullPath))
				{
					throw new Exception(String.Format("Received a notification for '{0}' but the path does not exist in the file system watcher collection.", e.FullPath));
				}

				FileSystemWatcher fileSystemWatcher = _fileSystemWatchersByAbsolutePath[e.FullPath];

				fileSystemWatcher.EnableRaisingEvents = false;
				_fileSystemWatchersByAbsolutePath.Remove(e.FullPath);
				_templateTypesByAbsolutePath.Remove(e.FullPath);
			}
		}
	}
}
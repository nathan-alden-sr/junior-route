using System;
using System.Configuration;
using System.Web.Configuration;

using Junior.Common;
using Junior.Route.AspNetIntegration;
using Junior.Route.AutoRouting.Containers;
using Junior.Route.Routing;
using Junior.Route.ViewEngines.Razor.CodeDomProviderFactories;
using Junior.Route.ViewEngines.Razor.CompiledTemplateFactories;
using Junior.Route.ViewEngines.Razor.Routing.TemplatePathResolvers;
using Junior.Route.ViewEngines.Razor.Routing.TemplateRepositories;
using Junior.Route.ViewEngines.Razor.TemplateAssemblyReferenceResolvers;
using Junior.Route.ViewEngines.Razor.TemplateClassNameBuilders;
using Junior.Route.ViewEngines.Razor.TemplateCodeBuilders;
using Junior.Route.ViewEngines.Razor.TemplateCompilers;

using JuniorRouteWebApplication.Endpoints;

namespace JuniorRouteWebApplication.Containers
{
	public class EndpointContainer : IContainer
	{
		private readonly FileSystemRepository _fileSystemRepository;

		public EndpointContainer(IHttpRuntime httpRuntime, IGuidFactory guidFactory)
		{
			var pathResolver = new CSharpResolver(httpRuntime);
			var fileSystem = new FileSystem(httpRuntime);
			var appDomainResolver = new AppDomainResolver();
			var compiler = new TemplateCompiler(appDomainResolver);
			var classNameBuilder = new RandomGuidBuilder(guidFactory);
			var codeBuilder = new CSharpBuilder();
			var codeDomProviderFactory = new FileExtensionFactory();
			var compiledTemplateFactory = new ActivatorFactory();
			var compilationSection = (CompilationSection)ConfigurationManager.GetSection("system.web/compilation");
			bool reloadChangedTemplateFiles = compilationSection != null && compilationSection.Debug;
			var fileSystemRepositoryConfiguration = new FileSystemRepositoryConfiguration(reloadChangedTemplateFiles);

			_fileSystemRepository = new FileSystemRepository(
				pathResolver,
				fileSystem,
				compiler,
				classNameBuilder,
				codeBuilder,
				codeDomProviderFactory,
				compiledTemplateFactory,
				fileSystemRepositoryConfiguration);
		}

		public T GetInstance<T>()
		{
			return (T)GetInstance(typeof(T));
		}

		public object GetInstance(Type type)
		{
			if (type == typeof(HelloWorld))
			{
				return new HelloWorld(_fileSystemRepository);
			}

			throw new ArgumentException(String.Format("Could not get instance for {0}.", type.FullName), "type");
		}
	}
}
using System.Configuration;
using System.Web.Configuration;

namespace Junior.Route.ViewEngines.Razor.Routing.TemplateRepositories
{
	public class DebugAttributeConfiguration : IFileSystemRepositoryConfiguration
	{
		private readonly bool _reloadChangedTemplateFiles;

		public DebugAttributeConfiguration()
		{
			var compilationSection = (CompilationSection)ConfigurationManager.GetSection("system.web/compilation");

			_reloadChangedTemplateFiles = compilationSection != null && compilationSection.Debug;
		}

		public bool ReloadChangedTemplateFiles
		{
			get
			{
				return _reloadChangedTemplateFiles;
			}
		}
	}
}
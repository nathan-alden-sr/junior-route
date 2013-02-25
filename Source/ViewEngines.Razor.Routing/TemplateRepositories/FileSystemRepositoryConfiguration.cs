namespace Junior.Route.ViewEngines.Razor.Routing.TemplateRepositories
{
	public class FileSystemRepositoryConfiguration : IFileSystemRepositoryConfiguration
	{
		private readonly bool _reloadChangedTemplateFiles;

		public FileSystemRepositoryConfiguration(bool reloadChangedTemplateFiles = false)
		{
			_reloadChangedTemplateFiles = reloadChangedTemplateFiles;
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
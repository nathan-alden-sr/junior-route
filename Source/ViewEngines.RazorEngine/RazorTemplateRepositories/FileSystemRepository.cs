using System;
using System.Collections.Generic;
using System.IO;

using Junior.Common;
using Junior.Route.Common;
using Junior.Route.ViewEngines.RazorEngine.RazorTemplateNameMappers;

namespace Junior.Route.ViewEngines.RazorEngine.RazorTemplateRepositories
{
	public class FileSystemRepository : IRazorTemplateRepository
	{
		private readonly Dictionary<string, IRazorTemplate> _razorTemplatesByName = new Dictionary<string, IRazorTemplate>();

		public FileSystemRepository(string viewRelativeRootDirectory, string searchPattern, SearchOption searchOption, IRazorTemplateNameMapper nameMapper, IFileSystem fileSystem)
		{
			viewRelativeRootDirectory.ThrowIfNull("viewRelativeRootDirectory");
			searchPattern.ThrowIfNull("searchPattern");
			nameMapper.ThrowIfNull("nameMapper");
			fileSystem.ThrowIfNull("fileSystem");

			string viewRootDirectory = fileSystem.AbsolutePath(viewRelativeRootDirectory);

			if (!fileSystem.DirectoryExists(viewRootDirectory))
			{
				throw new ApplicationException(String.Format("Invalid view directory '{0}'.", viewRootDirectory));
			}

			string[] paths = fileSystem.GetDirectoryFiles(viewRootDirectory, searchPattern, searchOption);

			foreach (string path in paths)
			{
				string relativePath = path.Remove(0, viewRootDirectory.Length + 1);
				string name = nameMapper.Map(relativePath);

				if (_razorTemplatesByName.ContainsKey(name))
				{
					throw new ApplicationException(String.Format("The key '{0}' already exists in the repository.", name));
				}

				_razorTemplatesByName.Add(name, new RazorTemplate(name, path, fileSystem));
			}
		}

		public IRazorTemplate GetByName(string name)
		{
			name.ThrowIfNull("name");

			IRazorTemplate template;

			if (!_razorTemplatesByName.TryGetValue(name, out template))
			{
				throw new ArgumentException(String.Format("No template named '{0}' exists in the repository.", name), "name");
			}

			return template;
		}
	}
}
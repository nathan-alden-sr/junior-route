using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.CSharp.RuntimeBinder;

namespace Junior.Route.ViewEngines.Razor.TemplateAssemblyReferenceResolvers
{
	public class AppDomainResolver : ITemplateAssemblyReferenceResolver
	{
		public IEnumerable<string> ResolveAssemblyLocations()
		{
			var assemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies().Where(arg => !arg.IsDynamic && !String.IsNullOrEmpty(arg.Location)))
				{
					typeof(RuntimeBinderException).Assembly
				};

			return assemblies.Select(arg => arg.Location).ToArray();
		}
	}
}
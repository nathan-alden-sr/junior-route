using System.CodeDom.Compiler;

namespace Junior.Route.ViewEngines.Razor.CodeDomProviderFactories
{
	public interface ICodeDomProviderFactory
	{
		CodeDomProvider CreateFromFileExtension(string extension);
	}
}
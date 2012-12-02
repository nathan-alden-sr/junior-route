using System;

namespace Junior.Route.ViewEngines.Razor.CompiledTemplateFactories
{
	public interface ICompiledTemplateFactory
	{
		ITemplate CreateFromType(Type type);
		ITemplate<TModel> CreateFromType<TModel>(Type type);
	}
}
using System;

using Junior.Common;

namespace Junior.Route.ViewEngines.Razor.CompiledTemplateFactories
{
	public class ActivatorFactory : ICompiledTemplateFactory
	{
		public ITemplate CreateFromType(Type type)
		{
			type.ThrowIfNull("type");

			var template = Activator.CreateInstance(type) as ITemplate;

			if (template == null)
			{
				throw new ArgumentException(String.Format("Type {0} does not implement {1}.", type.FullName, typeof(ITemplate).FullName), "type");
			}

			return template;
		}

		public ITemplate<TModel> CreateFromType<TModel>(Type type)
		{
			type.ThrowIfNull("type");

			var template = Activator.CreateInstance(type) as ITemplate<TModel>;

			if (template == null)
			{
				throw new ArgumentException(String.Format("Type {0} does not implement {1}.", type.FullName, typeof(ITemplate<TModel>).FullName), "type");
			}

			return template;
		}
	}
}
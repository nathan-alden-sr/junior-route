using Junior.Common;
using Junior.Route.Routing.Responses;

using RazorEngine;
using RazorEngine.Templating;

namespace Junior.Route.ViewEngines.RazorEngine
{
	public class RazorHtmlResponse<T> : RazorHtmlResponseBase
	{
		public RazorHtmlResponse(IRazorTemplate template, T model)
			: base(GetResponse(template, model))
		{
		}

		private static Response GetResponse(IRazorTemplate template, T model)
		{
			template.ThrowIfNull("template");

			lock (LockObject)
			{
				ITemplate resolvedTemplate = Razor.Resolve(template.Name, model);

				if (resolvedTemplate == null)
				{
					string templateContents = template.GetContents();

					Razor.Compile<T>(templateContents, template.Name);
				}
			}

			return Response
				.OK()
				.TextHtml()
				.Content(() => Razor.Run(template.Name, model));
		}
	}

	public class RazorHtmlResponse : RazorHtmlResponseBase
	{
		public RazorHtmlResponse(IRazorTemplate template)
			: base(GetResponse(template))
		{
		}

		private static Response GetResponse(IRazorTemplate template)
		{
			template.ThrowIfNull("template");

			lock (LockObject)
			{
				ITemplate resolvedTemplate = Razor.Resolve(template.Name);

				if (resolvedTemplate == null)
				{
					string templateContents = template.GetContents();

					Razor.Compile(templateContents, template.Name);
				}
			}

			return Response
				.OK()
				.TextHtml()
				.Content(() => Razor.Run(template.Name));
		}
	}
}
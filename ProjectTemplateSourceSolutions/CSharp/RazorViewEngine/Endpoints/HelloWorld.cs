using System.Threading.Tasks;

using Junior.Common;
using Junior.Route.Routing.AntiCsrf.HtmlGenerators;
using Junior.Route.Routing.Responses.Text;
using Junior.Route.ViewEngines.Razor.TemplateRepositories;

namespace JuniorRouteWebApplication.Endpoints
{
	public class HelloWorld
	{
		private readonly IAntiCsrfHtmlGenerator _antiCsrfHtmlGenerator;
		private readonly ITemplateRepository _templateRepository;

		public HelloWorld(ITemplateRepository templateRepository, IAntiCsrfHtmlGenerator antiCsrfHtmlGenerator)
		{
			templateRepository.ThrowIfNull("templateRepository");
			antiCsrfHtmlGenerator.ThrowIfNull("antiCsrfHtmlGenerator");

			_templateRepository = templateRepository;
			_antiCsrfHtmlGenerator = antiCsrfHtmlGenerator;
		}

		public async Task<HtmlResponse> Get()
		{
			string content = _templateRepository.Run(
				@"Templates\HelloWorld",
				new Model
					{
						Message = "Hello, world.",
						AntiCsrfHtml = await _antiCsrfHtmlGenerator.GenerateHiddenInputHtml()
					});

			return new HtmlResponse(content);
		}

		public class Model
		{
			public string Message
			{
				get;
				set;
			}

			public string AntiCsrfHtml
			{
				get;
				set;
			}
		}
	}
}
using Junior.Route.Routing.Responses.Text;
using Junior.Route.ViewEngines.Razor.Routing.TemplateRepositories;

namespace JuniorRouteWebApplication.Endpoints
{
	public class HelloWorld
	{
		private readonly ITemplateRepository _templateRepository;

		public HelloWorld(ITemplateRepository templateRepository)
		{
			_templateRepository = templateRepository;
		}

		public HtmlResponse Get()
		{
			string content = _templateRepository.Execute(@"Templates\HelloWorld", new Model { Message = "Hello, world." });

			return new HtmlResponse(content);
		}

		public class Model
		{
			public string Message
			{
				get;
				set;
			}
		}
	}
}
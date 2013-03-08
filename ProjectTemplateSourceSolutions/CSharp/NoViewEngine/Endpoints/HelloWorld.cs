using System;
using System.Threading.Tasks;

using Junior.Common;
using Junior.Route.Routing.AntiCsrf.HtmlGenerators;
using Junior.Route.Routing.Responses.Text;

namespace JuniorRouteWebApplication.Endpoints
{
	public class HelloWorld
	{
		private readonly IAntiCsrfHtmlGenerator _antiCsrfHtmlGenerator;

		public HelloWorld(IAntiCsrfHtmlGenerator antiCsrfHtmlGenerator)
		{
			antiCsrfHtmlGenerator.ThrowIfNull("antiCsrfHtmlGenerator");

			_antiCsrfHtmlGenerator = antiCsrfHtmlGenerator;
		}

		public async Task<HtmlResponse> Get()
		{
			return new HtmlResponse(
				String.Format(
					@"<html>
						<body style=""font-size: 3em;"">
							Hello, world.
							{0}
						</body>
					</html>",
					await _antiCsrfHtmlGenerator.GenerateHiddenInputHtml()));
		}
	}
}
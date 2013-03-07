using System.Threading.Tasks;

namespace Junior.Route.Routing.AntiCsrf.HtmlGenerators
{
	public interface IAntiCsrfHtmlGenerator
	{
		Task<string> GenerateHiddenInputHtml();
	}
}
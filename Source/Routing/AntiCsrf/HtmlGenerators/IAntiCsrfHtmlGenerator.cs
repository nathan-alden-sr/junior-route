using System.Threading.Tasks;
using System.Web;

namespace Junior.Route.Routing.AntiCsrf.HtmlGenerators
{
	public interface IAntiCsrfHtmlGenerator
	{
		Task<string> GenerateHiddenInputHtmlAsync(HttpResponseBase response);
	}
}
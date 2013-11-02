using System;
using System.Threading.Tasks;

namespace Junior.Route.Routing.AntiCsrf.Generators
{
	public interface IAntiCsrfGenerator
	{
		Task<AntiCsrfNonce> Generate(Guid? sessionId = null);
	}
}
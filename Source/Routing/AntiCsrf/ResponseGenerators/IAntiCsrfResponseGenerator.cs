using Junior.Route.Routing.AntiCsrf.Validators;

namespace Junior.Route.Routing.AntiCsrf.ResponseGenerators
{
	public interface IAntiCsrfResponseGenerator
	{
		ResponseResult GetResponse(ValidationResult result);
	}
}
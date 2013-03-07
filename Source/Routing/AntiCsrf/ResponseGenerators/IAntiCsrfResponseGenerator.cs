using Junior.Route.Routing.AntiCsrf.NonceValidators;

namespace Junior.Route.Routing.AntiCsrf.ResponseGenerators
{
	public interface IAntiCsrfResponseGenerator
	{
		ResponseResult GetResponse(ValidationResult result);
	}
}
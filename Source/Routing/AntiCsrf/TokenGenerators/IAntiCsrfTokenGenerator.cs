namespace Junior.Route.Routing.AntiCsrf.TokenGenerators
{
	public interface IAntiCsrfTokenGenerator
	{
		string Generate();
	}
}
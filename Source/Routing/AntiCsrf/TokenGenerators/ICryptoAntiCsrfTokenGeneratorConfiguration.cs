namespace Junior.Route.Routing.AntiCsrf.TokenGenerators
{
	public interface ICryptoAntiCsrfTokenGeneratorConfiguration
	{
		int LengthInBytes
		{
			get;
		}
	}
}
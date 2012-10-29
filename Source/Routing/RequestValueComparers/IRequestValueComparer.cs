namespace Junior.Route.Routing.RequestValueComparers
{
	public interface IRequestValueComparer
	{
		bool Matches(string value, string requestValue);
	}
}
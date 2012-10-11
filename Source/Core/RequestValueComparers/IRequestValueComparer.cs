namespace NathanAlden.JuniorRouting.Core.RequestValueComparers
{
	public interface IRequestValueComparer
	{
		bool Matches(string value, string requestValue);
	}
}
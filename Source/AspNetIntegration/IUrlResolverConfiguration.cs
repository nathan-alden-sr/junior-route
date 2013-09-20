using Junior.Route.Common;

namespace Junior.Route.AspNetIntegration
{
	public interface IUrlResolverConfiguration
	{
		string HostName
		{
			get;
		}

		ushort GetPort(Scheme scheme);
	}
}
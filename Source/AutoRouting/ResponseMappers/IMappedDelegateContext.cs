using System;

namespace Junior.Route.AutoRouting.ResponseMappers
{
	public interface IMappedDelegateContext : IDisposable
	{
		void Complete();
	}
}
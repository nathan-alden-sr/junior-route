using System.Threading.Tasks;

namespace Junior.Route.AutoRouting
{
	public static class TaskExtensions
	{
		// This method is called by ResponseMethodReturnTypeMapper via reflection
		public static async Task<TBase> Upcast<TDerived, TBase>(this Task<TDerived> task)
			where TDerived : TBase
		{
			return (TBase)await task;
		}
	}
}
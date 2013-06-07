using System;
using System.Reflection;
using System.Threading.Tasks;

using Junior.Common;

namespace Junior.Route.AutoRouting.IdMappers
{
	public class RandomIdMapper : IIdMapper
	{
		private readonly IGuidFactory _guidFactory;

		public RandomIdMapper(IGuidFactory guidFactory)
		{
			guidFactory.ThrowIfNull("guidFactory");

			_guidFactory = guidFactory;
		}

		public Task<IdResult> MapAsync(Type type, MethodInfo method)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");

			return IdResult.IdMapped(_guidFactory.Random()).AsCompletedTask();
		}
	}
}
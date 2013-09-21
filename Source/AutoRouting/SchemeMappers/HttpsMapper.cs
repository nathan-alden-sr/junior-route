using System;
using System.Reflection;
using System.Threading.Tasks;

using Junior.Common;
using Junior.Route.Common;

namespace Junior.Route.AutoRouting.SchemeMappers
{
	public class HttpsMapper : ISchemeMapper
	{
		public Task<SchemeResult> MapAsync(Type type, MethodInfo method)
		{
			return SchemeResult.SchemeMapped(Scheme.Https).AsCompletedTask();
		}
	}
}
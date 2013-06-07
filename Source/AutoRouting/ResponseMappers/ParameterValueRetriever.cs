using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.AutoRouting.ParameterMappers;

using ALinq;

namespace Junior.Route.AutoRouting.ResponseMappers
{
	public class ParameterValueRetriever
	{
		private readonly IParameterMapper[] _parameterMappers;

		public ParameterValueRetriever(IEnumerable<IParameterMapper> parameterMappers)
		{
			parameterMappers.ThrowIfNull("parameterMappers");

			_parameterMappers = parameterMappers.ToArray();

			if (!_parameterMappers.Any())
			{
				throw new ArgumentException("Must provide at least one parameter mapper.", "parameterMappers");
			}
		}

		public async Task<IEnumerable<object>> GetParameterValues(HttpContextBase context, Type type, MethodInfo method)
		{
			context.ThrowIfNull("context");
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");

			ParameterInfo[] parameterInfos = method.GetParameters();
			var parameterValues = new List<object>();

			foreach (ParameterInfo parameterInfo in parameterInfos)
			{
				Type parameterType = parameterInfo.ParameterType;
				string parameterName = parameterInfo.Name;
				Type currentParameterType = parameterType;

				do
				{
					bool mapped = false;

					IParameterMapper[] parameterMappers = await _parameterMappers
						.ToAsync()
						.Where(async arg => await arg.CanMapTypeAsync(context, parameterType))
						.ToArray();

					foreach (IParameterMapper parameterMapper in parameterMappers)
					{
						MapResult mapResult = await parameterMapper.MapAsync(context, type, method, parameterInfo);

						if (mapResult.ResultType == MapResultType.ValueNotMapped)
						{
							continue;
						}

						parameterValues.Add(mapResult.Value);
						mapped = true;
						break;
					}
					if (mapped)
					{
						break;
					}

					currentParameterType = currentParameterType.BaseType;
				} while (currentParameterType != null);

				if (currentParameterType == null)
				{
					throw new ApplicationException(
						String.Format(
							"No request parameter mapper was found for parameter '{0} {1}' of {2}.{3}.",
							parameterType.FullName,
							parameterName,
							type.FullName,
							method.Name));
				}
			}

			return parameterValues;
		}
	}
}
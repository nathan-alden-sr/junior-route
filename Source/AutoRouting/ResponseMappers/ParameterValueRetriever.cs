using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

using Junior.Common;
using Junior.Route.AutoRouting.ParameterMappers;

namespace Junior.Route.AutoRouting.ResponseMappers
{
	public class ParameterValueRetriever
	{
		private readonly IEnumerable<IParameterMapper> _parameterMappers;

		public ParameterValueRetriever(IEnumerable<IParameterMapper> parameterMappers)
		{
			parameterMappers.ThrowIfNull("parameterMappers");

			parameterMappers = parameterMappers.ToArray();

			if (!parameterMappers.Any())
			{
				throw new ArgumentException("Must provide at least 1 parameter mapper.", "parameterMappers");
			}

			_parameterMappers = parameterMappers;
		}

		public IEnumerable<object> GetParameterValues(HttpRequestBase request, Type type, MethodInfo method)
		{
			request.ThrowIfNull("request");
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

					foreach (IParameterMapper parameterMapper in _parameterMappers.Where(arg => arg.CanMapType(request, parameterType)))
					{
						MapResult mapResult = parameterMapper.Map(request, type, method, parameterInfo);

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
							"No request parameter mapper was found for parameter '{0} {1}' of '{2}.{3}'.",
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
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

using Junior.Common;

namespace Junior.Route.AutoRouting.ParameterMappers
{
	public class QueryStringToIConvertibleMapper : IParameterMapper
	{
		private readonly bool _caseSensitive;
		private readonly DataConversionErrorHandling _errorHandling;

		public QueryStringToIConvertibleMapper(bool caseSensitive = false, DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
		{
			_caseSensitive = caseSensitive;
			_errorHandling = errorHandling;
		}

		public bool CanMapType(HttpRequestBase request, Type parameterType)
		{
			request.ThrowIfNull("request");
			parameterType.ThrowIfNull("parameterType");

			return parameterType.ImplementsInterface<IConvertible>();
		}

		public MapResult Map(HttpRequestBase request, Type type, MethodInfo method, ParameterInfo parameter)
		{
			request.ThrowIfNull("request");
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			parameter.ThrowIfNull("parameter");

			Type parameterType = parameter.ParameterType;
			string parameterName = parameter.Name;
			string field = request.QueryString.AllKeys.LastOrDefault(arg => String.Equals(arg, parameterName, _caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase));

			if (field == null)
			{
				return MapResult.ValueNotMapped();
			}

			IConvertible value = request.QueryString[field];
			object convertedValue;

			try
			{
				convertedValue = value.ToType(parameterType, CultureInfo.InvariantCulture);
			}
			catch (Exception exception)
			{
				if (_errorHandling == DataConversionErrorHandling.ThrowException)
				{
					throw new ApplicationException(
						String.Format(
							"Value for query string field '{0}' could not be converted to parameter '{1} {2}' of {3}.{4}.",
							field,
							parameterType.FullName,
							parameterName,
							type.FullName,
							method.Name),
						exception);
				}
				convertedValue = parameterType.GetDefaultValue();
			}

			return MapResult.ValueMapped(convertedValue);
		}
	}
}
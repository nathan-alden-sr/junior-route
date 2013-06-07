using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;

namespace Junior.Route.AutoRouting.ParameterMappers
{
	public class FormToIConvertibleMapper : IParameterMapper
	{
		private readonly bool _caseSensitive;
		private readonly DataConversionErrorHandling _errorHandling;

		public FormToIConvertibleMapper(bool caseSensitive = false, DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
		{
			_caseSensitive = caseSensitive;
			_errorHandling = errorHandling;
		}

		public Task<bool> CanMapTypeAsync(HttpContextBase context, Type parameterType)
		{
			context.ThrowIfNull("context");
			parameterType.ThrowIfNull("parameterType");

			return parameterType.ImplementsInterface<IConvertible>().AsCompletedTask();
		}

		public Task<MapResult> MapAsync(HttpContextBase context, Type type, MethodInfo method, ParameterInfo parameter)
		{
			context.ThrowIfNull("context");
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			parameter.ThrowIfNull("parameter");

			Type parameterType = parameter.ParameterType;
			string parameterName = parameter.Name;
			string field = context.Request.Form.AllKeys.LastOrDefault(arg => String.Equals(arg, parameterName, _caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase));

			if (field == null)
			{
				return MapResult.ValueNotMapped().AsCompletedTask();
			}

			IConvertible value = context.Request.Form[field];
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
							"Value for form field '{0}' could not be converted to parameter '{1} {2}' of {3}.{4}.",
							field,
							parameterType.FullName,
							parameterName,
							type.FullName,
							method.Name),
						exception);
				}
				convertedValue = parameterType.GetDefaultValue();
			}

			return MapResult.ValueMapped(convertedValue).AsCompletedTask();
		}
	}
}
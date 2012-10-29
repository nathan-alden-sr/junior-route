using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

using Junior.Common;

namespace Junior.Route.AutoRouting.ParameterMappers.ModelPropertyMappers
{
	public class FormToIConvertibleMapper : IModelPropertyMapper
	{
		private readonly bool _caseSensitive;
		private readonly DataConversionErrorHandling _errorHandling;

		public FormToIConvertibleMapper(bool caseSensitive = false, DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
		{
			_caseSensitive = caseSensitive;
			_errorHandling = errorHandling;
		}

		public bool CanMapType(Type propertyType)
		{
			propertyType.ThrowIfNull("propertyType");

			return propertyType.ImplementsInterface<IConvertible>();
		}

		public MapResult Map(HttpRequestBase request, Type modelType, PropertyInfo property)
		{
			request.ThrowIfNull("request");
			modelType.ThrowIfNull("modelType");
			property.ThrowIfNull("property");

			Type propertyType = property.PropertyType;
			string propertyName = property.Name;
			string field = request.Form.AllKeys.LastOrDefault(arg => String.Equals(arg, propertyName, _caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase));

			if (field == null)
			{
				return MapResult.ValueNotMapped();
			}

			IConvertible value = request.Form[field];
			object convertedValue;

			try
			{
				convertedValue = value.ToType(propertyType, CultureInfo.InvariantCulture);
			}
			catch (Exception exception)
			{
				if (_errorHandling == DataConversionErrorHandling.ThrowException)
				{
					throw new ApplicationException(
						String.Format(
							"Value of form field '{0}' could not be converted to property '{1} {2}' of type '{3}'.",
							field,
							propertyType.FullName,
							propertyName,
							modelType.FullName),
						exception);
				}
				convertedValue = propertyType.GetDefaultValue();
			}

			return MapResult.ValueMapped(convertedValue);
		}
	}
}
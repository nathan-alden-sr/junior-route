using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;

namespace Junior.Route.AutoRouting.ParameterMappers.ModelPropertyMappers.Request
{
	public abstract class RequestMapper : IModelPropertyMapper
	{
		private readonly bool _caseSensitive;
		private readonly DataConversionErrorHandling _errorHandling;
		private readonly NameValueCollectionSource _source;

		protected RequestMapper(NameValueCollectionSource source, bool caseSensitive = false, DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
		{
			_source = source;
			_caseSensitive = caseSensitive;
			_errorHandling = errorHandling;
		}

		public abstract Task<bool> CanMapTypeAsync(HttpContextBase context, Type propertyType);

		public Task<MapResult> MapAsync(HttpContextBase context, Type modelType, PropertyInfo property)
		{
			context.ThrowIfNull("context");
			modelType.ThrowIfNull("modelType");
			property.ThrowIfNull("property");

			Type propertyType = property.PropertyType;
			string propertyName = property.Name;
			NameValueCollection nameValueCollection;

			switch (_source)
			{
				case NameValueCollectionSource.Form:
					nameValueCollection = context.Request.Form;
					break;
				case NameValueCollectionSource.QueryString:
					nameValueCollection = context.Request.QueryString;
					break;
				default:
					throw new InvalidOperationException(String.Format("Unexpected name-value collection source {0}.", _source));
			}
			string field = nameValueCollection.AllKeys.LastOrDefault(arg => String.Equals(arg, propertyName, _caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase));

			if (field == null)
			{
				return MapResult.ValueNotMapped().AsCompletedTask();
			}

			string value = nameValueCollection[field];

			try
			{
				return OnMapAsync(context, value, propertyType);
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

				return MapResult.ValueMapped(propertyType.GetDefaultValue()).AsCompletedTask();
			}
		}

		protected abstract Task<MapResult> OnMapAsync(HttpContextBase context, string value, Type propertyType);
	}
}
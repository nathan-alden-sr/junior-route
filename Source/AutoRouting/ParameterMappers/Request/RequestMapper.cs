using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;

namespace Junior.Route.AutoRouting.ParameterMappers.Request
{
	public abstract class RequestMapper : IParameterMapper
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

		public abstract Task<bool> CanMapTypeAsync(HttpContextBase context, Type parameterType);

		public Task<MapResult> MapAsync(HttpContextBase context, Type type, MethodInfo method, ParameterInfo parameter)
		{
			context.ThrowIfNull("context");
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			parameter.ThrowIfNull("parameter");

			Type parameterType = parameter.ParameterType;
			string parameterName = parameter.Name;
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
			string field = nameValueCollection.AllKeys.LastOrDefault(arg => String.Equals(arg, parameterName, _caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase));

			if (field == null)
			{
				return MapResult.ValueNotMapped().AsCompletedTask();
			}

			string value = nameValueCollection[field];

			try
			{
				return OnMapAsync(context, value, parameterType);
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

				return MapResult.ValueMapped(parameterType.GetDefaultValue()).AsCompletedTask();
			}
		}

		protected abstract Task<MapResult> OnMapAsync(HttpContextBase context, string value, Type parameterType);
	}
}
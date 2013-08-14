using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;

namespace Junior.Route.AutoRouting.ParameterMappers.Request
{
	public class ConvertibleMapper : RequestMapper
	{
		public ConvertibleMapper(NameValueCollectionSource source, bool caseSensitive = false, DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
			: base(source, caseSensitive, errorHandling)
		{
		}

		public override Task<bool> CanMapTypeAsync(HttpContextBase context, Type parameterType)
		{
			context.ThrowIfNull("context");
			parameterType.ThrowIfNull("parameterType");

			return parameterType.ImplementsInterface<IConvertible>().AsCompletedTask();
		}

		protected override Task<MapResult> OnMapAsync(HttpContextBase context, string value, Type parameterType)
		{
			context.ThrowIfNull("context");
			value.ThrowIfNull("value");
			parameterType.ThrowIfNull("parameterType");

			return MapResult.ValueMapped(((IConvertible)value).ToType(parameterType, CultureInfo.InvariantCulture)).AsCompletedTask();
		}
	}
}
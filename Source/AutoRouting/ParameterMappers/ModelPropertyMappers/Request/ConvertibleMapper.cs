using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;

namespace Junior.Route.AutoRouting.ParameterMappers.ModelPropertyMappers.Request
{
	public class ConvertibleMapper : RequestMapper
	{
		public ConvertibleMapper(NameValueCollectionSource source, bool caseSensitive = false, DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
			: base(source, caseSensitive, errorHandling)
		{
		}

		public override Task<bool> CanMapTypeAsync(HttpContextBase context, Type propertyType)
		{
			context.ThrowIfNull("context");
			propertyType.ThrowIfNull("propertyType");

			return propertyType.ImplementsInterface<IConvertible>().AsCompletedTask();
		}

		protected override Task<MapResult> OnMapAsync(HttpContextBase context, string value, Type propertyType)
		{
			context.ThrowIfNull("context");
			value.ThrowIfNull("value");
			propertyType.ThrowIfNull("parameterType");

			return MapResult.ValueMapped(((IConvertible)value).ToType(propertyType, CultureInfo.InvariantCulture)).AsCompletedTask();
		}
	}
}
using System;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;

namespace Junior.Route.AutoRouting.ParameterMappers.ModelPropertyMappers.Request
{
	public class GuidMapper : RequestMapper
	{
		private readonly string _format;

		public GuidMapper(NameValueCollectionSource source, bool caseSensitive = false, DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue, string format = null)
			: base(source, caseSensitive, errorHandling)
		{
			_format = format;
		}

		public override Task<bool> CanMapTypeAsync(HttpContextBase context, Type propertyType)
		{
			context.ThrowIfNull("context");
			propertyType.ThrowIfNull("propertyType");

			return (propertyType == typeof(Guid)).AsCompletedTask();
		}

		protected override Task<MapResult> OnMapAsync(HttpContextBase context, string value, Type propertyType)
		{
			context.ThrowIfNull("context");
			value.ThrowIfNull("value");
			propertyType.ThrowIfNull("propertyType");

			return MapResult.ValueMapped(_format != null ? Guid.ParseExact(value, _format) : Guid.Parse(value)).AsCompletedTask();
		}
	}
}
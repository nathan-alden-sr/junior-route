using System;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;

namespace Junior.Route.AutoRouting.ParameterMappers.Request
{
	public class GuidMapper : RequestMapper
	{
		private readonly string _format;

		public GuidMapper(NameValueCollectionSource source, bool caseSensitive = false, DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue, string format = null)
			: base(source, caseSensitive, errorHandling)
		{
			_format = format;
		}

		public override Task<bool> CanMapTypeAsync(HttpContextBase context, Type parameterType)
		{
			context.ThrowIfNull("context");
			parameterType.ThrowIfNull("parameterType");

			return (parameterType == typeof(Guid)).AsCompletedTask();
		}

		protected override Task<MapResult> OnMapAsync(HttpContextBase context, string value, Type parameterType)
		{
			context.ThrowIfNull("context");
			value.ThrowIfNull("value");
			parameterType.ThrowIfNull("parameterType");

			return MapResult.ValueMapped(_format != null ? Guid.ParseExact(value, _format) : Guid.Parse(value)).AsCompletedTask();
		}
	}
}
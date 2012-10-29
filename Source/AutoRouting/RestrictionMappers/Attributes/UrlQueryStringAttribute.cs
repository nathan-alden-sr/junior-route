using System;

using Junior.Common;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class UrlQueryStringAttribute : RestrictionAttribute
	{
		private readonly string _field;
		private readonly RequestValueComparer? _fieldComparer;
		private readonly string _value;
		private readonly RequestValueComparer? _valueComparer;

		public UrlQueryStringAttribute(string field, string value)
		{
			field.ThrowIfNull("field");
			value.ThrowIfNull("value");

			_field = field;
			_value = value;
		}

		public UrlQueryStringAttribute(string field, RequestValueComparer fieldComparer, string value, RequestValueComparer valueComparer)
		{
			field.ThrowIfNull("field");
			value.ThrowIfNull("value");

			_field = field;
			_fieldComparer = fieldComparer;
			_value = value;
			_valueComparer = valueComparer;
		}

		public override void Map(Routing.Route route)
		{
			route.ThrowIfNull("route");

			if (_fieldComparer != null && _valueComparer != null)
			{
				route.RestrictByUrlQueryString(_field, GetComparer(_fieldComparer.Value), _value, GetComparer(_valueComparer.Value));
			}
			else
			{
				route.RestrictByUrlQueryString(_field, _value);
			}
		}
	}
}
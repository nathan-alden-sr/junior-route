using System;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class HeaderRestrictionAttribute : RestrictionAttribute
	{
		private readonly string _field;
		private readonly bool _optional;
		private readonly string _value;
		private readonly RequestValueComparer? _valueComparer;

		public HeaderRestrictionAttribute(string field, string value, bool optional = false)
		{
			field.ThrowIfNull("field");
			value.ThrowIfNull("value");

			_field = field;
			_value = value;
			_optional = optional;
		}

		public HeaderRestrictionAttribute(string field, string value, RequestValueComparer valueComparer, bool optional = false)
		{
			field.ThrowIfNull("field");
			value.ThrowIfNull("value");

			_field = field;
			_value = value;
			_valueComparer = valueComparer;
			_optional = optional;
		}

		public override void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			if (_valueComparer != null)
			{
				route.RestrictByHeader(_field, _value, GetComparer(_valueComparer.Value), _optional);
			}
			else
			{
				route.RestrictByHeader(_field, _value, _optional);
			}
		}
	}
}
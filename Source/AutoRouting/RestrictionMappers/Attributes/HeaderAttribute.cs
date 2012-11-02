using System;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class HeaderAttribute : RestrictionAttribute
	{
		private readonly string _field;
		private readonly string _value;
		private readonly RequestValueComparer? _valueComparer;

		public HeaderAttribute(string field, string value)
		{
			field.ThrowIfNull("field");
			value.ThrowIfNull("value");

			_field = field;
			_value = value;
		}

		public HeaderAttribute(string field, string value, RequestValueComparer valueComparer)
		{
			field.ThrowIfNull("field");
			value.ThrowIfNull("value");

			_field = field;
			_value = value;
			_valueComparer = valueComparer;
		}

		public override void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			if (_valueComparer != null)
			{
				route.RestrictByHeader(_field, _value, GetComparer(_valueComparer.Value));
			}
			else
			{
				route.RestrictByHeader(_field, _value);
			}
		}
	}
}
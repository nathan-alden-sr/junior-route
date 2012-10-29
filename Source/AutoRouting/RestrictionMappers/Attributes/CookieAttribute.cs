using System;

using Junior.Common;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class CookieAttribute : RestrictionAttribute
	{
		private readonly string _name;
		private readonly RequestValueComparer? _nameComparer;
		private readonly string _value;
		private readonly RequestValueComparer? _valueComparer;

		public CookieAttribute(string name, string value)
		{
			name.ThrowIfNull("name");
			value.ThrowIfNull("value");

			_name = name;
			_value = value;
		}

		public CookieAttribute(string name, RequestValueComparer nameComparer, string value, RequestValueComparer valueComparer)
		{
			name.ThrowIfNull("name");
			value.ThrowIfNull("value");

			_name = name;
			_nameComparer = nameComparer;
			_value = value;
			_valueComparer = valueComparer;
		}

		public override void Map(Routing.Route route)
		{
			if (_nameComparer != null && _valueComparer != null)
			{
				route.RestrictByCookie(_name, GetComparer(_nameComparer.Value), _value, GetComparer(_valueComparer.Value));
			}
			else
			{
				route.RestrictByCookie(_name, _value);
			}
		}
	}
}
using Junior.Common;

namespace Junior.Route.Routing.Responses
{
	public sealed class Header
	{
		private readonly string _field;
		private readonly string _value;

		public Header(string field, string value)
		{
			field.ThrowIfNull("field");
			value.ThrowIfNull("value");

			_field = field;
			_value = value;
		}

		public string Field
		{
			get
			{
				return _field;
			}
		}

		public string Value
		{
			get
			{
				return _value;
			}
		}

		public Header Clone()
		{
			return new Header(_field, _value);
		}
	}
}
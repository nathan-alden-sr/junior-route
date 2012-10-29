using Junior.Common;

namespace Junior.Route.Routing.Responses
{
	public class CookieValue
	{
		private readonly string _name;
		private readonly string _value;

		public CookieValue(string name, string value)
		{
			name.ThrowIfNull("name");
			value.ThrowIfNull("value");

			_name = name;
			_value = value;
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public string Value
		{
			get
			{
				return _value;
			}
		}
	}
}
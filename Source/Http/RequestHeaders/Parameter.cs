namespace Junior.Route.Http.RequestHeaders
{
	public class Parameter
	{
		private readonly string _name;
		private readonly string _value;

		internal Parameter(string name, string value)
		{
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
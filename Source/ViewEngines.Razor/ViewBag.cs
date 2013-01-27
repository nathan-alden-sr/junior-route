using System.Collections.Generic;
using System.Dynamic;

namespace Junior.Route.ViewEngines.Razor
{
	public class ViewBag : DynamicObject
	{
		private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return _values.Keys;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			return _values.TryGetValue(binder.Name, out result);
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			_values[binder.Name] = value;

			return true;
		}
	}
}
using System;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection;

namespace Junior.Route.ViewEngines.Razor
{
	public class DynamicModel : DynamicObject
	{
		public object Model
		{
			get;
			set;
		}

		[DebuggerStepThrough]
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			if (Model == null)
			{
				throw new InvalidOperationException("No model was provided.");
			}

			var dynamicObject = Model as DynamicModel;

			if (dynamicObject != null)
			{
				return dynamicObject.TryGetMember(binder, out result);
			}

			Type modelType = Model.GetType();
			PropertyInfo property = modelType.GetProperty(binder.Name);

			if (property == null)
			{
				result = null;
				return false;
			}

			object value = property.GetValue(Model, null);

			if (value == null)
			{
				result = null;
				return true;
			}

			Type valueType = value.GetType();

			result = valueType.IsAnonymousType() ? new DynamicModel { Model = value } : value;

			return true;
		}
	}
}
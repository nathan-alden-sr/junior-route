using System.Dynamic;
using System.IO;
using System.Text;

using Junior.Common;

namespace Junior.Route.ViewEngines.Razor
{
	public class Template : ITemplate
	{
		private readonly StringBuilder _stringBuilder = new StringBuilder();

		public string Contents
		{
			get
			{
				return _stringBuilder.ToString();
			}
		}

		public virtual void Execute()
		{
		}

		public void Write(object value)
		{
			if (value != null)
			{
				_stringBuilder.Append(value);
			}
		}

		public void WriteLiteral(string value)
		{
			if (value != null)
			{
				_stringBuilder.Append(value);
			}
		}

		public static void WriteTo(TextWriter writer, object value)
		{
			writer.ThrowIfNull("writer");

			if (value != null)
			{
				writer.Write(value);
			}
		}

		public static void WriteLiteralTo(TextWriter writer, string value)
		{
			writer.ThrowIfNull("writer");

			if (value != null)
			{
				writer.Write(value);
			}
		}
	}

	public class Template<TModel> : Template, ITemplate<TModel>
	{
		private readonly bool _hasDynamicModel;
		private object _model;

		public Template()
		{
			_hasDynamicModel = GetType().IsDefined(typeof(DynamicModelAttribute), true);
		}

		public TModel Model
		{
			get
			{
				return (TModel)_model;
			}
			set
			{
				_model = _hasDynamicModel && !(value is DynamicObject) && !(value is ExpandoObject) ? (object)new DynamicModel { Model = value } : value;
			}
		}
	}
}
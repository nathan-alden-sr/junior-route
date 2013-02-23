using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;

using Junior.Common;
using Junior.Route.ViewEngines.Razor.TemplateRepositories;

namespace Junior.Route.ViewEngines.Razor
{
	public class Template : ITemplate
	{
		private TemplateRunContext _context;

		public string Layout
		{
			get;
			set;
		}

		public dynamic ViewBag
		{
			get
			{
				return _context.ViewBag;
			}
		}

		public ITemplateRepository TemplateRepository
		{
			get;
			set;
		}

		public string Run(TemplateRunContext context)
		{
			context.ThrowIfNull("context");

			_context = context;

			var stringBuilder = new StringBuilder();

			using (var stringWriter = new StringWriter(stringBuilder))
			{
				_context.CurrentWriter = stringWriter;
				Execute();
				_context.CurrentWriter = null;
			}

			if (Layout == null)
			{
				return stringBuilder.ToString();
			}

			ITemplate layoutTemplate = ResolveLayout(Layout);
			var templateWriter = new TemplateWriter(writer => writer.Write(stringBuilder.ToString()));

			context.PushBody(templateWriter);

			return layoutTemplate.Run(context);
		}

		public string Run()
		{
			return Run(new TemplateRunContext());
		}

		public virtual void Execute()
		{
		}

		public virtual void Write(object value)
		{
			WriteTo(_context.CurrentWriter, value);
		}

		public virtual void WriteLiteral(string value)
		{
			WriteLiteralTo(_context.CurrentWriter, value);
		}

		public virtual void Write(TemplateWriter writer)
		{
			if (writer != null)
			{
				writer.WriteTo(_context.CurrentWriter);
			}
		}

		public virtual void WriteAttribute(string name, PositionTagged<string> prefix, PositionTagged<string> suffix, params AttributeValue[] values)
		{
			WriteAttributeTo(_context.CurrentWriter, name, prefix, suffix, values);
		}

		public void DefineSection(string name, Action writeDelegate)
		{
			_context.DefineSection(name, writeDelegate);
		}

		public TemplateWriter RenderBody()
		{
			return _context.PopBody();
		}

		public TemplateWriter RenderSection(string name, bool isRequired = true)
		{
			name.ThrowIfNull("name");

			Action writeDelegate = _context.GetSectionWriteDelegate(name);

			if (writeDelegate == null)
			{
				if (isRequired)
				{
					throw new ArgumentException(String.Format("No section named '{0}' is defined.", name), "name");
				}

				return new TemplateWriter(writer => { });
			}

			return new TemplateWriter(writer => writeDelegate());
		}

		public TemplateWriter Include<TTemplate>(string relativePath, IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate
		{
			relativePath.ThrowIfNull("relativePath");
			namespaceImports.ThrowIfNull("namespaceImports");

			ThrowIfNoTemplateRepository();

			return new TemplateWriter(writer => writer.Write(TemplateRepository.Get<TTemplate>(relativePath, namespaceImports).Run()));
		}

		public TemplateWriter Include<TTemplate>(string relativePath)
			where TTemplate : ITemplate
		{
			relativePath.ThrowIfNull("relativePath");

			ThrowIfNoTemplateRepository();

			return new TemplateWriter(writer => writer.Write(TemplateRepository.Get<TTemplate>(relativePath).Run()));
		}

		public TemplateWriter Include<TTemplate, TModel>(string relativePath, TModel model, IEnumerable<string> namespaceImports)
			where TTemplate : ITemplate<TModel>
		{
			relativePath.ThrowIfNull("relativePath");
			namespaceImports.ThrowIfNull("namespaceImports");

			ThrowIfNoTemplateRepository();

			return new TemplateWriter(writer => writer.Write(TemplateRepository.Get<TTemplate, TModel>(relativePath, model, namespaceImports).Run()));
		}

		public TemplateWriter Include<TTemplate, TModel>(string relativePath, TModel model)
			where TTemplate : ITemplate<TModel>
		{
			relativePath.ThrowIfNull("relativePath");

			ThrowIfNoTemplateRepository();

			return new TemplateWriter(writer => writer.Write(TemplateRepository.Get<TTemplate, TModel>(relativePath, model).Run()));
		}

		public TemplateWriter Include(string relativePath, IEnumerable<string> namespaceImports)
		{
			relativePath.ThrowIfNull("relativePath");
			namespaceImports.ThrowIfNull("namespaceImports");

			ThrowIfNoTemplateRepository();

			return new TemplateWriter(writer => writer.Write(TemplateRepository.Get(relativePath, namespaceImports).Run()));
		}

		public TemplateWriter Include(string relativePath)
		{
			relativePath.ThrowIfNull("relativePath");

			ThrowIfNoTemplateRepository();

			return new TemplateWriter(writer => writer.Write(TemplateRepository.Get(relativePath).Run()));
		}

		public TemplateWriter Include<TModel>(string relativePath, TModel model, IEnumerable<string> namespaceImports)
		{
			relativePath.ThrowIfNull("relativePath");
			namespaceImports.ThrowIfNull("namespaceImports");

			ThrowIfNoTemplateRepository();

			return new TemplateWriter(writer => writer.Write(TemplateRepository.Get(relativePath, model, namespaceImports).Run()));
		}

		public TemplateWriter Include<TModel>(string relativePath, TModel model)
		{
			relativePath.ThrowIfNull("relativePath");

			ThrowIfNoTemplateRepository();

			return new TemplateWriter(writer => writer.Write(TemplateRepository.Get(relativePath, model).Run()));
		}

		public virtual bool IsSectionDefined(string name)
		{
			name.ThrowIfNull("name");

			return _context.GetSectionWriteDelegate(name) != null;
		}

		public virtual void WriteTo(TextWriter writer, object value)
		{
			writer.ThrowIfNull("writer");

			if (value != null)
			{
				writer.Write(value);
			}
		}

		public virtual void WriteTo(TextWriter textWriter, TemplateWriter templateWriter)
		{
			textWriter.ThrowIfNull("textWriter");

			if (templateWriter != null)
			{
				templateWriter.WriteTo(textWriter);
			}
		}

		public virtual void WriteAttributeTo(TextWriter writer, string name, PositionTagged<string> prefix, PositionTagged<string> suffix, params AttributeValue[] values)
		{
			if (values.Length == 0)
			{
				WriteLiteralTo(writer, prefix.Value);
				WriteLiteralTo(writer, suffix.Value);
				return;
			}

			bool first = true;
			bool wroteSomething = false;

			foreach (AttributeValue value in values)
			{
				PositionTagged<object> positionTagged = value.Value;

				if (positionTagged.Value == null)
				{
					continue;
				}

				string stringValue = positionTagged.Value.Equals(true) ? name : (positionTagged.Value as string ?? positionTagged.Value.ToString());

				if (first)
				{
					WriteLiteralTo(writer, prefix.Value);
					first = false;
				}
				else
				{
					WriteLiteralTo(writer, value.Prefix.Value);
				}
				if (value.Literal)
				{
					WriteLiteralTo(writer, stringValue);
				}
				else
				{
					WriteTo(writer, stringValue);
				}
				wroteSomething = true;
			}

			if (wroteSomething)
			{
				WriteLiteralTo(writer, suffix.Value);
			}
		}

		public virtual void WriteLiteralTo(TextWriter writer, string literal)
		{
			writer.ThrowIfNull("writer");

			if (literal != null)
			{
				writer.Write(literal);
			}
		}

		protected virtual ITemplate ResolveLayout(string relativePath)
		{
			relativePath.ThrowIfNull("relativePath");

			ThrowIfNoTemplateRepository();

			return TemplateRepository.Get(relativePath);
		}

		private void ThrowIfNoTemplateRepository()
		{
			if (TemplateRepository == null)
			{
				throw new InvalidOperationException("A template repository must be provided.");
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
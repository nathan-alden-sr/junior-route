using System;
using System.Collections.Generic;
using System.IO;

using Junior.Common;

namespace Junior.Route.ViewEngines.Razor
{
	public class TemplateRunContext
	{
		private readonly Stack<TemplateWriter> _bodyWriters = new Stack<TemplateWriter>();
		private readonly Dictionary<string, Action> _sections = new Dictionary<string, Action>();
		private readonly ViewBag _viewBag;

		public TemplateRunContext(ViewBag viewBag = null)
		{
			_viewBag = viewBag ?? new ViewBag();
		}

		internal TextWriter CurrentWriter
		{
			get;
			set;
		}

		public dynamic ViewBag
		{
			get
			{
				return _viewBag;
			}
		}

		public void DefineSection(string name, Action @writeDelegate)
		{
			name.ThrowIfNull("name");

			if (_sections.ContainsKey(name))
			{
				throw new ArgumentException(String.Format("A section named '{0}' has already been defined.", name), "name");
			}

			_sections.Add(name, writeDelegate);
		}

		public Action GetSectionWriteDelegate(string name)
		{
			name.ThrowIfNull("name");

			Action writeDelegate;

			return _sections.TryGetValue(name, out writeDelegate) ? writeDelegate : null;
		}

		internal void PushBody(TemplateWriter writer)
		{
			writer.ThrowIfNull("writer");

			_bodyWriters.Push(writer);
		}

		internal TemplateWriter PopBody()
		{
			return _bodyWriters.Pop();
		}
	}
}
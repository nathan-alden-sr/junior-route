using System;
using System.IO;

using Junior.Common;

namespace Junior.Route.ViewEngines.Razor
{
	public class TemplateWriter
	{
		private readonly Action<TextWriter> _writerDelegate;

		public TemplateWriter(Action<TextWriter> writerDelegate)
		{
			writerDelegate.ThrowIfNull("writerDelegate");

			_writerDelegate = writerDelegate;
		}

		public void WriteTo(TextWriter writer)
		{
			writer.ThrowIfNull("writer");

			_writerDelegate(writer);
		}

		public override string ToString()
		{
			using (var writer = new StringWriter())
			{
				_writerDelegate(writer);

				return writer.ToString();
			}
		}
	}
}
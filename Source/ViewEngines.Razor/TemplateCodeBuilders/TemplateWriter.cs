using System;
using System.IO;

using Junior.Common;

namespace Junior.Route.ViewEngines.Razor.TemplateCodeBuilders
{
	public class TemplateWriter
	{
		private readonly Action<TextWriter> writerDelegate;

		public TemplateWriter(Action<TextWriter> writer)
		{
			writer.ThrowIfNull("writer");

			writerDelegate = writer;
		}

		public override string ToString()
		{
			using (var writer = new StringWriter())
			{
				writerDelegate(writer);

				return writer.ToString();
			}
		}
	}
}
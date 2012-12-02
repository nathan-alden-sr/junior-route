using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Junior.Route.ViewEngines.Razor.TemplateCompilers
{
	public class TemplateCompilationException : ApplicationException
	{
		private readonly ReadOnlyCollection<CompilerError> _errors;

		internal TemplateCompilationException(CompilerErrorCollection errorCollection)
			: base(GetMessage(errorCollection))
		{
			_errors = new ReadOnlyCollection<CompilerError>(errorCollection.Cast<CompilerError>().ToArray());
		}

		public ReadOnlyCollection<CompilerError> Errors
		{
			get
			{
				return _errors;
			}
		}

		private static string GetMessage(CompilerErrorCollection errorCollection)
		{
			var builder = new StringBuilder();

			builder.AppendFormat("{0} error{1} occurred during template compilation. Inspect the Errors property for detailed information.{2}", errorCollection.Count, errorCollection.Count == 1 ? "" : "s", Environment.NewLine);
			for (int i = 0; i < errorCollection.Count; i++)
			{
				CompilerError error = errorCollection[i];

				builder.AppendFormat("{0}: {1}{2}", error.ErrorNumber, error.ErrorText, Environment.NewLine);
			}

			return builder.ToString();
		}
	}
}
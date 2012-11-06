using System;
using System.Collections.Generic;
using System.Linq;

using Junior.Common;

namespace Junior.Route.Diagnostics.Web
{
	public abstract class DiagnosticsView : View
	{
		private readonly Dictionary<string, HashSet<DiagnosticViewLink>> _diagnosticViewLinksByHeading = new Dictionary<string, HashSet<DiagnosticViewLink>>(StringComparer.OrdinalIgnoreCase);

		public override string Title
		{
			get
			{
				return "Diagnostics - JuniorRoute";
			}
		}

		public IEnumerable<string> DiagnosticViewHeadings
		{
			get
			{
				return _diagnosticViewLinksByHeading.Keys.OrderBy(arg => arg);
			}
		}

		public void AddDiagnosticViewLinks(IEnumerable<DiagnosticViewLink> links)
		{
			links.ThrowIfNull("links");

			links = links.ToArray();

			foreach (DiagnosticViewLink link in links)
			{
				HashSet<DiagnosticViewLink> linkList;

				if (!_diagnosticViewLinksByHeading.TryGetValue(link.Heading, out linkList))
				{
					linkList = new HashSet<DiagnosticViewLink>();
					_diagnosticViewLinksByHeading.Add(link.Heading, linkList);
				}

				linkList.Add(link);
			}
		}

		public void AddDiagnosticViewLinks(params DiagnosticViewLink[] links)
		{
			AddDiagnosticViewLinks((IEnumerable<DiagnosticViewLink>)links);
		}

		public IEnumerable<DiagnosticViewLink> GetDiagnosticViewLinks(string heading)
		{
			heading.ThrowIfNull("heading");

			return _diagnosticViewLinksByHeading[heading];
		}
	}
}
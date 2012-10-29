using System;
using System.Collections.Generic;
using System.Linq;

using Junior.Common;
using Junior.Route.Common;

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

		public void AddDiagnosticViewLinks(string heading, IEnumerable<DiagnosticViewLink> links)
		{
			heading.ThrowIfNull("heading");
			links.ThrowIfNull("links");

			HashSet<DiagnosticViewLink> linkList;

			if (!_diagnosticViewLinksByHeading.TryGetValue(heading, out linkList))
			{
				linkList = new HashSet<DiagnosticViewLink>();
				_diagnosticViewLinksByHeading.Add(heading, linkList);
			}

			linkList.AddRange(links);
		}

		public void AddDiagnosticViewLinks(string heading, params DiagnosticViewLink[] links)
		{
			heading.ThrowIfNull("heading");
			links.ThrowIfNull("links");

			AddDiagnosticViewLinks(heading, (IEnumerable<DiagnosticViewLink>)links);
		}

		public IEnumerable<DiagnosticViewLink> GetDiagnosticViewLinks(string heading)
		{
			heading.ThrowIfNull("heading");

			return _diagnosticViewLinksByHeading[heading];
		}
	}
}
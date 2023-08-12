using System.Text;
using System.Text.RegularExpressions;
using DataAccess.Models;
using HtmlAgilityPack;
using Scraper.Common;

namespace Scraper.Richmond;

internal class Minutes : DocumentBase
{
	private readonly HtmlDocument _page;
	
	public Minutes(Uri uri, string meetingsDocumentsType) : base(uri) {
		_page = new HtmlWeb().Load(uri);
		
		MeetingsDocumentsType = meetingsDocumentsType;
	}

	protected override void Parse() {
		var sb = new StringBuilder();

		// Iterate through all inner text found in the HTML and append to string builder
		foreach (var node in _page.DocumentNode.SelectNodes("//div[@class='content']//text()")) {
			// Some rudimentary filtering of text
			if (string.IsNullOrWhiteSpace(node.InnerText) || node.InnerText == "&nbsp;") continue;

			var cleanedText = Regex.Replace(node.InnerText, @"&nbsp;", "");
			sb.Append($"{cleanedText}\n");
		}

		var textContent = sb.ToString();

		var fingerprint = GenerateFingerprint(textContent);

		Model = new DocumentModel() {
			Url = _uri.ToString(),
			Fingerprint = fingerprint,
			TextContent = textContent
		};
	}

	public IEnumerable<DocumentBase> GetReferences() {
		var links = _page.DocumentNode.SelectNodes("//div[@class='content']//a");
		if (links is null) yield break;

		// Iterate through all links found in the HTML and append to string builder
		foreach (var link in links) {
			// Skip the link if its inner text matches
			if (
				// Every minutes has link on top that links to a PDF printout of itself. Skip this.
				string.Equals(link.InnerText, "Printer-Friendly Minutes", StringComparison.OrdinalIgnoreCase)
			) continue;

			// Deal with relative URLs
			var url = link.Attributes["href"].Value;
			Uri referenceUri;
			if (!string.IsNullOrEmpty(url) && url[0] == '/') {
				referenceUri = new Uri(Website.BaseUri, url);
			} else {
				referenceUri = new Uri(url);
			}

			url = referenceUri.ToString();

			if (Regex.IsMatch(url, @"pdf$")) {
				yield return new ReferencePdf(referenceUri);
			} else if (Regex.IsMatch(url, @"htm$")) {
				yield return new Minutes(referenceUri, "previous minutes");
			} else {
				throw new Exception("Can't tell what the file extension is");
			}
		}
	}
}
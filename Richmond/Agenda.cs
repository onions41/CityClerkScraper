using System.Text;
using System.Text.RegularExpressions;
using DataAccess.Models;
using HtmlAgilityPack;
using Scraper.Common;

namespace Scraper.Richmond;

internal class Agenda : DocumentBase
{
	public Agenda(Uri uri) : base(uri) {
		MeetingsDocumentsType = "agenda";
	}

	protected override void Parse() {
		var page = new HtmlWeb().Load(Uri);

		StringBuilder sb = new();
		// Iterate through all inner text found in the HTML and append to string builder
		foreach (var node in page.DocumentNode.SelectNodes("//div[@class='content']//text()")) {
			// Some rudimentary filtering of text
			if (string.IsNullOrWhiteSpace(node.InnerText) || node.InnerText == "&nbsp;") continue;
			var cleanedText = Regex.Replace(node.InnerText, @"&nbsp;", "");
			sb.Append($"{cleanedText}\n");
		}

		var textContent = sb.ToString();
		var fingerprint = GenerateFingerprint(textContent);

		Model = new DocumentModel() {
			Url = Uri.ToString(),
			Fingerprint = fingerprint,
			TextContent = textContent,
		};
	}

	protected override byte[] GenerateFingerprint<T>(T content) {
		if (content is not string textContent) throw new Exception("Impossible. TextContent is not of type string.");
		
		var fingerprint = new byte[100];
		var contentBytes = Encoding.ASCII.GetBytes(textContent);
		
		if (contentBytes.Length > 100) {
			var middleIndex = contentBytes.Length / 2;
			var startIndex = middleIndex - 50;
			Array.Copy(
				contentBytes,
				startIndex,
				fingerprint,
				0,
				100
			);
		} else {
			fingerprint = contentBytes;
		}
		
		return fingerprint;
	}
}
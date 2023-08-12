using System.Text.RegularExpressions;
using DataAccess.Models;
using Scraper.Common;
using HtmlAgilityPack;

namespace Scraper.Vancouver;

internal class Meeting : MeetingBase
{
	private readonly Uri _uri;
	private readonly HtmlDocument _page;

	public Meeting(Uri uri) {
		_uri = uri;
		_page = new HtmlWeb().Load(uri);

		var url = uri.ToString();

		// Determine meeting type
		string? meetingType = null;
		foreach (var pair in Website.MeetingTypeAbbr) {
			if (Regex.IsMatch(url, $"/{pair.Value}")) meetingType = pair.Key;
		}

		if (meetingType is null) throw new Exception("**** Meeting type could not be resolved ****");

		// Determine meeting date
		var dateStr = Regex.Match(url, @"(\d{8})").Groups[1].Value;
		var meetingDate = new DateTime(
			int.Parse(dateStr.Substring(0, 4)),
			int.Parse(dateStr.Substring(4, 2)),
			int.Parse(dateStr.Substring(6, 2))
		);

		Model = new MeetingModel() {
			MunicipalityName = Website.MunicipalityName,
			Type = meetingType,
			Date = meetingDate,
			Url = url
		};
	}

	public override IEnumerable<DocumentBase> GetDocuments() {
		var titleDiv = _page.DocumentNode.SelectSingleNode("//div[@id='pleasenote']");

		if (titleDiv is not null) {
			// Return minutes found in the titleDiv
			foreach (var link in titleDiv.SelectNodes(".//li/a")) {
				if (Regex.IsMatch(link.InnerText, "minutes", RegexOptions.IgnoreCase)) {
					// return minutes
					Uri minutesUri;
					try {
						minutesUri = new Uri(link.Attributes["href"].Value);
					} catch (UriFormatException) {
						minutesUri = new Uri(_uri, link.Attributes["href"].Value);
					}

					yield return new MinutesPdf(minutesUri);
				}
			}
		}

		// .main-content is nested inside .content. Sometimes, .main-content doesn't exist (for some in camera meetings)
		var contentDiv = _page.DocumentNode.SelectSingleNode("//div[@class='main-content']")
		                 ?? _page.DocumentNode.SelectSingleNode("//div[@class='content']");

		if (contentDiv is not null) {
			foreach (var link in contentDiv.SelectNodes(".//a[contains(@href, '.pdf')]")) {
				// The link at the bottom of the page is a PDF version of the meeting page. Ignore it
				if (link.ParentNode.Attributes["class"]?.Value == "agendaPDF") continue;
				
				Uri uri;
				try {
					uri = new Uri(link.Attributes["href"].Value);
				} catch (UriFormatException) {
					uri = new Uri(_uri, link.Attributes["href"].Value);
				}
					
				if (link.InnerHtml == "Decision") {
					yield return new MinutesPdf(uri);
				} else {
					yield return new ReferencePdf(uri);
				}
			}
		}
	}

	public override IEnumerable<VideoBase> GetVideos() {
		var titleDiv = _page.DocumentNode.SelectSingleNode("//div[@id='pleasenote']");

		// titleDiv is null for in camera meeting page
		if (titleDiv is null) yield break;

		foreach (var item in titleDiv.SelectNodes(".//li/a")) {
			if (Regex.IsMatch(item.InnerText, "video", RegexOptions.IgnoreCase)) {
				// return minutes
				var videoUri = new Uri(item.Attributes["href"].Value);
				yield return new Video(videoUri);
			}
		}
	}
}
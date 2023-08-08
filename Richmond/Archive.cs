using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Scraper.Richmond;

// Represents the webpage you arrive at by going to citycouncil.richmond.ca/agendas/archives
// then clicking Agendas & Minutes Archives from the left side panel,
// then clicking on a type of meeting.
// This page shows links to each year's archive.
internal class Archive
{
	private readonly Uri _uri;
	private readonly string _meetingType;
	private readonly HtmlDocument _page;

	public Archive(
		Uri uri,
		string meetingType
	) {
		_uri = uri;
		_meetingType = meetingType;
		_page = new HtmlWeb().Load(_uri);
	}

	// Yields Uris of archive pages for each year
	public IEnumerable<ArchiveYear> GetArchiveYears(DateTime startDate, DateTime endDate) {
		// Selects all <a> tags that link to each year's archive page
		var archiveLinks = _page.DocumentNode.SelectNodes(
			"//div[@class='links-block links-block--childlinks']/ul/li/a"
		) ?? throw new Exception("No links could be retrieved from this meeting's archive page");

		// Loop from start year to end year, inclusive.
		for (var year = startDate.Year; year <= endDate.Year; year++) {
			// Filters retrieved links for the specific year
			var results =
				archiveLinks.Where<HtmlNode>(link => Regex.Match(link.InnerHtml, $"{year}").Success);
			// Throws if more than one link is found containing that year
			if (results.Count<HtmlNode>() > 1) {
				throw new Exception($"More than one archive link found for year {year}");
			}

			// Throws if no archive link is found
			var link = results.First();

			// Grab the href of the retrieved <a> element
			var href = link.Attributes["href"].Value;
			// href is a relative URL
			if (!Regex.Match(href, "^http").Success) {
				// Append it to baseUrl to make it absolute
				href = $"{_uri}{href}";
			}

			yield return new ArchiveYear(new Uri(href), _meetingType);
		}
	}
}
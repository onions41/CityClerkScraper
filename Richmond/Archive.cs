using System.Text.RegularExpressions;

using HtmlAgilityPack;

namespace Scraper.Richmond;

// Represents the webpage you arrive at by going to citycouncil.richmond.ca/agendas/archives
// then clicking Agendas & Minutes Archives from the left side panel,
// then clicking on a type of meeting.
// This page shows links to each year's archive.
class Archive
{
  private readonly Uri _archiveUri;

  public Archive(
    string meetingTypeKeyword,
    Uri baseUri)
  {
    // Path to the archive page for a specific meeting type
    _archiveUri = new Uri(baseUri, $"/agendas/archives/{meetingTypeKeyword}.htm");
  }

  // Yields Uris of archive pages for each year
  public IEnumerable<Uri> GetArchiveYearUris(
    DateTime startDate,
    DateTime endDate)
  {
    // HtmlAgilityPack parcer
    HtmlDocument meetingPage = new HtmlWeb().Load(_archiveUri);

    // Selects all <a> elements that link to each year's archive page
    HtmlNodeCollection archiveLinks = meetingPage.DocumentNode.SelectNodes(
      "//div[@class='links-block links-block--childlinks']/ul/li/a"
    ) ?? throw new ArgumentNullException("No links could be retrieved from this meeting's archive page");

    // Loop from start year to end year, inclusive.
    for (int year = startDate.Year; year <= endDate.Year; year++)
    {
      // Filters retrieved links for the specific year
      IEnumerable<HtmlNode> results = archiveLinks.Where<HtmlNode>(link => Regex.Match(link.InnerHtml, $"{year}").Success);
      // Throws if more than one link is found containing that year
      if (results.Count<HtmlNode>() > 1) { throw new Exception($"More than one archive link found for year {year}"); }
      // Throws if no archive link is found
      HtmlNode link = results.First();

      // Grab the href of the retrieved <a> element
      string href = link.Attributes["href"].Value;
      // href is a reletive URL
      if (!Regex.Match(href, "^http").Success)
      {
        // Append it to baseUrl to make it absolute
        href = $"{_archiveUri}{href}";
      }

      yield return new Uri(href);
    }
  }
}


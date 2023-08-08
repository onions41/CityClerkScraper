using System.Globalization;
using HtmlAgilityPack;

namespace Scraper.Richmond;

// Represents the webpage you arrive at by going to citycouncil.richmond.ca/agendas/archives
// then clicking Agendas & Minutes Archives from the left side panel,
// then clicking on a type of meeting,
// then clicking on one of the links representing a year's Agendas & Minutes.
// This page is a portal containing links to resources, like meeting minutes, meeting recorded videos, etc.
// Richmond website archives are organized like this. 
// Archives -> Meeting type -> Year -> Resources 
internal class ArchiveYear
{
	private readonly string _meetingType;
	private readonly HtmlDocument _page;

	public ArchiveYear(Uri uri, string meetingType) {
		_meetingType = meetingType;
		_page = new HtmlWeb().Load(uri) ??
		        throw new ArgumentException("_page", $"Could not parse the page at {uri.ToString()}");
		Uri = uri;
	}

	public Uri Uri { get; private init; }

	public IEnumerable<Meeting> GetMeetings(DateTime startDate, DateTime endDate) {
		// <tr> elements that contain each meeting's information and resource links
		var meetingRows = _page.DocumentNode.SelectNodes(
			"//table[@id='ctl00_ctl00_main_main_GridViewPrevious']/tr"
		) ?? throw new Exception("Could not find the <tr> elements containing each meeting's information");

		foreach (var meetingRow in meetingRows) {
			// The first <span> contains the date of the meeting
			var node = meetingRow.SelectSingleNode(".//span");

			if (node is null) continue;

			// Tries to parse the date of the meeting
			var success = DateTime.TryParseExact(
				node.InnerHtml,
				"MMMM d, yyyy",
				CultureInfo.InvariantCulture,
				DateTimeStyles.None,
				out var date
			);
			// Could not parse the date
			if (success is false) {
				throw new Exception("Could not parse the date of the meeting");
			}

			// If the date of the meeting falls outside of the desired date range
			if (date < startDate || endDate < date) continue;

			for (var i = 1; i <= 2; i++) {
				// ChildNode[1] is the <td> that contains the links to the meeting records of the current type
				// ChildNode[2] is the <td> that contains the links to the "in camera" meeting which occured on the same day
				var resourceLinks = meetingRow.ChildNodes[i].SelectNodes(".//a");

				// Don't return a meeting if there are no <a> tags associated with it
				if (resourceLinks is null) {
					continue;
				}

				// Loads link href values into a list
				List<Uri> resourceUris = new();
				foreach (var link in resourceLinks) {
					try {
						// Deal with relative URLs
						var url = link.Attributes["href"].Value;
						Uri resourceUri;
						if (!string.IsNullOrEmpty(url) && url[0] == '/') {
							resourceUri = new Uri(RichmondWebsite.BaseUri, url);
						} else {
							resourceUri = new Uri(url);
						}

						resourceUris.Add(resourceUri);
					}
					catch (Exception) {
						// Error if Uri cannot be created because the href value is invalid, just skip
					}
				}

				// Even if <a> tags were found, it's possible that all of them were invalid
				// Don't return a meeting if so.
				if (resourceUris.Count == 0) {
					continue;
				}

				switch (i) {
					case 1:
						yield return new Meeting(date, _meetingType, resourceUris);
						break;
					case 2:
						yield return new Meeting(date, "in camera meeting", resourceUris);
						break;
				}
			}
		}
	}
}
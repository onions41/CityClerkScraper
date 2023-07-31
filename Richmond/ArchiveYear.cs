using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using DataAccess.Data;

using HtmlAgilityPack;

namespace Scraper.Richmond;

// Represents the webpage you arrive at by going to citycouncil.richmond.ca/agendas/archives
// then clicking Agendas & Minutes Archives from the left side panel,
// then clicking on a type of meeting,
// then clicking on one of the links representing a year's Agendas & Minutes.
// This page is a portal containing links to resources, like meeting minutes, meeting recorded videos, etc.
// Richmond website archives are organized like this. 
// Archives -> Meeting type -> Year -> Resources 
internal class ArchiveYear {
  private readonly HtmlDocument _page;
  private readonly string _meetingType;
  private readonly IMeetingData _meetingData;

  public ArchiveYear(Uri archiveYearUri, string meetingType, IMeetingData meetingData) {
    // HtmlAgilityPack parser
    _page = new HtmlWeb().Load(archiveYearUri)
      ?? throw new ArgumentException("_page", $"Could not parse the page at {archiveYearUri.ToString()}");

    _meetingType = meetingType;
    _meetingData = meetingData;
  }

  public IEnumerable<MeetingElement> GetMeetingElements() {
    // <tr> elements that contain each meeting's information and resource links
    HtmlNodeCollection meetingRows = _page.DocumentNode.SelectNodes(
      "//table[@id='ctl00_ctl00_main_main_GridViewPrevious']/tr"
    ) ?? throw new ArgumentNullException("meetingRows", "Could not find the <tr> elements containing each meeting's information");

    foreach (var meetingRow in meetingRows) {
      // The first <span> contains the date of the meeting
      HtmlNode node = meetingRow.SelectSingleNode(".//span");
      if (node is not null) {
        DateTime date;
        // Tries to parse the date of the meeting
        bool success = DateTime.TryParseExact(
          node.InnerHtml,
          "MMMM d, yyyy",
          CultureInfo.InvariantCulture,
          DateTimeStyles.None,
          out date
        );
        // Could not parse the date
        if (success is false) {
          throw new Exception("Could not parse the date of the meeting");
        }

        for (int i = 1; i <= 2; i++) {
          // ChildNode[1] is the <td> that contains the links to the meeting records of the current type
          // ChildNode[2] is the <td> that contains the links to the "in camera" meeting which occured on the same day
          HtmlNodeCollection resourceLinks = meetingRow.ChildNodes[1].SelectNodes(".//a");

          // Yields the MeetingElement if there were links to resources connected to the meeting was/were found.
          // If resourceLinks is null, it means there was no meeting
          if (resourceLinks is not null) {
            // Loads link href values into a list
            List<Uri> resourceUris = new();
            foreach (var link in resourceLinks) {
              try {
                resourceUris.Add(new Uri(link.Attributes["href"].Value));
              } catch (Exception) {
                // Error if Uri cannot be created because the href value is invalid, just skip
              }
            }

            if (i == 1) {
              yield return new MeetingElement(date, _meetingType, resourceUris);
            } else if (i == 2) {
              yield return new MeetingElement(date, "in camera", resourceUris);
            }
          }
        }
      }
    }
  }
}

  // Yields URLs for all resources found on the page
  // TODO: Should take dates in parameters and yield only the resources that fit the range
  // public IEnumerable<Uri> GetResourceUris()
  // {
  //   // HtmlAgilityPack parser
  //   HtmlDocument yearPage = new HtmlWeb().Load(_archiveYearUri);

  //   HtmlNodeCollection resourceLinks = yearPage.DocumentNode.SelectNodes(
  //     "//table[@id='ctl00_ctl00_main_main_GridViewPrevious']//a"
  //   ) ?? throw new ArgumentNullException("No links could be retrieved from this year's archive page");

  //   foreach (var link in resourceLinks)
  //   {
  //     Uri uri;
  //     try
  //     {
  //       uri = new(link.Attributes["href"].Value);
  //     }
  //     catch (Exception)
  //     {
  //       // Just skip broken links
  //       continue;
  //     }
  //     yield return uri;
  //   }
  // }




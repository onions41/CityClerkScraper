using System.Text.RegularExpressions;
using HtmlAgilityPack;
using DataAccess.Data;

namespace Scraper.Richmond;

internal class RichmondScraper : IRichmondScraper {
  private readonly ILogger<RichmondScraper> _logger;
  private readonly IMeetingData _meetingData;
  private readonly Uri _baseUri = new("https://citycouncil.richmond.ca");
  private readonly Dictionary<string, string> _meetingTypes = new Dictionary<string, string>() {
    {"council meeting", "council"},
    {"community safety committee", "safety"},
    {"development permit panel", "dpp"},
    {"finance committee", "finance"},
    {"general purpose committee", "gp"},
    {"parks, recreation, and cultural services committee", "prcs"},
    {"planning committee", "planning"},
    {"public hearing", "hearings"},
    {"public works and transportation committee", "pwt"}
  };

  public RichmondScraper(ILogger<RichmondScraper> logger, IMeetingData meetingData) {
    _logger = logger;
    _meetingData = meetingData;
  }

  public async Task Scrape(DateTime startDate, DateTime endDate) {
    // IEnumerable<MeetingElement> meetings = new ArchiveYear()


    // List<Uri> uris = new() {
    //   new Uri("https://www.google.com"),
    //   new Uri("https://www.yandex.com")
    // };
    // MeetingElement meeting = new(new DateTime(2022, 3, 4), "council meeting", uris);

    // _logger.LogInformation(meeting.ResourceUris[0].ToString());
    // _logger.LogInformation(meeting.ResourceUris[1].ToString());

    // await meeting.Save(_meetingData);
    // Console.WriteLine("######");



    // foreach (var pair in _meetingTypes)
    // {
    //   _logger.LogInformation(pair.Key);
    //   _logger.LogInformation(pair.Value);

    //   Archive archive = new(pair.Value, _baseUri);
    //   IEnumerable<Uri> archiveYearUris = archive.GetArchiveYearUris(startDate, endDate);

    //   foreach (Uri archiveYearUri in archiveYearUris)
    //   {
    //     _logger.LogInformation(archiveYearUri.ToString());

    //     ArchiveYear archiveYear = new(archiveYearUri, pair.Key);

    //     // IEnumerable<Uri> resourceUris = archiveYear.GetResourceUris();
    //     // Thread.Sleep(1000);
    //     // foreach (Uri resourceUri in resourceUris)
    //     // {
    //     //   _logger.LogInformation(resourceUri.ToString());
    //     // }
    //   }
    // }
  }
}
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using DataAccess.Data;

namespace Scraper.Richmond;

internal class RichmondWebsite : IRichmondSite
{
	private readonly ILogger<RichmondWebsite> _logger;
	private readonly IMeetingData _meetingData;
	public static readonly Uri BaseUri = new Uri("https://citycouncil.richmond.ca");

	private readonly Dictionary<string, string> _meetingTypes = new Dictionary<string, string>() {
		{ "council meeting", "council" },
		{ "community safety committee", "safety" },
		{ "development permit panel", "dpp" },
		{ "finance committee", "finance" },
		{ "general purpose committee", "gp" },
		{ "parks, recreation, and cultural services committee", "prcs" },
		{ "planning committee", "planning" },
		{ "public hearing", "hearings" },
		{ "public works and transportation committee", "pwt" }
	};

	public RichmondWebsite(ILogger<RichmondWebsite> logger, IMeetingData meetingData) {
		_logger = logger;
		_meetingData = meetingData;
	}

	public Task Scrape(DateTime startDate, DateTime endDate) {
		// Iterates over each type of meeting
		foreach (var pair in _meetingTypes) {
			_logger.LogInformation("");
			_logger.LogInformation($"**#### Starting on *{pair.Key}* ####**");

			var archiveUri = new Uri(BaseUri, $"/agendas/archives/{pair.Value}.htm");
			_logger.LogInformation($"URL for this is {archiveUri}");

			var archive = new Archive(archiveUri, pair.Key);

			// Iterates over each year
			foreach (var archiveYear in archive.GetArchiveYears(startDate, endDate)) {
				_logger.LogInformation("");
				_logger.LogInformation($"### ArchiveYear URL {archiveYear.Uri} ###");

				// Iterates over each meeting
				foreach (var meeting in archiveYear.GetMeetings(startDate, endDate)) {
					_logger.LogInformation(
						$"&&& {meeting.Model.MunicipalityName} {meeting.Model.Type} on {meeting.Model.Date}"
					);

					foreach (var document in meeting.GetDocuments()) {
						_logger.LogInformation(
							$"A {document.MeetingsDocumentsType} was found: {document.Uri}"
						);

						if (document is not Minutes minutes) continue;
						// Iterate over each reference
						foreach (var reference in minutes.GetReferences()) {
							Thread.Sleep(5000);
							_logger.LogInformation(
								$"A reference {reference.MeetingsDocumentsType} found: {reference.Uri}"
							);
						}
					}

					foreach (var video in meeting.GetVideos()) {
						_logger.LogInformation($"Found this video {video.Uri}");
					}
				}
			}
		}

		return Task.CompletedTask;
	}
}
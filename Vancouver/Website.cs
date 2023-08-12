using Scraper.Common;

namespace Scraper.Vancouver;

public class Website : IWebsite
{
	private readonly ILogger<Website> _logger;
	
	public static readonly string MunicipalityName = "Vancouver";
	public static readonly Uri BaseUri = new Uri("https://council.vancouver.ca");
	public static Dictionary<string, string> MeetingTypeAbbr = new() {
		{ "public hearing", "phea" },
		{ "regular council meeting", "regu" },
		{ "special council meeting", "spec" },
		{ "in camera meeting", "icre" },
		{ "finance committee", "cfsc" },
		{ "policy and strategic priorities committee", "pspc" },
		{ "nomination subcommittee", "nom" },
		{ "auditor general committee", "agc" },
		{ "auditor general recruitment committee", "agrc" },
		{ "business license hearing", "blhe" },
		{ "court of revision", "crev" },
		{ "inaugural council meeting", "inau" },
		{ "chauffeur's permit appeal hearing", "cpah" }
	};
	

	public Website(ILogger<Website> logger) {
		_logger = logger;
	}

	public Task ScrapeArchive(DateTime startDate, DateTime endDate) {
		throw new NotImplementedException();
	}

	public Task DryRunArchive(DateTime startDate, DateTime endDate) {
		var spa = new Spa();
		
		foreach (var meeting in spa.GetMeetings(startDate, endDate)) {
			_logger.LogInformation("");
			_logger.LogInformation("#### Meeting found ####");
			_logger.LogInformation(meeting.Model.Type);
			_logger.LogInformation(meeting.Model.Date.ToString());
			_logger.LogInformation(meeting.Model.Url);

			foreach (var document in meeting.GetDocuments()) {
				switch (document) {
					case MinutesPdf minutes:
						_logger.LogInformation($"*** Minutes: {minutes.Model.Url}");
						break;
					case ReferencePdf reference:
						_logger.LogInformation($"*** Reference: {reference.Model.Url}");
						break;
				}
			}

			foreach (var video in meeting.GetVideos()) {
				_logger.LogInformation($"*** Video: {video.Model.Url}");
			}
		}

		return Task.CompletedTask;
	}
	
	public Task ScrapeLatest(DateTime startDate, DateTime endDate) {
		throw new NotImplementedException();
	}
	
	public Task DryRunLatest(DateTime startDate, DateTime endDate) {
		throw new NotImplementedException();
	}
}
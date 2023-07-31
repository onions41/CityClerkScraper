
namespace Scraper.Richmond;

internal sealed class TestGetMeetingElements : IHostedService {
  private readonly ILogger _logger;
  private readonly IHostApplicationLifetime _appLifetime;
  private readonly Task _completedTask = Task.CompletedTask;

  public TestGetMeetingElements(
    ILogger<TestGetMeetingElements> logger,
    IHostApplicationLifetime appLifetime
  ) {
    _logger = logger;
    _appLifetime = appLifetime;
  }

  public Task StartAsync(CancellationToken cancellationToken) {
    // Your script goes here

    ArchiveYear archiveYear = new(
      new Uri("https://citycouncil.richmond.ca/schedule/WebAgendaMinutesList.aspx?Category=6&Year=2018"),
      "council meeting"
    );

    IEnumerable<MeetingElement> meetings = archiveYear.GetMeetingElements();

    foreach (MeetingElement meeting in meetings) {
      _logger.LogInformation(meeting.Model.MunicipalityName);
      _logger.LogInformation(meeting.Model.Date.ToLongDateString());
      _logger.LogInformation(meeting.Model.Type);

      foreach (Uri uri in meeting.ResourceUris) {
        _logger.LogInformation(uri.ToString());
      }
    }

    // End script
    _appLifetime.StopApplication();
    return _completedTask;
  }

  public Task StopAsync(CancellationToken cancellationToken) {
    return _completedTask;
  }
}

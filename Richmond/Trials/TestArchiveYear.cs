// using Scraper.Richmond;
//
// namespace Scraper.TestServices;
//
// internal class TestArchiveYear : IHostedService
// {
// 	private readonly ILogger<TestArchiveYear> _logger;
// 	private readonly IHostApplicationLifetime _appLifetime;
// 	private readonly Task _completedTask = Task.CompletedTask;
//
// 	public TestArchiveYear(
// 		ILogger<TestArchiveYear> logger,
// 		IHostApplicationLifetime appLifetime
// 	) {
// 		_logger = logger;
// 		_appLifetime = appLifetime;
// 	}
//
// 	public Task StartAsync(CancellationToken cancellationToken) {
// 		// Your script goes here
//
// 		var archiveYear = new ArchiveYear(
// 			new Uri("http://citycouncil.richmond.ca/schedule/WebAgendaMinutesList.aspx?Category=6&Year=2021"),
// 			"council meeting"
// 		);
//
// 		var meetings = archiveYear.GetMeetings();
//
// 		foreach (var meeting in meetings) {
// 			_logger.LogInformation(meeting.Model.Date.ToLongDateString());
// 			_logger.LogInformation(meeting.Model.Type);
//
// 			foreach (var uri in meeting.ResourceUris) {
// 				_logger.LogInformation(uri.ToString());
// 			}
// 		}
//
// 		// End script
// 		_appLifetime.StopApplication();
// 		return _completedTask;
// 	}
//
// 	public Task StopAsync(CancellationToken cancellationToken) {
// 		return _completedTask;
// 	}
// }
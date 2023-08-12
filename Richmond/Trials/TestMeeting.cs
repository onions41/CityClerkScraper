// using Scraper.Richmond;
// using DataAccess.Data;
//
// namespace Scraper.TestServices;
//
// internal class TestMeeting : IHostedService
// {
// 	private readonly ILogger<TestMeeting> _logger;
// 	private readonly IConfiguration _config;
// 	private readonly IMeetingData _meetingData;
// 	private readonly IHostApplicationLifetime _appLifetime;
// 	private readonly Task _completedTask = Task.CompletedTask;
//
// 	public TestMeeting(
// 		ILogger<TestMeeting> logger,
// 		IHostApplicationLifetime appLifetime,
// 		IConfiguration config,
// 		IMeetingData meetingData
// 	) {
// 		_logger = logger;
// 		_appLifetime = appLifetime;
// 		_config = config;
// 		_meetingData = meetingData;
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
// 			_logger.LogInformation("&&&&&&&&&&&&&&&&");
// 			_logger.LogInformation(meeting.Model.Date.ToLongDateString());
// 			_logger.LogInformation(meeting.Model.Type);
// 			_logger.LogInformation("&&&&&&&&&&&&&&&&");
//
// 			foreach (var document in meeting.GetDocuments()) {
// 				_logger.LogInformation(document.Uri.ToString());
// 				_logger.LogInformation(document.GetType().ToString());
// 				_logger.LogInformation(document.Model.Fingerprint);
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
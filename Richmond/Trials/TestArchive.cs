// using Scraper.Richmond;
//
// namespace Scraper.TestServices;
//
// internal class TestArchive : IHostedService
// {
// 	private readonly ILogger<TestArchive> _logger;
// 	private readonly IHostApplicationLifetime _appLifetime;
// 	private readonly Task _completedTask = Task.CompletedTask;
//
// 	public TestArchive(
// 		ILogger<TestArchive> logger,
// 		IHostApplicationLifetime appLifetime
// 	) {
// 		_logger = logger;
// 		_appLifetime = appLifetime;
// 	}
//
// 	public Task StartAsync(CancellationToken cancellationToken) {
// 		// Your script goes here
//
// 		var archive = new Archive(
// 			new Uri($"https://citycouncil.richmond.ca/agendas/archives/council.htm"),
// 			"council meeting",
// 			new DateTime(2019, 1, 1),
// 			new DateTime(2022, 12, 31)
// 		);
//
// 		IEnumerable<ArchiveYear> archiveYears = archive.GetArchiveYears();
//
// 		foreach (var archiveYear in archiveYears) {
// 			_logger.LogInformation(archiveYear.Uri.ToString());
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
// using Scraper.Richmond;
//
// namespace Scraper.TestServices;
//
// internal class TestRichmond : IHostedService
// {
// 	private readonly ILogger<TestRichmond> _logger;
// 	private readonly IConfiguration _config;
// 	private readonly IHostApplicationLifetime _appLifetime;
// 	private readonly Task _completedTask = Task.CompletedTask;
// 	private readonly IRichmondSite _site;
//
// 	public TestRichmond(
// 		ILogger<TestRichmond> logger,
// 		IConfiguration config,
// 		IHostApplicationLifetime appLifetime,
// 		IRichmondSite site
// 	) {
// 		_logger = logger;
// 		_config = config;
// 		_appLifetime = appLifetime;
// 		_site = site;
// 	}
//
// 	public async Task StartAsync(CancellationToken cancellationToken) {
// 		// Start of script
//
// 		var startDate = new DateTime(2019, 6, 1);
// 		var endDate = new DateTime(2019, 6, 30);
// 		await _site.Scrape(startDate, endDate);
//
// 		// End of script
//
// 		_appLifetime.StopApplication();
// 		// return _completedTask;
// 	}
//
// 	public Task StopAsync(CancellationToken cancellationToken) {
// 		return _completedTask;
// 	}
// }
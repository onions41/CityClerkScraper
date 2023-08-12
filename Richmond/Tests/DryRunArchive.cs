using Scraper.Common;

namespace Scraper.Richmond.Tests;

internal class DryRunArchive : IHostedService
{
	private readonly ILogger<DryRunArchive> _logger;
	private readonly IConfiguration _config;
	private readonly IHostApplicationLifetime _appLifetime;
	private readonly Task _completedTask = Task.CompletedTask;
	private readonly IWebsite _website;

	public DryRunArchive(
		ILogger<DryRunArchive> logger,
		IConfiguration config,
		IHostApplicationLifetime appLifetime,
		IWebsite website
	) {
		_logger = logger;
		_config = config;
		_appLifetime = appLifetime;
		_website = website;
	}
	
	public async Task StartAsync(CancellationToken cancellationToken) {
		// Start of script

		var startDate = new DateTime(2019, 6, 1);
		var endDate = new DateTime(2019, 6, 30);
		await _website.DryRunArchive(startDate, endDate);

		// End of script

		_appLifetime.StopApplication();
		// return _completedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		return _completedTask;
	}
} 

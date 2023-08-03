using Scraper.Richmond;

namespace Scraper;

internal sealed class ScrapingService : IHostedService
{
	private readonly ILogger _logger;
	private readonly IConfiguration _config;
	private readonly IHostApplicationLifetime _appLifetime;
	private readonly Task _completedTask = Task.CompletedTask;
	private readonly ISite _richmond;

	public ScrapingService(
		ILogger<ScrapingService> logger,
		IConfiguration config,
		IHostApplicationLifetime appLifetime,
		ISite richmond
	) {
		_logger = logger;
		_config = config;
		_appLifetime = appLifetime;
		_richmond = richmond;
	}

	public async Task StartAsync(CancellationToken cancellationToken) {
		// Start of script

		var startDate = new DateTime(2013, 1, 1);
		var endDate = new DateTime(2022, 4, 4);

		await _richmond.Scrape(startDate, endDate);
		Console.WriteLine("*******");

		// End of script

		_appLifetime.StopApplication();
		return;
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		return _completedTask;
	}
}

/*
When I run this program, I want it to send a request to OpenAI's GPT endpoint.
I want it to log that it had sent a request to the Endpoint, with details on the prompt used.
I want it to log the response that it gets.
I want it to print the response it gets on the console.
Finally, I want it to exit.

Let me, at first, figure out how to print Hello World, then exit.
*/
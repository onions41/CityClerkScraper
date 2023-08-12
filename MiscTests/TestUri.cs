namespace Scraper.MiscTests;

public class TestUri : IHostedService
{
	private readonly ILogger<TestUri> _logger;
	private readonly IConfiguration _config;
	private readonly IHostApplicationLifetime _appLifetime;
	private readonly Task _completedTask = Task.CompletedTask;

	public TestUri(
		ILogger<TestUri> logger,
		IConfiguration config,
		IHostApplicationLifetime appLifetime
	) {
		_logger = logger;
		_config = config;
		_appLifetime = appLifetime;
	}
	
	public Task StartAsync(CancellationToken cancellationToken) {
		// Start of script

		const string incompleteUrl = "documents/regu20190611min.pdf";
		const string baseUrl = "https://council.vancouver.ca/20190611/regu20190611ag.htm";

		// var incompleteUri = new Uri(incompleteUrl);
		var baseUri = new Uri(baseUrl);
		var fullUri = new Uri(baseUri, incompleteUrl);
		
		Console.WriteLine($"1: {fullUri}");

		// End of script

		_appLifetime.StopApplication();
		return _completedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		return _completedTask;
	}
}
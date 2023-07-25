internal sealed class TestService : IHostedService
{
  private readonly ILogger _logger;
  private readonly IConfiguration _config;
  private readonly IHostApplicationLifetime _appLifetime;
  private readonly Task _completedTask = Task.CompletedTask;
  private readonly IRichmondScraper _richmondScraper;

  public TestService(
    ILogger<TestService> logger,
    IHostApplicationLifetime appLifetime,
    IConfiguration config,
    IRichmondScraper richmondScraper)
  {
    _logger = logger;
    _appLifetime = appLifetime;
    _config = config;
    _richmondScraper = richmondScraper;
  }

  public Task StartAsync(CancellationToken cancellationToken)
  {
    // Your script goes here
    var startDate = new DateTime(2013, 1, 1);
    var endDate = new DateTime(2022, 4, 4);

    _richmondScraper.Scrape(startDate, endDate);

    // End script
    _appLifetime.StopApplication();
    return _completedTask;
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    return _completedTask;
  }
}

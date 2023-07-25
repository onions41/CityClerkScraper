internal sealed class TestService2 : IHostedService
{
  private readonly ILogger _logger;
  private readonly IConfiguration _config;
  private readonly IHostApplicationLifetime _appLifetime;
  private readonly Task _completedTask = Task.CompletedTask;
  private readonly IRichmondScraper _richmondScraper;

  public TestService2(
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
 

    // End script
    _appLifetime.StopApplication();
    return _completedTask;
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    return _completedTask;
  }
}

using MuPDFCore;

namespace Scraper.MiscTests;

internal sealed class TestService : IHostedService {
  private readonly ILogger _logger;
  private readonly IConfiguration _config;
  private readonly IHostApplicationLifetime _appLifetime;
  private readonly Task _completedTask = Task.CompletedTask;

  public TestService(
    ILogger<TestService> logger,
    IHostApplicationLifetime appLifetime,
    IConfiguration config
  ) {
    _logger = logger;
    _appLifetime = appLifetime;
    _config = config;
  }

  public async Task StartAsync(CancellationToken cancellationToken) {
    // Your script goes here

    // using HttpClient httpClient = new() { BaseAddress = new Uri("https://citycouncil.richmond.ca") };
    using HttpClient httpClient = new();

    // https://citycouncil.richmond.ca/agendafiles/Open_Council_5-28-2018.pdf
    // byte[] pdfByteArr = httpClient.GetByteArrayAsync("agendafiles/Open_Council_5-28-2018.pdf").Result;
    byte[] pdfByteArr = await httpClient.GetByteArrayAsync("https://www.africau.edu/images/default/sample.pdf");

    using MuPDFContext muCtx = new();
    using MuPDFDocument muPdf = new(muCtx, data: pdfByteArr, fileType: InputFileTypes.PDF);

    var text = muPdf.ExtractText();

    // using (StreamWriter writer = new("output.txt")) {
    //   writer.Write(muPdf.ExtractText());
    // }

    Console.Write(text);

    // End script
    _appLifetime.StopApplication();
    // return _completedTask;
  }

  public Task StopAsync(CancellationToken cancellationToken) {
    return _completedTask;
  }
}

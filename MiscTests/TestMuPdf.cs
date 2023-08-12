using MuPDFCore;

namespace Scraper.MiscTests;

public class TestMuPdf : IHostedService
{
	private readonly ILogger<TestMuPdf> _logger;
	private readonly IConfiguration _config;
	private readonly IHostApplicationLifetime _appLifetime;
	private readonly Task _completedTask = Task.CompletedTask;

	public TestMuPdf(
		ILogger<TestMuPdf> logger,
		IConfiguration config,
		IHostApplicationLifetime appLifetime
	) {
		_logger = logger;
		_config = config;
		_appLifetime = appLifetime;
	}

	public Task StartAsync(CancellationToken cancellationToken) {
		// Start of script

		const string filePath = "notebook/Bylaw_971354580.pdf"; // Specify the file path

		var pdfByteArr = File.ReadAllBytes(filePath); // Read all bytes from the file

		Console.WriteLine("Total number of bytes read: " + pdfByteArr.Length);

		using var ctx = new MuPDFContext();
		using var muPdfDoc = new MuPDFDocument(ctx, data: pdfByteArr, fileType: InputFileTypes.PDF);

		
		
		var content = muPdfDoc.ExtractText(includeAnnotations: false);
		_logger.LogInformation($"String length is {content.Length}");
		_logger.LogInformation(content);

		// End of script

		_appLifetime.StopApplication();
		return _completedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		return _completedTask;
	}
}
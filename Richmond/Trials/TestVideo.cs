// namespace Scraper.Richmond;
//
// internal sealed class TestVideoElement : IHostedService {
//   private readonly ILogger _logger;
//   private readonly IHostApplicationLifetime _appLifetime;
//   private readonly Task _completedTask = Task.CompletedTask;
//   private readonly IVideoData _videoData;
//
//   public TestVideoElement(
//     ILogger<TestVideoElement> logger,
//     IHostApplicationLifetime appLifetime,
//     IVideoData videoData
//   ) {
//     _logger = logger;
//     _appLifetime = appLifetime;
//     _videoData = videoData;
//   }
//
//   public async Task StartAsync(CancellationToken cancellationToken) {
//     // Your script goes here
//
//     Uri uri = new("https://youtu.be/0Ac_COGqgIc");
//     Video video = new(uri);
//
//     uint id = await video.Save(_videoData);
//     Console.WriteLine($"Video was saved, the generated ID is: {id}");
//
//     // End script
//     _appLifetime.StopApplication();
//   }
//
//   public Task StopAsync(CancellationToken cancellationToken) {
//     return _completedTask;
//   }
// }

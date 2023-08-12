// using Scraper.Richmond;
// using Scraper.Common;
//
// namespace Scraper.TestServices;
//
// public class TestDocument : IHostedService
// {
//     private readonly ILogger<TestDocument> _logger;
//     private readonly IHostApplicationLifetime _appLifetime;
//     private readonly Task _completedTask = Task.CompletedTask;
//
//     public TestDocument(
//         ILogger<TestDocument> logger,
//         IHostApplicationLifetime appLifetime
//     )
//     {
//         _logger = logger;
//         _appLifetime = appLifetime;
//     }
//
//     public Task StartAsync(CancellationToken cancellationToken)
//     {
//         // Your script goes here
//
//         var document = new Minutes(
//             // new Uri("https://citycouncil.richmond.ca/agendas/archives/council/2018/112618_minutes.htm"),
//             new Uri("https://citycouncil.richmond.ca/agendas/archives/council/2021/120621_minutes.htm")
//         );
//         // new Uri("https://citycouncil.richmond.ca/agendas/archives/council/2021/120621_minutes.htm")
//         // // https://citycouncil.richmond.ca/agendas/archives/council/2021/112221_minutes.htm
//  
//         foreach (var reference in document.GetReferences()) {
//             _logger.LogInformation("$$$$$$$$$$$$$$$$$$$$$$$$");
//             _logger.LogInformation(reference.GetType().ToString());
//             _logger.LogInformation(reference.Uri.ToString());
//  }
//
//         // End script
//         _appLifetime.StopApplication();
//         return _completedTask;
//     }
//
//     public Task StopAsync(CancellationToken cancellationToken)
//     {
//         return _completedTask;
//     }
// }
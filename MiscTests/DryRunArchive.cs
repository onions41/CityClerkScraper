using Scraper.Common;
using Vancouver = Scraper.Vancouver;
using Richmond = Scraper.Richmond;

namespace Scraper.MiscTests;

internal class DryRunArchive : IHostedService
{
   private readonly ILogger<DryRunArchive> _logger;
   private readonly IConfiguration _config;
   private readonly IHostApplicationLifetime _appLifetime;
   private readonly Task _completedTask = Task.CompletedTask;
   private readonly IEnumerable<IWebsite> _websites;

   public DryRunArchive(
      ILogger<DryRunArchive> logger,
      IConfiguration config,
      IHostApplicationLifetime appLifetime,
      IEnumerable<IWebsite> websites
   ) {
      _logger = logger;
      _config = config;
      _appLifetime = appLifetime;
      _websites = websites;
   }

   public async Task StartAsync(CancellationToken cancellationToken) {
      // Start of script

      foreach (var website in _websites) {
         if (website is not Richmond.Website) continue;
         
         var startDate = new DateTime(2019, 6, 1);
         var endDate = new DateTime(2019, 6, 30);
         
         await website.DryRunArchive(startDate, endDate);
      }

      // End of script

      _appLifetime.StopApplication();
      // return _completedTask;
   }

   public Task StopAsync(CancellationToken cancellationToken) {
      return _completedTask;
   }
}
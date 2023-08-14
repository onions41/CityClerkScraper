using DataAccess.Data;
using DataAccess.Models;
using Scraper.Vancouver;

namespace Scraper.MiscTests;

public class TestPgSqlFunc : IHostedService
{
   private readonly ILogger<TestPgSqlFunc> _logger;
   private readonly IConfiguration _config;
   private readonly IHostApplicationLifetime _appLifetime;
   private readonly Task _completedTask = Task.CompletedTask;
   private readonly IMeetingData _meetings;

   public TestPgSqlFunc(
      ILogger<TestPgSqlFunc> logger,
      IConfiguration config,
      IHostApplicationLifetime appLifetime,
      IMeetingData meetings
   ) {
      _logger = logger;
      _config = config;
      _appLifetime = appLifetime;
      _meetings = meetings;
   }

   public async Task StartAsync(CancellationToken cancellationToken) {
      // Start of script

      var meeting = new MeetingModel() {
         MunicipalityName = "Burnaby",
         Type = "moved up to scraper",
         Date = new DateTime(2022, 01, 03)
      };
      
      var something = await _meetings.InsertMeeting(meeting);
      Console.WriteLine(something is null);
      // End of script

      _appLifetime.StopApplication();
      // return _completedTask;
   }

   public Task StopAsync(CancellationToken cancellationToken) {
      return _completedTask;
   }
}
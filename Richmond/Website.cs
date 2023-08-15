using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using DataAccess.Data;
using Scraper.Common;

namespace Scraper.Richmond;

internal class Website : IWebsite
{
   private readonly ILogger<Website> _logger;
   private readonly IMeetingData _meetingData;
   private readonly IDocumentData _documentData;
   private readonly IVideoData _videoData;

   public static readonly string MunicipalityName = "Richmond";
   public static readonly Uri BaseUri = new Uri("https://citycouncil.richmond.ca");

   public readonly Dictionary<string, string> MeetingTypes = new() {
      { "council meeting", "council" },
      { "community safety committee", "safety" },
      { "development permit panel", "dpp" },
      { "finance committee", "finance" },
      { "general purpose committee", "gp" },
      { "parks, recreation, and cultural services committee", "prcs" },
      { "planning committee", "planning" },
      { "public hearing", "hearings" },
      { "public works and transportation committee", "pwt" }
   };

   public Website(ILogger<Website> logger, IMeetingData meetingData, IDocumentData documentData, IVideoData videoData) {
      _logger = logger;
      _meetingData = meetingData;
      _documentData = documentData;
      _videoData = videoData;
   }

   public Task ScrapeArchive(DateTime startDate, DateTime endDate) {
      throw new NotImplementedException();
   }

   public async Task DryRunArchive(DateTime startDate, DateTime endDate) {
      foreach (var pair in MeetingTypes) {
         _logger.LogInformation("");
         _logger.LogInformation($"#### Starting on *{pair.Key}* archive #####");

         var archiveUri = new Uri(BaseUri, $"/agendas/archives/{pair.Value}.htm");
         _logger.LogInformation($"URL for this is {archiveUri}");

         var archive = new Archive(archiveUri, pair.Key);

         // Iterates over each year
         foreach (var archiveYear in archive.GetArchiveYears(startDate, endDate)) {
            _logger.LogInformation("");
            _logger.LogInformation($"*### ArchiveYear URL {archiveYear.Uri} ###*");

            // Iterates over each meeting
            foreach (var meeting in archiveYear.GetMeetings(startDate, endDate)) {
               _logger.LogInformation(
                  $"Found a meeting {meeting.Model.MunicipalityName} {meeting.Model.Type} on {meeting.Model.Date}"
               );

               await meeting.Save(_meetingData);

               foreach (var document in meeting.GetDocuments()) {
                  _logger.LogInformation(
                     $"A {document.MeetingsDocumentsType} was found: {document.Model.Url}"
                  );

                  await document.Save(_documentData);

                  if (document is not Minutes minutes) continue;
                  // Iterate over each reference
                  foreach (var reference in minutes.GetReferences()) {
                     Thread.Sleep(5000);
                     _logger.LogInformation(
                        $"A reference {reference.MeetingsDocumentsType} found: {reference.Model.Url}"
                     );

                     await reference.Save(_documentData);
                  }
               }

               foreach (var video in meeting.GetVideos()) {
                  _logger.LogInformation($"Found this video {video.Model.Url}");

                  await video.Save(_videoData);
               }
            }
         }
      }

      // return Task.CompletedTask;
   }

   public Task ScrapeLatest(DateTime startDate, DateTime endDate) {
      throw new NotImplementedException();
   }

   public Task DryRunLatest(DateTime startDate, DateTime endDate) {
      throw new NotImplementedException();
   }
}
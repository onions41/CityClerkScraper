using DataAccess.Models;
using DataAccess.Data;

namespace Scraper.Common;

internal abstract class MeetingBase
{
	public MeetingModel? Model { get; protected init; }
	
	public async Task Save(IMeetingData meetingData) {
   	if (Model is null) throw new Exception("Cannot save meeting before its Model is initialized");
   	var meetingId = await meetingData.InsertMeeting(Model);
   	Model.Id = meetingId;
   }
	
	public abstract IEnumerable<DocumentBase> GetDocuments();
	public abstract IEnumerable<Video> GetVideos();
}
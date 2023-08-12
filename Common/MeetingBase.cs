using DataAccess.Models;

namespace Scraper.Common;

internal abstract class MeetingBase
{
	public MeetingModel Model { get; protected init; }
	public abstract IEnumerable<DocumentBase> GetDocuments();
	public abstract IEnumerable<Video> GetVideos();
}
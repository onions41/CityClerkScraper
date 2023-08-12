using DataAccess.Models;

namespace Scraper.Common;

internal class Video
{
	public Video(Uri uri, string type) {
		Model = new VideoModel() {
			Url = uri.ToString()
		};
		MeetingsVideosType = type;
	}
	
	public VideoModel Model { get; }
	public string MeetingsVideosType { get;}
}
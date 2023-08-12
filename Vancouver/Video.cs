using Scraper.Common;

namespace Scraper.Vancouver;

internal class Video : VideoBase
{
	public Video(Uri uri) {
		Model = new VideoModel() {
			Url = uri.ToString()
		};
		MeetingsVideosType = "meeting video";
	}
}
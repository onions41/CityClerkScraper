using DataAccess.Models;
using DataAccess.Data;

namespace Scraper.Richmond;

internal class Video
{
	public Video(Uri uri, string type) {
		Uri = uri;
		Type = type;

		Model = new VideoModel() {
			Url = uri.ToString()
		};
    }

	public Uri Uri { get; private init; }
	public string Type { get; private init; }
	public VideoModel? Model { get; private init; }

	// public async Task<uint> Save(IVideoData db) {
	//   return await db.InsertVideo(_model);
	// }
}
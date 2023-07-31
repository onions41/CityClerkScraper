using DataAccess.Models;
using DataAccess.Data;

namespace Scraper.Richmond;

internal class VideoElement {
  private readonly VideoModel _model;

  public VideoElement(Uri uri) {
    _model = new VideoModel() {
      Url = uri.ToString()
    };
  }

  public async Task<uint> Save(IVideoData db) {
    return await db.InsertVideo(_model);
  }
}
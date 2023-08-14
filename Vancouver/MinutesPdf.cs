using Scraper.Common;

namespace Scraper.Vancouver;

internal class MinutesPdf : PdfDocument
{
	public MinutesPdf(Uri uri , int meetingId) : base(uri, meetingId) {
		MeetingsDocumentsType = "minutes";
	}
}
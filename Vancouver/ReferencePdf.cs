using Scraper.Common;

namespace Scraper.Vancouver;

internal class ReferencePdf : PdfDocument
{
	public ReferencePdf(Uri uri, int meetingId) : base(uri, meetingId) {
		MeetingsDocumentsType = "reference";
	}
}
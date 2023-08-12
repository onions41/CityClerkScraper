using Scraper.Common;

namespace Scraper.Vancouver;

internal class MinutesPdf : PdfDocument
{
	public MinutesPdf(Uri uri) : base(uri) {
		MeetingsDocumentsType = "minutes";
	}
}
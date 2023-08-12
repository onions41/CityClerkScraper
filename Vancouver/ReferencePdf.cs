using Scraper.Common;

namespace Scraper.Vancouver;

internal class ReferencePdf : PdfDocument
{
	public ReferencePdf(Uri uri) : base(uri) {
		MeetingsDocumentsType = "reference";
	}
}
using System.Text;
using DataAccess.Models;
using Scraper.Common;

namespace Scraper.Richmond;

internal class StaffReport : DocumentBase
{
	public StaffReport(Uri uri, int meetingId) : base(uri, meetingId) {
		MeetingsDocumentsType = "staff report";
	}

	protected override void Parse() {
		// The staff report is a very long PDF containing all of their reports and appendix content.
		// It includes drawings, tables, and charts, and is not ideal to parse as a simple string.
		// I will keep only the URL of the staff reports, as I don't think it would be worth the tokens
		// to parse staff reports with GPT.

		// The fingerprint of a staff report is the last 100 characters of its URL
		// If the URL is shorter than 100 characters, it will be padded on the right.
		// I'm not relying in Postgres padding for database compatability
		var url = Uri.ToString();
		var fingerprint = GenerateFingerprint(url);

		Model = new DocumentModel() {
			Url = url,
			Fingerprint = fingerprint
		};
	}
}
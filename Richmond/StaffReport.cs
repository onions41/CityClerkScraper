using System.Text;
using DataAccess.Models;
using Scraper.Common;

namespace Scraper.Richmond;

internal class StaffReport : DocumentBase
{
	public StaffReport(Uri uri) : base(uri) {
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

	protected override byte[] GenerateFingerprint<T>(T urlStr) {
		if (urlStr is not string url) throw new Exception("Impossible. Url is not of type string.");
		var fingerprint = new byte[100];
		var urlBytes = Encoding.ASCII.GetBytes(url);
		
		if (urlBytes.Length > 100) {
			var middleIndex = urlBytes.Length / 2;
			var startIndex = middleIndex - 50;
			Array.Copy(
				urlBytes,
				startIndex,
				fingerprint,
				0,
				100
			);
		} else {
			fingerprint = urlBytes;
		}
		
		return fingerprint;
	}
}
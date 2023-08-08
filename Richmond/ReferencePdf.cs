using System.Text;
using DataAccess.Models;
using HtmlAgilityPack;
using MuPDFCore;
using System.Text.RegularExpressions;
using Scraper.Common;

namespace Scraper.Richmond;

internal class ReferencePdf : DocumentBase
{
	// private readonly HtmlDocument _page;
	public ReferencePdf(Uri uri, string linkText) : base(uri) {
		LinkText = linkText;
		MeetingsDocumentsType = "reference";
		// _page = new HtmlWeb().Load(uri);


		var url = uri.ToString();
		if (Regex.IsMatch(url, @"pdf$")) {
			FileExt = "pdf";
		} else if (Regex.IsMatch(url, @"htm$")) {
			FileExt = "htm";
		} else {
			throw new Exception("Can't tell what the file extension is");
		}
	}

	public string LinkText { get; private init; }
	public string FileExt { get; private init; }

	protected override void Parse() {
		using var httpClient = new HttpClient();
		var pdfByteArr = httpClient.GetByteArrayAsync(Uri).Result;
		
		// // Extracts the text out of the pdfByteArr with MuPDF. This should be moved to another service. Like a processor service.
		// using var ctx = new MuPDFContext();
		// using var muPdfDoc = new MuPDFDocument(ctx, data: pdfByteArr, fileType: InputFileTypes.PDF);
		// var content = muPdfDoc.ExtractText(includeAnnotations: false);
		
		var fingerprint = GenerateFingerprint(pdfByteArr);

		Model = new DocumentModel() {
			Url = Uri.ToString(),
			Fingerprint = fingerprint,
			RawContent = pdfByteArr
		};
	}

	protected override byte[] GenerateFingerprint<T>(T content) {
		if (content is not byte[] pdf) throw new Exception("Impossible. Content is not of byte[] type.");
		
		var middleIndex = pdf.Length / 2;
		var startIndex = middleIndex - 50;
		var fingerprint = new byte[100];
		
		// pdf is shorter than 100 bytes
		if (startIndex < 0) {
			fingerprint = pdf;
		} else {
			// Copy the middle 100 bytes into the new array
			Array.Copy(
				pdf,
				startIndex,
				fingerprint,
				0,
				100
			);
		}

		return fingerprint;
	}
}
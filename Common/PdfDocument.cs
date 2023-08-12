using DataAccess.Models;
using MuPDFCore;

namespace Scraper.Common;

internal abstract class PdfDocument : DocumentBase
{
	public PdfDocument(Uri uri) : base(uri) {
	}

	protected override void Parse() {
		using var httpClient = new HttpClient();
		var pdfByteArr = httpClient.GetByteArrayAsync(_uri).Result;
		var fingerprint = GenerateFingerprint(pdfByteArr);

		using var ctx = new MuPDFContext();
		using var muPdfDoc = new MuPDFDocument(ctx, data: pdfByteArr, fileType: InputFileTypes.PDF);

		Model = new DocumentModel() {
			Url = _uri.ToString(),
			Fingerprint = fingerprint,
		};

		var textContent = muPdfDoc.ExtractText(includeAnnotations: false);
		
		if (textContent is null || textContent.Length < 300) {
			// Save the bytes
			Model.Format = "pdf";
			Model.RawContent = pdfByteArr;
		} else {
			// Save the extracted text
			Model.TextContent = textContent;
		}
	}
}
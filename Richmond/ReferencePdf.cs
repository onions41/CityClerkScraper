using System.Text;
using DataAccess.Models;
using HtmlAgilityPack;
using MuPDFCore;
using System.Text.RegularExpressions;
using Scraper.Common;

namespace Scraper.Richmond;

internal class ReferencePdf : PdfDocument
{
	public ReferencePdf(Uri uri) : base(uri) {
		MeetingsDocumentsType = "reference";
	}
}
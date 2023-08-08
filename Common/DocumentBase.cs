using System.Text;
using System.Text.RegularExpressions;
using DataAccess.Models;
using HtmlAgilityPack;
using MuPDFCore;

namespace Scraper.Common;

internal abstract class DocumentBase
{
	protected DocumentBase(Uri uri) {
		Uri = uri;
		// Ignore this warning. It is calling an abstract method, not a virtual method.
		// Which means there is no ambiguity over with implementation is being called.
		Parse(); 
	}

	public Uri Uri { get; private init; }
	public DocumentModel? Model { get; protected set; }
	public string MeetingsDocumentsType { get; protected init; }
	protected abstract void Parse();
	protected abstract byte[] GenerateFingerprint<T>(T content);
}
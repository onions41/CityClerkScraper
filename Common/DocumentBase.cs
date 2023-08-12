using System.Text;
using System.Text.RegularExpressions;
using DataAccess.Models;
using HtmlAgilityPack;
using MuPDFCore;

namespace Scraper.Common;

internal abstract class DocumentBase
{
	protected readonly Uri _uri;

	protected DocumentBase(Uri uri) {
		_uri = uri;
		// Ignore this warning. It is calling an abstract method, not a virtual method.
		// Which means there is no ambiguity over with implementation is being called.
		Thread.Sleep(1000);
		Parse(); 
	}

	public DocumentModel Model { get; protected set; }
	public string MeetingsDocumentsType { get; protected init; }
	protected abstract void Parse();

	protected static byte[] GenerateFingerprint(byte[] content) {
		var middleIndex = content.Length / 2;
		var startIndex = middleIndex - 50;
		var fingerprint = new byte[100];
		
		// pdf is shorter than 100 bytes
		if (startIndex < 0) {
			fingerprint = content;
		} else {
			// Copy the middle 100 bytes into the new array
			Array.Copy(
				content,
				startIndex,
				fingerprint,
				0,
				100
			);
		}
		
		return fingerprint;
	}

	protected static byte[] GenerateFingerprint(string content) {
		var fingerprint = new byte[100];
		var contentBytes = Encoding.ASCII.GetBytes(content);
		
		if (contentBytes.Length > 100) {
			var middleIndex = contentBytes.Length / 2;
			var startIndex = middleIndex - 50;
			Array.Copy(
				contentBytes,
				startIndex,
				fingerprint,
				0,
				100
			);
		} else {
			fingerprint = contentBytes;
		}
		
		return fingerprint;
	}
	
}
using System.Text.RegularExpressions;
using DataAccess.Models;
using DataAccess.Data;
using Scraper.Common;

namespace Scraper.Richmond;

internal class Meeting
{
	private readonly string _meetingType;
	private const string MunicipalityName = "Richmond";

	public Meeting(
		DateTime date,
		string meetingType,
		List<Uri> resourceUris
	) {
		_meetingType = meetingType;

		Model = new MeetingModel() {
			MunicipalityName = MunicipalityName,
			Type = meetingType,
			Date = date,
		};

		ResourceUris = resourceUris;
	}

	public MeetingModel Model { get; private init; }
	public List<Uri> ResourceUris { get; private init; }

	public IEnumerable<DocumentBase> GetDocuments() {
		foreach (var uri in ResourceUris) {
			var url = uri.ToString();

			if (Regex.IsMatch(url, "Open_Council.+pdf$")) {
				yield return new StaffReport(uri);
			} else if (Regex.IsMatch(url, @"agenda\.htm$")) {
				yield return new Agenda(uri);
			} else if (Regex.IsMatch(url, @"minutes\.htm$")) {
				yield return new Minutes(uri, "minutes");
			}
		}
	}

	public IEnumerable<Video> GetVideos() {
		foreach (var uri in ResourceUris) {
			var url = uri.ToString();
			string type;

			// If URL matches, it is a document.
			if (Regex.IsMatch(url, @"youtu\.be")) {
				type = "meeting recording";
			} else if (Regex.IsMatch(url, @"MediaPlayer\.php")) {
				type = "meeting recording youtube";
			} else {
				continue;
			}

			yield return new Video(uri, type);
		}
	}
}
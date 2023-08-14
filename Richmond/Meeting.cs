using System.Text.RegularExpressions;
using DataAccess.Models;
using DataAccess.Data;
using Scraper.Common;

namespace Scraper.Richmond;

internal class Meeting : MeetingBase
{
	private readonly string _meetingType;

	public Meeting(
		DateTime date,
		string meetingType,
		List<Uri> resourceUris
	) {
		_meetingType = meetingType;

		Model = new MeetingModel() {
			MunicipalityName = Website.MunicipalityName,
			Type = meetingType,
			Date = date,
		};

		ResourceUris = resourceUris;
	}
	public List<Uri> ResourceUris { get; }

	public override IEnumerable<DocumentBase> GetDocuments() {
		foreach (var uri in ResourceUris) {
			var url = uri.ToString();
            
			if (Model?.Id is null) throw new Exception("MeetingId is null when trying to create get a document");

			if (Regex.IsMatch(url, "Open_Council.+pdf$")) {
				yield return new StaffReport(uri, (int)Model.Id);
			} else if (Regex.IsMatch(url, @"agenda\.htm$")) {
				yield return new Agenda(uri, (int)Model.Id);
			} else if (Regex.IsMatch(url, @"minutes\.htm$")) {
				yield return new Minutes(uri, (int)Model.Id,"minutes");
			}
		}
	}

	public override IEnumerable<Video> GetVideos() {
		foreach (var uri in ResourceUris) {
			var url = uri.ToString();
			string type;

			// If URL matches, it is a document.
			if (Regex.IsMatch(url, @"youtu\.be")) {
				type = "meeting video";
			} else if (Regex.IsMatch(url, @"MediaPlayer\.php")) {
				type = "meeting video youtube";
			} else {
				continue;
			}

			yield return new Video(uri, type);
		}
	}
}
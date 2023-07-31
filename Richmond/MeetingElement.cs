using DataAccess.Models;
using DataAccess.Data;

internal class MeetingElement {
  private readonly MeetingModel _model;
  private readonly string _municipalityName = "Richmond";

  public MeetingElement(
    DateTime date,
    string meetingType,
    List<Uri> resourceUris
  ) {
    _model = new MeetingModel() {
      MunicipalityName = _municipalityName,
      Type = meetingType,
      Date = date,
    };

    ResourceUris = resourceUris;
  }

  public List<Uri> ResourceUris { init; get; }

  public async Task Save(MeetingData data) {
    await data.InsertMeeting(_model);
  }
}
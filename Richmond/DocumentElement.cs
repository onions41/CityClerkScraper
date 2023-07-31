using System.Text.RegularExpressions;

internal class DocumentElement {
  private readonly string _type;
  public DocumentElement(Uri uri) {
    string url = uri.ToString();

    Regex.Match(url, "");
      // if url contains the string "minutes"
      // if url contains the string "open... .pdf"
      // if url contains youtu.be
  }
}

// When you come back, create a videos table
// then change the meetings_documents table to meetings_resources table
// it will need to have a resource_id and type Enum to make the join table polymorphic

//
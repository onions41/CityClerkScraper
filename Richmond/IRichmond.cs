namespace Scraper.Richmond;

internal interface IRichmond
{
  Task Scrape(DateTime startDate, DateTime endDate);
}
namespace Scraper.Richmond;

internal interface IRichmondScraper
{
  Task Scrape(DateTime startDate, DateTime endDate);
}
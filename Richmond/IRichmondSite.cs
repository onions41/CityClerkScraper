namespace Scraper.Richmond;

internal interface IRichmondSite
{
	Task Scrape(DateTime startDate, DateTime endDate);
}
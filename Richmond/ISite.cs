namespace Scraper.Richmond;

internal interface ISite
{
	Task Scrape(DateTime startDate, DateTime endDate);
}
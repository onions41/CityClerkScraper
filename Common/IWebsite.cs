namespace Scraper.Common;

public interface IWebsite
{
	Task ScrapeLatest(DateTime startDate, DateTime endDate);
	Task DryRunLatest(DateTime startDate, DateTime endDate);
	Task ScrapeArchive(DateTime startDate, DateTime endDate);
	Task DryRunArchive(DateTime startDate, DateTime endDate);
}
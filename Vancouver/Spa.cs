using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Scraper.Common;

namespace Scraper.Vancouver;

internal class Spa
{
	private readonly Uri _spaUri = new("https://covapp.vancouver.ca/councilMeetingPublic/CouncilMeetings.aspx");

	public Spa() {
	}

	public IEnumerable<Meeting> GetMeetings(DateTime startDate, DateTime endDate) {
		// Initialize Selenium Chrome driver
		var options = new ChromeOptions();
		// options.AddArgument("--headless=new");
		var chromeDriver = new ChromeDriver(options);
		// Used for waiting explicitly
		var wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(5));
		// wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

		// Navigate to SPA landing page and click the button that reveals the date range fields
		chromeDriver.Navigate().GoToUrl(_spaUri);
		chromeDriver.FindElement(By.LinkText("By Date")).Click(); // Found waiting to be unnecessary here

		// Initialize chunk date range
		var chunkStartDate = startDate;
		var chunkEndDate = (chunkStartDate + TimeSpan.FromDays(90) > endDate)
			? endDate
			: chunkStartDate + TimeSpan.FromDays(90);

		// Loops over date range chunks
		while (true) {
			// Fill from-to date fields
			wait.Until(d => d.FindElement(By.Id("LiverpoolTheme_wt93_block_wtMainContent_wttxtFromDate")).Displayed);
			chromeDriver.FindElement(By.Id("LiverpoolTheme_wt93_block_wtMainContent_wttxtFromDate")).Clear();
			chromeDriver.FindElement(By.Id("LiverpoolTheme_wt93_block_wtMainContent_wttxtFromDate"))
				.SendKeys(chunkStartDate.ToString("yyyy-MM-dd"));
			chromeDriver.FindElement(By.Id("LiverpoolTheme_wt93_block_wtMainContent_wttxtToDate")).Clear();
			chromeDriver.FindElement(By.Id("LiverpoolTheme_wt93_block_wtMainContent_wttxtToDate"))
				.SendKeys(chunkEndDate.ToString("yyyy-MM-dd"));

			// Click button that submits the field values
			// Waiting not necessary as page didn't change since above step
			var displayButton = chromeDriver.FindElement(By.XPath("//input[@value='Display']"));
			try { // In case clicking via driver doesn't work, click via JavaScript.
				displayButton.Click();
			} catch (Exception) {
				// arguments[0] is displayButton as DOM
				chromeDriver.ExecuteScript("arguments[0].click();", displayButton);
			}
			wait.Until(d => {
				try {
					return !displayButton.Displayed;
				} catch (StaleElementReferenceException) {
					return true;
				}
			});

			// Loops over each page in date range
			while (true) {
				// Wait until a link is displayed inside the table then grab all the links
				// (The wait feature of Selenium is finicky. Trying to make this code neater will only result in uncaught
				// stale element errors)
				try {
					wait.Until(d =>
						d.FindElement(By.Id("LiverpoolTheme_wt93_block_wtMainContent_wtTblCommEventTable"))
							.FindElement(By.XPath(".//a[contains(@href, '.htm')]")).Displayed
					);
				} catch (WebDriverTimeoutException) {
					// No links found in this date range
					break;
				}

				var links = chromeDriver
					.FindElement(By.Id("LiverpoolTheme_wt93_block_wtMainContent_wtTblCommEventTable"))
					.FindElements(By.XPath(".//a[contains(@href, '.htm')]"));
				foreach (var link in links) {
					// Yield a meeting
					var meetingUri = new Uri(link.GetAttribute("href"));
					yield return new Meeting(meetingUri);
				}

				// Check if nextButton exists
				try {
					wait.Until(d => d.FindElement(By.XPath("//a[text()='next']")).Displayed);
				} catch (WebDriverTimeoutException) {
					break;
				}

				// Click nextButton then wait until it becomes stale
				var nextButton = chromeDriver.FindElement(By.XPath("//a[text()='next']"));
				nextButton.Click();
				wait.Until(d => {
					try {
						return !nextButton.Displayed;
					} catch (StaleElementReferenceException) {
						return true;
					}
				});
			}

			// Set dates for next iteration
			chunkStartDate = chunkEndDate + TimeSpan.FromDays(1);
			if (chunkStartDate > endDate) break;
			chunkEndDate = (chunkStartDate + TimeSpan.FromDays(90) > endDate)
				? endDate
				: chunkStartDate + TimeSpan.FromDays(90);
		}

		chromeDriver.Quit();
	}
}
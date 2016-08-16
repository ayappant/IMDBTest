using System;
using System.Configuration;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using IMDBTests.Utilities;
using OpenQA.Selenium.Support.UI;


namespace IMDBTests.SeleniumHelpers
{
  public enum DriverToUse
  {
    InternetExplorer,
    Chrome,
    Firefox
  }

  public class DriverFactory
  {
    private static readonly FirefoxProfile FirefoxProfile = CreateFirefoxProfile();

    private static FirefoxProfile CreateFirefoxProfile()
    {
      var firefoxProfile = new FirefoxProfile();
      firefoxProfile.SetPreference("network.automatic-ntlm-auth.trusted-uris", "http://localhost");
      return firefoxProfile;
    }

    public IWebDriver Create()
    {
      IWebDriver driver;
      var driverToUse = ConfigurationHelper.Get<DriverToUse>("DriverToUse");
      
        switch (driverToUse)
        {
          case DriverToUse.InternetExplorer:
            var options = new InternetExplorerOptions();
            options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            driver = new InternetExplorerDriver(ConfigurationHelper.internetExplorerDriverPath, options);
            break;
          case DriverToUse.Firefox:
            var firefoxProfile = FirefoxProfile;
            driver = new FirefoxDriver(firefoxProfile);
            driver.Manage().Window.Maximize();
            break;
          case DriverToUse.Chrome:
            driver = new ChromeDriver(ConfigurationHelper.chromeDriverPath);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      

      driver.Manage().Window.Maximize();
      var timeouts = driver.Manage().Timeouts();

      timeouts.ImplicitlyWait(TimeSpan.FromSeconds(ConfigurationHelper.Get<int>("ImplicitlyWait")));

      timeouts.SetPageLoadTimeout(TimeSpan.FromSeconds(ConfigurationHelper.Get<int>("PageLoadTimeout")));

      // Suppress the onbeforeunload event first. This prevents the application hanging on a dialog box that does not close.
      ((IJavaScriptExecutor)driver).ExecuteScript("window.onbeforeunload = function(e){};");
      return driver;
    }

    

    public static void WaitForElementVisible(IWebDriver _webDriver,By by, int timeOutInSeconds)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      try
      {
        var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(timeOutInSeconds));
        wait.Until(ExpectedConditions.ElementIsVisible(by));
      }
      catch (Exception)
      {
        Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed.Seconds);
      }
      finally
      {
        stopwatch.Stop();
      }
    }

  }
}
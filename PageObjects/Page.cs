using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using NUnit.Framework;
using IMDBTests.Utilities;
using IMDBTests.SeleniumHelpers;

namespace Tests.PageObjects
{
  public class Page
  {
    private readonly IWebDriver _driver;

    public Page(IWebDriver driver)
    {
      _driver = driver;
      PageFactory.InitElements(_driver, this);
    }

    [FindsBy(How = How.XPath, Using = "//td[@class='titleColumn']")]
    public IList<IWebElement> titleList { get; set; }

    [FindsBy(How = How.XPath, Using = "//td[@class='titleColumn']")]
    public IWebElement element { get; set; }

    [FindsBy(How = How.CssSelector, Using = "td[class='ratingColumn imdbRating']")]
    public IList<IWebElement> imdbRatingList { get; set; }

    [FindsBy(How = How.CssSelector, Using = "span[class='secondaryInfo']")]
    public IList<IWebElement> yearList { get; set; }

    [FindsBy(How = How.XPath, Using = "//div[contains(@class, 'article')]/h3")]
    public IWebElement title { get; set; }

    [FindsBy(How = How.XPath, Using = "//table[@class='chart full-width']/thead")]
    public IWebElement tableHeader { get; set; }

    [FindsBy(How = How.ClassName, Using = "lister-sort-by")]
    public IWebElement sortOptions { get; set; }




    public void FetchDetails(string baseUrl, string filePath)
    {
      _driver.Navigate().GoToUrl(baseUrl);

      DriverFactory.WaitForElementVisible(_driver, By.Id("amazon-affiliates"), 5);

      string[] title = new string[titleList.Count];
      string[] year = new string[yearList.Count];
      string[] imdbRating = new string[imdbRatingList.Count];
      string[] writeValue = new string[titleList.Count + 1];
      writeValue[0] = string.Format("|{0}|{1}|{2}|", "Movie", "IMDB Rating", "Year");

      for (int i = 0; i < titleList.Count; i++)
      {
        title[i] = titleList[i].Text;
        year[i] = yearList[i].Text;
        title[i] = title[i].Replace(year[i], string.Empty);//removing year value from title of the movie
        title[i] = Regex.Replace(title[i], @"^[\d.]*\s*", "", RegexOptions.Multiline);//removing numbers from movie title
        imdbRating[i] = imdbRatingList[i].Text;
        year[i] = Regex.Replace(year[i], @"[\(\)']+", "", RegexOptions.Multiline);//removing brackets() from year value       
        writeValue[i + 1] = string.Format("|{0}|{1}|{2}|", title[i], imdbRating[i], year[i]);
      }
      File.WriteAllLines(filePath, writeValue);
    }



    public string Title(string baseUrl)
    {
      _driver.Navigate().GoToUrl(baseUrl);

      DriverFactory.WaitForElementVisible(_driver, By.Id("amazon-affiliates"), 5);//code needs to be refactored

      string t = title.Text;
      return t;

    }

    public int MovieCount(string baseUrl)
    {
      _driver.Navigate().GoToUrl(baseUrl);

      DriverFactory.WaitForElementVisible(_driver, By.Id("amazon-affiliates"), 5);//code needs to be refactored

      int movieCount = titleList.Count;
      return movieCount;
    }

    public string TableHeaderList(string baseUrl)
    {
      _driver.Navigate().GoToUrl(baseUrl);

      DriverFactory.WaitForElementVisible(_driver, By.Id("amazon-affiliates"), 5);//code needs to be refactored

      string t = tableHeader.Text;
      return t;

    }

    public string SortOptions(string baseUrl, string selectBy)
    {
      _driver.Navigate().GoToUrl(baseUrl);

      DriverFactory.WaitForElementVisible(_driver, By.Id("amazon-affiliates"), 5);//code needs to be refactored

      SelectElement select = new SelectElement(sortOptions);

      select.SelectByText(selectBy);


      return element.Text;


    }

    public void HyperLink(string baseUrl)
    {
      _driver.Navigate().GoToUrl(baseUrl);

      DriverFactory.WaitForElementVisible(_driver, By.Id("amazon-affiliates"), 5);//code needs to be refactored
      IWebElement link = element.FindElement(By.LinkText("The Shawshank Redemption"));//code needs to be refactored
      link.Click();



    }
  }
}


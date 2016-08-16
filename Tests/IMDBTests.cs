using System;
using System.Globalization;
using System.Text;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using IMDBTests.SeleniumHelpers;
using IMDBTests.Utilities;
using Tests.PageObjects;

namespace IMDBTests
{
  [TestFixture]
  public class IMDBTests
  {
    private IWebDriver _driver;
    private StringBuilder _verificationErrors;
    private string _baseUrl;

    [SetUp]
    public void SetupTest()
    {
      _driver = new DriverFactory().Create();
      _baseUrl = ConfigurationHelper.Get<string>("TargetUrl");
      _verificationErrors = new StringBuilder();
    }

    [TearDown]
    public void TeardownTest()
    {
      try
      {
        _driver.Quit();
        _driver.Close();
      }
      catch (Exception)
      {
        // Ignore errors if we are unable to close the browser
      }
      _verificationErrors.ToString().Should().BeEmpty("No verification errors are expected.");
    }

    [Test]
    public void FetchMovieDetailsShouldSucceed()
    {
      // Variables 
      string filePath = ConfigurationHelper.filePath;

      //Act
      new Page(_driver).FetchDetails(_baseUrl, filePath);

      // Assert
      Assert.IsNotNull(filePath);
    }

    [Test]
    public void ValidateMovieCount()
    {
      // Arrange
      // Act
      int count = new Page(_driver).MovieCount(_baseUrl);

      // Assert
      Assert.AreEqual(250, count);
    }

    [Test]
    public void ValidatePageTitle()
    {
      // Arrange
      //Act
      string title = new Page(_driver).Title(_baseUrl);

      // Assert
      Assert.AreEqual("IMDb Charts", title);
    }

    [Test]
    public void Validate_PresenceofTableHeaders()
    {
      // Arrange
      //Act
      string title = new Page(_driver).TableHeaderList(_baseUrl);

      // Assert
      Assert.AreEqual("Rank & Title IMDb Rating Your Rating", title);

    }

    [Test]
    public void Validate_SortingOptions_Ranking()
    {
      // Arrange
      string imdbMovie = "1. The Shawshank Redemption (1994)";   //code needs to be refactored

      //Act
      string firstRank=new Page(_driver).SortOptions(_baseUrl,"Ranking");

      // Assert
      Assert.AreEqual(imdbMovie, firstRank);

    }

    [Test]
    public void ValidateHyperLink()
    {
      // Arrange
      //Act & Assert
      new Page(_driver).HyperLink(_baseUrl);


    }
  }
}



using AppiumCSharp.Pages;
using AppiumCSharp.Utils;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Service;
using System;
using System.IO;
using System.Threading;

namespace AppiumCSharp
{
    public class TestBase
    {
        private ThreadLocal<AppiumDriver<IWebElement>> driver = new ThreadLocal<AppiumDriver<IWebElement>>();
        private AppiumDriver<IWebElement> GetDriver() => driver.Value;
        
        AppiumLocalService appiumLocalService;
        PlatformCapabilities platformCapabilities = new PlatformCapabilities();
        AppiumOptions appiumOptions = new AppiumOptions();
        AppiumServiceBuilder appiumServiceBuilder = new AppiumServiceBuilder();
        Report report = new Report();
        ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        private readonly string url = Startup.ReadFromAppSettings("AppUrl");

        protected LoginPage loginPage;
        protected HomePage homePage;

        //BrowserStack Credentials
        private readonly static string username = "<your_browserstack_username>";
        private readonly static string accessKey = "<your_browserstack_accesskey>";
        private readonly Uri uri = new Uri($"https://{username}:{accessKey}@hub-cloud.browserstack.com/wd/hub");

        private enum PlatformType
        {
            Android,
            iOS,
            WebAndroid,
            WebIOS
        }

        private AppiumDriver<IWebElement> InitializeDriver(PlatformType platformType)
        {
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.NewCommandTimeout, 300);
            appiumOptions.AddAdditionalCapability("browserstack.user", username);
            appiumOptions.AddAdditionalCapability("browserstack.key", accessKey);
            switch (platformType)
            {
                case PlatformType.Android:
                    appiumOptions = platformCapabilities.InitNativeAndroidCapabilities();
                    driver.Value = new AndroidDriver<IWebElement>(uri, appiumOptions);
                    loginPage = new LoginPageNativeAndroid(driver);
                    homePage = new HomePageNative(driver);
                    break;
                case PlatformType.iOS:
                    appiumOptions = platformCapabilities.InitNativeIOSCapabilities();
                    driver.Value = new IOSDriver<IWebElement>(uri, appiumOptions);
                    loginPage = new LoginPageNativeIOS(driver);
                    homePage = new HomePageNative(driver);
                    break;
                case PlatformType.WebAndroid:
                    appiumOptions = platformCapabilities.InitWebAndroidCapabilities();
                    driver.Value = new AndroidDriver<IWebElement>(uri, appiumOptions);
                    loginPage = new LoginPageWeb(driver);
                    homePage = new HomePageWeb(driver);
                    NavigateToWebApp();
                    break;
                case PlatformType.WebIOS:
                    appiumOptions = platformCapabilities.InitWebIOSCapabilities();
                    driver.Value = new IOSDriver<IWebElement>(uri, appiumOptions);
                    loginPage = new LoginPageWeb(driver);
                    homePage = new HomePageWeb(driver);
                    NavigateToWebApp();
                    break;
                default:
                    throw new Exception("No platform selected!");
            }
            return GetDriver();
        }       


        private void StartAppiumServer()
        {
            string appiumJSPath = Environment.ExpandEnvironmentVariables($"%SystemDrive%/Users/%USERNAME%/AppData/Local/Programs/Appium/resources/app/node_modules/appium/build/lib/main.js");
            appiumServiceBuilder
                .WithAppiumJS(new FileInfo(appiumJSPath))
                .UsingDriverExecutable(new FileInfo(@"C:\Program Files\nodejs\node.exe"));
            appiumLocalService = appiumServiceBuilder.Build();
            if (!appiumLocalService.IsRunning)
                appiumLocalService.Start();
        }

        private void CloseAppiumServer()
        {
            if (appiumLocalService.IsRunning)
                appiumLocalService.Dispose();
        }

        private void SetUpDriver()
        {
            Enum.TryParse(Startup.ReadFromAppSettings("PlatformType"), out PlatformType platformType);
            driver.Value = InitializeDriver(platformType);
        }

        private void CloseDriver() => GetDriver()?.Quit();

        private void AttachScreenshotToTheReport()
        {
            try
            {
                if (GetDriver() != null)
                {
                    if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
                    {
                        var screenshot = GetDriver().GetScreenshot();
                        string screenshotPath = $"{TestContext.CurrentContext.Test.MethodName}.png";
                        screenshot.SaveAsFile(screenshotPath);
                        report.SaveScreenshotToReport(screenshotPath);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Snapshot was not taken" + e.Message);
            }
        }

        private void SetLandscapeOrientation()
        {
            GetDriver().Orientation = ScreenOrientation.Landscape;
            Report.test.Info($"Browser has switched to landscape orientation");
        }

        private void NavigateToWebApp()
        {
            SetLandscapeOrientation();
            GetDriver().Navigate().GoToUrl(url);
            Report.test.Info($"Browser navigated to {url}");
        }

        private IConfiguration BuildTestDataFile() => configurationBuilder.AddJsonFile(@"Utils\TestsData.json").Build();

        protected string GetData(string value) => BuildTestDataFile().GetSection(value).Value;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            StartAppiumServer();
            report.StartReport();
        }


        [OneTimeTearDown]
        public void OneTimeTearDown() => CloseAppiumServer();


        [SetUp]
        public void SetUp()
        {
            report.CreateTest();
            SetUpDriver();
        }


        [TearDown]
        public void TearDown()
        {
            AttachScreenshotToTheReport();
            report.LogTestStatus();
            CloseDriver();
        }
    }
}

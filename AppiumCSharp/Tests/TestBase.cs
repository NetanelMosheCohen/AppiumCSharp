using AppiumCSharp.Pages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Threading;

namespace AppiumCSharp
{
    public class TestBase
    {
        private ThreadLocal<AppiumDriver<IWebElement>> driver = new ThreadLocal<AppiumDriver<IWebElement>>();
        private AppiumDriver<IWebElement> GetDriver() => driver.Value;
        AppiumOptions appiumOptions = new AppiumOptions();
        AppiumLocalService appiumLocalService;
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
                    InitNativeAndroidCapabilities();
                    break;
                case PlatformType.iOS:
                    InitNativeIOSCapabilities();
                    break;
                case PlatformType.WebAndroid:
                    InitWebAndroidCapabilities();
                    break;
                case PlatformType.WebIOS:
                    InitWebIOSCapabilities();
                    break;
                default:
                    throw new Exception("No platform selected!");
            }
            return GetDriver();
        }

        private void InitNativeAndroidCapabilities()
        {
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.App, Startup.ReadFromAppSettings("App"));
            appiumOptions.AddAdditionalCapability("appWaitActivity", Startup.ReadFromAppSettings("AppActivity"));
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, Startup.ReadFromAppSettings("DeviceName"));
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, Startup.ReadFromAppSettings("OSVersion"));
            driver.Value = new AndroidDriver<IWebElement>(uri, appiumOptions);
            loginPage = new LoginPageNativeAndroid(driver);
            homePage = new HomePageNative(driver);
        }

        private void InitNativeIOSCapabilities()
        {
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, Startup.ReadFromAppSettings("PlatformName"));
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.AutomationName, Startup.ReadFromAppSettings("AutomationName"));
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, Startup.ReadFromAppSettings("PlatformVersion"));
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, Startup.ReadFromAppSettings("DeviceName"));
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.App, Startup.ReadFromAppSettings("App"));
            driver.Value = new IOSDriver<IWebElement>(uri, appiumOptions);
            loginPage = new LoginPageNativeIOS(driver);
            homePage = new HomePageNative(driver);
        }

        private void InitWebAndroidCapabilities()
        {
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.AutomationName, Startup.ReadFromAppSettings("AutomationName"));
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.BrowserName, Startup.ReadFromAppSettings("BrowserName"));
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, Startup.ReadFromAppSettings("DeviceName"));
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, Startup.ReadFromAppSettings("OSVersion"));
            appiumOptions.AddAdditionalCapability(ChromeOptions.Capability, JObject.Parse("{'w3c':false}")); //Required because of a bug in Appium C# client                   
            driver.Value = new AndroidDriver<IWebElement>(uri, appiumOptions);
            loginPage = new LoginPageWeb(driver);
            homePage = new HomePageWeb(driver);
            NavigateToWebApp();
        }

        private void InitWebIOSCapabilities()
        {
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.AutomationName, Startup.ReadFromAppSettings("AutomationName"));
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.BrowserName, Startup.ReadFromAppSettings("BrowserName"));
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, Startup.ReadFromAppSettings("PlatformName"));
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, Startup.ReadFromAppSettings("DeviceName"));
            driver.Value = new IOSDriver<IWebElement>(uri, appiumOptions);
            loginPage = new LoginPageWeb(driver);
            homePage = new HomePageWeb(driver);
            NavigateToWebApp();
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
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System.Threading;

namespace AppiumCSharp.Pages
{
    public class HomePageNative : HomePage
    {
        protected override By ProductItem => MobileBy.AccessibilityId("test-Item title");
    }
}

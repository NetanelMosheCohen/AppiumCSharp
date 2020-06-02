using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System.Threading;

namespace AppiumCSharp.Pages
{
    public class HomePageWeb : HomePage
    {
        public HomePageWeb(ThreadLocal<AppiumDriver<IWebElement>> driver) : base(driver)
        { }

        protected override By ProductItem => By.ClassName("inventory_item_name");
    }
}

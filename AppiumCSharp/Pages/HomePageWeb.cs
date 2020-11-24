using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System.Threading;

namespace AppiumCSharp.Pages
{
    public class HomePageWeb : HomePage
    {
        protected override By ProductItem => By.ClassName("inventory_item_name");
    }
}

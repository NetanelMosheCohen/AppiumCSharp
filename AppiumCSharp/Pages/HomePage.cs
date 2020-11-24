using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System.Threading;

namespace AppiumCSharp.Pages
{
    public abstract class HomePage : BasePage
    {
        protected abstract By ProductItem { get; }

        public void IsAddToCartButtonDisplayed() => WaitForElement(ProductItem);           
    }
}

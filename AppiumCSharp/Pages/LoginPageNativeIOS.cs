using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System.Threading;

namespace AppiumCSharp.Pages
{
    public class LoginPageNativeIOS : LoginPageNativeCommon
    {
        public LoginPageNativeIOS(ThreadLocal<AppiumDriver<IWebElement>> driver) : base(driver)
        { }

        private By SecretPassword => By.ClassName("XCUIElementTypeStaticText");

        public override bool IsPasswordDisplayed()
        {
            ScrollIOS(SwipeDirection.Down, SecretPassword);
            return IsElementAppears(SecretPassword);
        }
    }
}

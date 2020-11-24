using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System.Threading;

namespace AppiumCSharp.Pages
{
    public class LoginPageNativeAndroid : LoginPageNativeCommon
    {
        private By SecretPassword => By.XPath("//*[contains(@text, 'secret_sauce')]");

        public override bool IsPasswordDisplayed()
        {
            SwipeByCooridnates(SwipeDirection.Down, SecretPassword);
            return IsElementAppears(SecretPassword);
        }
    }
}

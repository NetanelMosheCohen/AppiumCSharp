using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System.Threading;

namespace AppiumCSharp.Pages
{
    public abstract class LoginPage : BasePage
    {
        protected abstract By UserNameField { get; }
        protected abstract By PasswordField { get; }
        protected abstract By LoginButton { get; }
        protected abstract By InvalidCredentialsErrorMessage { get; }


        public void Login(string userName, string password)
        {           
            SendKeysToElement(UserNameField, userName);
            SendKeysToElement(PasswordField, password);
            ClickOnElement(LoginButton);
        }

        public void IsInvalidCredentialsErrorMessageAppears() => WaitForElement(InvalidCredentialsErrorMessage);

        public abstract bool IsPasswordDisplayed();
    }
}

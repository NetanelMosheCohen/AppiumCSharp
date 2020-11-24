using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System.Threading;

namespace AppiumCSharp.Pages
{
    public class LoginPageWeb : LoginPage
    {
        protected override By UserNameField => By.Id("user-name");
        protected override By PasswordField => By.Id("password");
        protected override By LoginButton => By.ClassName("btn_action");
        protected override By InvalidCredentialsErrorMessage => By.ClassName("error-button");
        private By SecretPassword => By.ClassName("login_password");

        public override bool IsPasswordDisplayed() => IsElementAppears(SecretPassword);
    }
}

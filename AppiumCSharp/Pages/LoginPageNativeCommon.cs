using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System.Threading;

namespace AppiumCSharp.Pages
{
    public abstract class LoginPageNativeCommon : LoginPage
    {
        protected override By UserNameField => MobileBy.AccessibilityId("test-Username");
        protected override By PasswordField => MobileBy.AccessibilityId("test-Password");
        protected override By LoginButton => MobileBy.AccessibilityId("test-LOGIN");
        protected override By InvalidCredentialsErrorMessage => MobileBy.AccessibilityId("test-Error message");
    }
}

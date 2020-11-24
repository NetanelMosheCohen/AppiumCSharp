using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AppiumCSharp
{
    public class BasePage
    {
        public static ThreadLocal<AppiumDriver<IWebElement>> driver = new ThreadLocal<AppiumDriver<IWebElement>>();
        public static AppiumDriver<IWebElement> GetDriver() => driver.Value;

        public void ClickOnElement(By element)
        {
            WaitForElement(element);
            try
            {
                GetDriver().FindElement(element).Click();
                Report.test.Info($"Clicked on element {element}");
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to click on the element: {element}. {e.Message}");
            }

        }

        public void SendKeysToElement(By element, string text)
        {
            WaitForElement(element);
            try
            {
                GetDriver().FindElement(element).SendKeys(text);
                Report.test.Info($"Sent keys to element {element}");
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to send keys to the element: {element}. {e.Message}");
            }

        }

        public enum SwipeDirection
        {
            Up,
            Down,
            Left,
            Right
        }

        //This function is useful in case that the cross platform scroll function (SwipeByCooridnates) is not working for iOS device
        public void ScrollIOS(SwipeDirection swipeDirection, By element)
        {
            int scrollAttempts = 3;
            Dictionary<string, object> args = new Dictionary<string, object>
            {
                { "direction", swipeDirection.ToString().ToLower() }
            };
            try
            {
                for (var i = 0; i < scrollAttempts; i++)
                {
                    if (!IsElementAppears(element))
                        GetDriver().ExecuteScript("mobile: scroll", args);
                }
                Report.test.Info($"iOS scroll {swipeDirection} to element {element}");
            }

            catch (Exception e)
            {
                throw new Exception($"iOS scroll {swipeDirection} to element failed: {e.Message}");
            }
        }

        public void SwipeByCooridnates(SwipeDirection swipeDirection, By element)
        {
            int scrollAttempts = 3;
            int windowWidth = GetDriver().Manage().Window.Size.Width;
            int windowHeight = GetDriver().Manage().Window.Size.Height;
            int startX, endX, startY, endY;
            startX = startY = endX = endY = 0;

            //Checking which direction is given
            switch (swipeDirection)
            {
                case SwipeDirection.Up:
                    startX = endX = windowWidth / 2;
                    startY = (int)(windowHeight * 0.2);
                    endY = (int)(windowHeight * 0.8);
                    break;
                case SwipeDirection.Down:
                    startX = endX = windowWidth / 2;
                    startY = (int)(windowHeight * 0.8);
                    endY = (int)(windowHeight * 0.2);
                    break;
                case SwipeDirection.Left:
                    startY = endY = windowHeight / 2;
                    startX = (int)(windowHeight * 0.8);
                    endX = (int)(windowHeight * 0.2);
                    break;
                case SwipeDirection.Right:
                    startY = endY = windowHeight / 2;
                    startX = (int)(windowHeight * 0.2);
                    endX = (int)(windowHeight * 0.8);
                    break;
            }
            try
            {
                for (var i = 0; i < scrollAttempts; i++)
                {
                    if (!IsElementAppears(element))
                        new TouchAction(GetDriver())
                            .Press(startX, startY)
                            .Wait(300)
                            .MoveTo(endX, endY)
                            .Release()
                            .Perform();
                }
                Report.test.Info($"Swiped by coordinates to element {element}");
            }

            catch (Exception e)
            {
                throw new Exception($"Swipe by coordinated failed: {e.Message}");
            }
        }

        public bool IsElementAppears(By element)
        {
            try
            {
                GetDriver().FindElement(element);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void WaitForElement(By element)
        {
            try
            {
                new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(10)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(element));
            }
            catch (Exception e)
            {
                throw new Exception($"Wait For Element {element} Failed: {e.Message}");
            }
        }

        public void SwitchToNativeContext()
        {
            try
            {
                GetDriver().Context = "NATIVE_APP";
                Report.test.Info("Context has switched to native app");
            }
            catch (Exception e)
            {
                throw new Exception($"Switching to native view failed: {e.Message}");
            }

        }

        public void SwitchToWebViewContext(string requiredContext)
        {
            try
            {
                foreach (var context in GetDriver().Contexts)
                {
                    if (context.Contains(requiredContext))
                        GetDriver().Context = context;
                }
                Report.test.Info($"Context has switched to webview {requiredContext}");
            }
            catch (Exception e)
            {
                throw new Exception($"Switching to web view failed: {e.Message}");
            }
        }
    }
}

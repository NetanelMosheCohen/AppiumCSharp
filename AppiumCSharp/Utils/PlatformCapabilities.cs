using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Chrome;

namespace AppiumCSharp.Utils
{
    public class PlatformCapabilities
    {
        AppiumOptions appiumOptions = new AppiumOptions();

        public AppiumOptions InitNativeAndroidCapabilities()
        {
            lock (appiumOptions)
            {
                appiumOptions.AddAdditionalCapability(MobileCapabilityType.App, Startup.ReadFromAppSettings("App"));
                appiumOptions.AddAdditionalCapability("appWaitActivity", Startup.ReadFromAppSettings("AppActivity"));
                appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, Startup.ReadFromAppSettings("DeviceName"));
                appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, Startup.ReadFromAppSettings("OSVersion"));
            }            
            return appiumOptions;
        }

        public AppiumOptions InitNativeIOSCapabilities()
        {
            lock (appiumOptions)
            {
                appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, Startup.ReadFromAppSettings("PlatformName"));
                appiumOptions.AddAdditionalCapability(MobileCapabilityType.AutomationName, Startup.ReadFromAppSettings("AutomationName"));
                appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, Startup.ReadFromAppSettings("PlatformVersion"));
                appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, Startup.ReadFromAppSettings("DeviceName"));
                appiumOptions.AddAdditionalCapability(MobileCapabilityType.App, Startup.ReadFromAppSettings("App"));
            }               
            return appiumOptions;
        }

        public AppiumOptions InitWebAndroidCapabilities()
        {
            lock (appiumOptions)
            {
                appiumOptions.AddAdditionalCapability(MobileCapabilityType.AutomationName, Startup.ReadFromAppSettings("AutomationName"));
                appiumOptions.AddAdditionalCapability(MobileCapabilityType.BrowserName, Startup.ReadFromAppSettings("BrowserName"));
                appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, Startup.ReadFromAppSettings("DeviceName"));
                appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, Startup.ReadFromAppSettings("OSVersion"));
                appiumOptions.AddAdditionalCapability(ChromeOptions.Capability, JObject.Parse("{'w3c':false}")); //Required because of a bug in Appium C# client                   
            }                
            return appiumOptions;
        }

        public AppiumOptions InitWebIOSCapabilities()
        {
            lock (appiumOptions)
            {
                appiumOptions.AddAdditionalCapability(MobileCapabilityType.AutomationName, Startup.ReadFromAppSettings("AutomationName"));
                appiumOptions.AddAdditionalCapability(MobileCapabilityType.BrowserName, Startup.ReadFromAppSettings("BrowserName"));
                appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, Startup.ReadFromAppSettings("PlatformName"));
                appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, Startup.ReadFromAppSettings("DeviceName"));
            }                
            return appiumOptions;
        }
    }
}

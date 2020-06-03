# Appium mobile test automation framework using C# + .NET Core 3.1 + NUnit.

## **The framework contains:**

- Cross-platform support for running the tests on 4 platforms - Native Android app, native iOS app, Android web app and iOS web app
- Abstraction layer to run a single test without code changes on all the 4 platforms!
- Platform parameterization using config files
- Page Object Model design
- Tests run report and dashboard using Extent Reports library (including screenshots, logging test steps)
- Parallel execution on real devices using BrowserStack Cloud
- Reading tests data from external file (JSON)
- Starting Appium server programmatically


## **The tests can be run via CMD by using the following .NET Core command:**

**For native Android app:**
`($env:ASPNETCORE_ENVIRONMENT="Android") | dotnet test -v n`

**For native iOS app:**
`($env:ASPNETCORE_ENVIRONMENT="iOS") | dotnet test -v n`

**For Android web app:**
`($env:ASPNETCORE_ENVIRONMENT="WebAndroid") | dotnet test -v n`

**For iOS web app:**
`($env:ASPNETCORE_ENVIRONMENT="WebIOS") | dotnet test -v n`



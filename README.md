# Appium mobile test automation framework using C# + .NET Core 3.1 + NUnit.

## **The framework contains:**

- Cross-platform support for running tests on 4 different platforms:
    - Native Android app
    - Native iOS app
    - Android web app
    - iOS web app
- Abstraction layers to be able to run a single test without code changes on all 4 platforms!
- Platform parameterization using config files
- Page Object Model design
- Tests run report and dashboard using Extent Reports library (including errors, stack trace, screenshots, logging test steps, etc.)
- Parallel execution on real mobile devices using BrowserStack Cloud
- Reading tests data from external file (JSON)
- Managing Appium server programmatically
- Support for running the project on CI/CD tools such as Jenkins (demonstrated in the video below)


## **The tests can be run via CMD by using the following .NET Core command:**

**For native Android app:**
`($env:ASPNETCORE_ENVIRONMENT="Android") | dotnet test -v n`

**For native iOS app:**
`($env:ASPNETCORE_ENVIRONMENT="iOS") | dotnet test -v n`

**For Android web app:**
`($env:ASPNETCORE_ENVIRONMENT="WebAndroid") | dotnet test -v n`

**For iOS web app:**
`($env:ASPNETCORE_ENVIRONMENT="WebIOS") | dotnet test -v n`

## **Some screenshots from the project:**
A quick demo of the project:  
https://www.youtube.com/watch?v=cuAlgJhxt_Q


## **Some screenshots from the project:**

![alt text](https://github.com/NetanelMosheCohen/AppiumCSharp/blob/master/Demo.PNG?raw=true)
![alt text](https://github.com/NetanelMosheCohen/AppiumCSharp/blob/master/SuccessfullRun.PNG?raw=true)
![alt text](https://github.com/NetanelMosheCohen/AppiumCSharp/blob/master/FailedRun.PNG?raw=true)
![alt text](https://github.com/NetanelMosheCohen/AppiumCSharp/blob/master/ReportSnapshot.PNG?raw=true)




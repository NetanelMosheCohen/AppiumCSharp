using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using ExtentReport = AventStack.ExtentReports.ExtentReports;


namespace AppiumCSharp
{
    public class Report
    {
        [ThreadStatic]
        public static ExtentTest test;
        ExtentReport extent = new ExtentReport();
        
        public void StartReport()
        {
            string dir = TestContext.CurrentContext.TestDirectory + "\\";
            string fileName = GetType().ToString() + ".html";
            var htmlReporter = new ExtentHtmlReporter(dir + fileName);
            extent.AttachReporter(htmlReporter);
        }

        private void SaveTestDataToTheReport() => extent.Flush();

        public void CreateTest() => test = extent.CreateTest(TestContext.CurrentContext.Test.Name);

        public void LogTestStatus()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var message = TestContext.CurrentContext.Result.Message;
            string stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                    ? string.Empty
                    : string.Format("{0}", TestContext.CurrentContext.Result.StackTrace);
            Status logStatus = status switch
            {
                TestStatus.Failed => Status.Fail,
                TestStatus.Inconclusive => Status.Warning,
                TestStatus.Skipped => Status.Skip,
                _ => Status.Pass,
            };
            test.Log(logStatus, $"Test status: {logStatus}<br> {message}<br><br> {stacktrace}");
            SaveTestDataToTheReport();
        }

        public void SaveScreenshotToReport(string snapshotPath) => test.AddScreenCaptureFromPath(snapshotPath);
    }
}

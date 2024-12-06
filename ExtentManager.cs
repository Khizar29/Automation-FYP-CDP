using ExtentReports;
using System;
using System.IO;

namespace AutomationFYPCDP
{
    public class ExtentManager
    {
        private static ExtentReports extent;
        private static ExtentTest test;
        private static string reportDirectory = @"C:\Reports";  // Modify this path if needed

        // Create ExtentReports instance and configure the report settings
        public static ExtentReports GetExtent()
        {
            if (extent == null)
            {
                var htmlReporter = new ExtentHtmlReporter(Path.Combine(reportDirectory, "ExtentReport.html"));
                htmlReporter.Config.Theme = Theme.Standard;
                htmlReporter.Config.DocumentTitle = "Test Report";
                htmlReporter.Config.ReportName = "Automation Test Results";

                extent = new ExtentReports();
                extent.AttachReporter(htmlReporter);
            }
            return extent;
        }

        // Create test log instance
        public static ExtentTest StartTest(string testName, string description)
        {
            test = extent.CreateTest(testName, description);
            return test;
        }

        // Flush the report data to file
        public static void EndReport()
        {
            extent.Flush();
        }

        // Capture screenshot if needed
        public static void AddScreenshot(string screenshotPath)
        {
            test.AddScreenCaptureFromPath(screenshotPath);
        }
    }
}

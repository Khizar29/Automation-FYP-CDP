using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationFYPCDP
{
    internal class CorePage
    {
        public static IWebDriver driver;
        public static ExtentReports extentReports;
        public static ExtentTest Test;
        public static ExtentTest Step;

        public static void SeleniumInit(string Browser)
        {
            if (Browser == "Chrome")
            {
                var options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                options.AddArgument("--incognito");
                IWebDriver ChromeDriver = new ChromeDriver(options);
                driver = ChromeDriver;
            }
            //else if (Browser == "Firefoz")
            //{ 

            //}

        }
        public static void CreateReport(String path)
        {
            extentReports = new ExtentReports();
            var SparkReporter = new ExtentSparkReporter(path);
            extentReports.AttachReporter(SparkReporter);
        }
       
        public static void TakeScreenshot(Status status, string stepDetail)
        {
            string path = @"C:\ExtentReports\Images\" +DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            Screenshot screenshot =((ITakesScreenshot)driver).GetScreenshot();
            File.WriteAllBytes(path, screenshot.AsByteArray);
            Step.Log(status, stepDetail, MediaEntityBuilder.CreateScreenCaptureFromPath(path).Build());
        }

    }
}

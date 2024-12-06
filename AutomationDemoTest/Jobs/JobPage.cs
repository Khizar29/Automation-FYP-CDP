using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AutomationFYPCDP
{
    internal class JobPage : CorePage
    {
        #region Locators
        By loginnav= By.XPath("//*[@id=\"root\"]/div/header/div[1]/nav/div[3]/span");
        By emailtxt = By.Id("email");
        By passwordxt = By.Id("password");
        By signinbtn = By.XPath("//*[@id=\"root\"]/div/header/div[3]/div/main/div/form/button");
        


        #endregion

        #region Locators
        By jobnav = By.Id("jobs");
        By searchtxt = By.XPath("//*[@id=\"root\"]/div/main/div/div[1]/div[1]/input");
        By jobtypeselect = By.XPath("//*[@id=\"root\"]/div/main/div/div[1]/div[2]/div[1]/select");

        By remote = By.XPath("//*[@id=\"root\"]/div/main/div/div[1]/div[2]/div[1]/select/option[2]");
        By onsite = By.XPath("//*[@id=\"root\"]/div/main/div/div[1]/div[2]/div[1]/select/option[3]");
        By hybrid = By.XPath("//*[@id=\"root\"]/div/main/div/div[1]/div[2]/div[1]/select/option[4]");
        By internship = By.XPath("//*[@id=\"root\"]/div/main/div/div[1]/div[2]/div[1]/select/option[5]");
        //By signinbtn = By.XPath("//*[@id=\"root\"]/div/header/div[3]/div/main/div/form/button");
        //By signinbtn = By.XPath("//*[@id=\"root\"]/div/header/div[3]/div/main/div/form/button");

        #endregion

        public void NavigateToJobs()
        {
            Step = Test.CreateNode("JobPage");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                // Wait for the overlay to either disappear or become non-blocking
                wait.Until(drv =>
                {
                    var overlay = drv.FindElements(By.XPath("//div[contains(@class, 'fixed inset-0')]"));
                    if (overlay.Count == 0)
                    {
                        // Overlay is not present in the DOM
                        return true;
                    }

                    // Check if the overlay is not displayed or its `pointer-events` is set to none
                    var style = overlay[0].GetCssValue("pointer-events");
                    return !overlay[0].Displayed || style == "none";
                });
                Console.WriteLine("Blocking overlay is no longer visible or active.");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("No blocking overlay detected, proceeding...");
            }

            // Click on the 'Jobs' navigation link
            IWebElement jobsNav = driver.FindElement(jobnav);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", jobsNav);
            jobsNav.Click();
            CorePage.TakeScreenshot(Status.Pass, "Job Page navigated");
            Console.WriteLine("Navigated to the Jobs page.");
        }



        public void FilterJobsByType(string jobType)
        {

            // Open the job type dropdown
            driver.FindElement(jobtypeselect).Click();
            try
            {
                // Dynamically build the XPath for the job type and make it case-insensitive
                string jobTypeXpath = $"//*[@id=\"root\"]/div/main/div/div[1]/div[2]/div[1]/select/option[translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = '{jobType.ToLower()}']";

                // Locate and click the specified job type
                var jobTypeElement = driver.FindElement(By.XPath(jobTypeXpath));
                jobTypeElement.Click();
                Console.WriteLine($"Selected '{jobType}' job type from the dropdown.");
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"Job type '{jobType}' not found in the dropdown. Check the provided input or HTML structure.");
                throw;
            }

            // Wait for job cards to appear
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            try
            {
                wait.Until(drv => drv.FindElements(By.XPath("//*[@id=\"root\"]/div/main/div/div[2]/div/div/div")).Count > 0);
                CorePage.TakeScreenshot(Status.Pass, "Job filtered Successfully");
                Console.WriteLine($"{jobType} jobs displayed successfully.");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"No jobs displayed for the selected '{jobType}' filter.");
                throw;
            }
        }




    }
}

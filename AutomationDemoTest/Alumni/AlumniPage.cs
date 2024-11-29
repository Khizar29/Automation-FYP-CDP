using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
namespace AutomationFYPCDP
{
    internal class AlumniPage : CorePage
    {
        #region Locators
        By alumniNav = By.Id("alumni");
        By searchBar = By.Id("search_alumni");
        By searchBtn = By.Id("search_btn");
        By profileCard = By.XPath("//*[@id=\"root\"]/div/main/div/div[3]/div[1]/div/div");
        By jobtypeselect = By.XPath("//*[@id=\"root\"]/div/main/div/div[1]/div[2]/div[1]/select");
        By remote = By.XPath("//*[@id=\"root\"]/div/main/div/div[1]/div[2]/div[1]/select/option[2]");
        By onsite = By.XPath("//*[@id=\"root\"]/div/main/div/div[1]/div[2]/div[1]/select/option[3]");
        By hybrid = By.XPath("//*[@id=\"root\"]/div/main/div/div[1]/div[2]/div[1]/select/option[4]");
        By internship = By.XPath("//*[@id=\"root\"]/div/main/div/div[1]/div[2]/div[1]/select/option[5]");
        //By signinbtn = By.XPath("//*[@id=\"root\"]/div/header/div[3]/div/main/div/form/button");
        //By signinbtn = By.XPath("//*[@id=\"root\"]/div/header/div[3]/div/main/div/form/button");

        #endregion

        public void NavigateToAlumnis()
        {
            // Click on the Alumni navigation link
            driver.FindElement(alumniNav).Click();
            Console.WriteLine("Navigated to the Alumni page.");
        }

        public void SearchAlumni(string firstname) 
        { 
            // Wait for the search bar to be visible
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement searchBarElement = wait.Until(drv => drv.FindElement(searchBar));
            searchBarElement.SendKeys(firstname);
            driver.FindElement(searchBtn).Click();

            // Wait for the profile cards to be visible after search
            IWebElement profileCardElement = wait.Until(drv => drv.FindElement(profileCard));
            profileCardElement.Click();

            Console.WriteLine("Navigated to the profile card of the alumni.");
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
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            try
            {
                wait.Until(drv => drv.FindElements(By.XPath("//*[@id=\"root\"]/div/main/div/div[2]/div/div/div")).Count > 0);
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

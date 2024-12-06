using AventStack.ExtentReports;
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
            Step = Test.CreateNode("AlumniPage");
            // Click on the Alumni navigation link
            driver.FindElement(alumniNav).Click();
            CorePage.TakeScreenshot(Status.Pass, "Alumni Directory entered");
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
            IWebElement profile = wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"root\"]/div/header[2]/div[2]/button[1]")));

            Console.WriteLine("Navigated to the profile card of the alumni.");
            CorePage.TakeScreenshot(Status.Pass, "Alumni Card displayed");
        }

    }
}

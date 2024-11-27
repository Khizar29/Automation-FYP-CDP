﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AutomationDemoTest
{
    internal class LoginPage : CorePage
    {
        #region Locators
        By loginnav = By.XPath("//*[@id=\"root\"]/div/header/div[1]/nav/div[3]/span");
        By emailtxt = By.Id("email");
        By passwordxt = By.Id("password");
        By signinbtn = By.XPath("//*[@id=\"root\"]/div/header/div[3]/div/main/div/form/button");

        #endregion

        public void Login(string url, string username, string password)
        {
            // Navigate to the URL
            driver.Url = url;

            // Click on the login navigation link
            driver.FindElement(loginnav).Click();

            // Fill in the login form
            driver.FindElement(emailtxt).SendKeys(username);
            driver.FindElement(passwordxt).SendKeys(password);

            // Click on the Sign In button
            driver.FindElement(signinbtn).Click();

            // Wait for the login modal to disappear or the "Logout" button to appear
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            try
            {
                // Wait until the modal is no longer visible
                wait.Until(drv =>
                {
                    var modal = drv.FindElements(By.XPath("//div[contains(@class, 'modal')]")); // Adjust modal locator if needed
                    return modal.Count == 0 || !modal[0].Displayed;
                });

                // Alternatively, check for the presence of a "Logout" button or other post-login element
                wait.Until(drv => drv.FindElement(By.XPath("//*[@id='home']/div[3]/div/button[text()='Logout']")).Displayed);

                Console.WriteLine("Login successful, modal closed.");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Login modal did not close within the expected time.");
                throw;
            }
        }

        public void LoginInvalid(string url, string username, string password)
        {
            // Navigate to the URL
            driver.Url = url;

            driver.FindElement(loginnav).Click();

            driver.FindElement(emailtxt).SendKeys(username);
            driver.FindElement(passwordxt).SendKeys(password);

            driver.FindElement(signinbtn).Click();
        }
    }
}

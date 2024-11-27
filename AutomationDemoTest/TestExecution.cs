
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Configuration;
using System.Drawing;

namespace AutomationDemoTest
{
    [TestClass]
    public class TestExecution
    {
        #region Setup and CLeanups

        public TestContext instance;

        public TestContext TestContext
        {
            set { instance = value; }
            get { return instance; }
        }

        [ClassInitialize()]

        public static void ClassInit(TestContext context)
        {

        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {

        }

        [TestInitialize()]
        public void TestInit()
        {

            CorePage.SeleniumInit(ConfigurationManager.AppSettings["Browser"].ToString());
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            CorePage.driver.Close();
        }
        
        #endregion 

        LoginPage LoginPage = new LoginPage();
        JobPage job = new JobPage();
        ManageJobs ManageJobs = new ManageJobs();

        [TestMethod]
        public void TestcasewithValidusernamevalidpassword()
        {
           
            LoginPage.Login("http://localhost:3000/", "s.khizarali03@gmail.com", "abcd12345");
            //String welcome = CorePage.driver.FindElement(By.XPath("//*[@id=\"home\"]/div[3]/div/button")).Text;

            // Use WebDriverWait to wait until the button text changes to "Logout"
            WebDriverWait wait = new WebDriverWait(CorePage.driver, TimeSpan.FromSeconds(10));
            IWebElement button = wait.Until(drv => drv.FindElement(By.XPath("//*[@id='home']/div[3]/div/button")));

            // Wait until the button text is "Logout"
            wait.Until(drv => button.Text == "Logout");

            // Retrieve the button text
            String welcome = button.Text;

            // Assert the button text
            Assert.AreEqual("Logout", welcome, "Button text did not update to 'Logout' after login.");



        }

        [TestMethod]
        //DataSource("Microsoft.VisualStudio.TestTool.DataSource.XML',"Data.xml", "TestcasewithiNValidusernameinvalidpassword", "DataAccessMethodSequential)
        public void TestcasewithiNValidusernameinvalidpassword()
        {
            //string url = TestContext.DataRow["url"].ToString();
            LoginPage.LoginInvalid("http://localhost:3000/", "s.khizarali03@gmail.com", "abcd2345");

            // Wait for the error message to appear
            WebDriverWait wait = new WebDriverWait(CorePage.driver, TimeSpan.FromSeconds(10));
            IWebElement errorAlert = wait.Until(drv => drv.FindElement(By.XPath("//div[contains(@class, 'MuiAlert-message')]")));

            // Verify the error message text
            string actualErrorMessage = errorAlert.Text;
            Assert.AreEqual("The password you entered is incorrect.", actualErrorMessage, "Error message does not match.");

            //String error = CorePage.driver.FindElement(By.XPath("//*[@id=\"root\"]/div/header/div[3]/div/main/div/form/div[3]/div/div[2]")).Text;
            //Assert.AreEqual("The password you entered is incorrect.", error);
            
        }

        [TestMethod]

        public void searchJob_TC001()
        {
            Console.WriteLine("Starting searchJob_TC001 test...");
            LoginPage.Login("http://localhost:3000/", "s.khizarali03@gmail.com", "abcd12345");

            Console.WriteLine("Navigating to the Jobs page...");
            job.NavigateToJobs();

            Console.WriteLine("Filtering jobs by 'Remote'...");
            job.FilterJobsByType("remote");

            // Validate job cards are displayed and contain "Remote" text
            WebDriverWait wait = new WebDriverWait(CorePage.driver, TimeSpan.FromSeconds(20)); 
            try
            {
                Console.WriteLine("Waiting for job cards...");
                var jobCards = wait.Until(drv => drv.FindElements(By.XPath("//div[contains(@class, 'grid')]/div[contains(@class, 'bg-blue-100')]")));
                Assert.IsTrue(jobCards.Count > 0, "No job cards were displayed for the 'Remote' filter.");
                Console.WriteLine($"Found {jobCards.Count} job card(s). Validating content...");
                var jobTypeText = CorePage.driver.FindElement(By.Id("jobtype")).Text;
                Assert.AreEqual("Remote", jobTypeText, $"Job card does not contain the expected text: 'Remote'. Actual text: '{jobTypeText}'");
                Console.WriteLine("All job cards validated successfully and contain 'Remote'.");
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail("Job cards did not load within the specified wait time.");
            }
            Console.WriteLine("Test case 'searchJob_TC001' completed successfully!");
        }


        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML","Data.xml", "AdminJobPost_TC001", DataAccessMethod.Sequential)]
        public void AdminJobPost_TC001()
        {
            string url = TestContext.DataRow["url"].ToString();
            string username = TestContext.DataRow["username"].ToString();
            string password  = TestContext.DataRow["password"].ToString();
            string company = TestContext.DataRow["company"].ToString();
            string title = TestContext.DataRow["title"].ToString();
            string jobtype = TestContext.DataRow["jobtype"].ToString();
            string requirements = TestContext.DataRow["requirements"].ToString();
            string description = TestContext.DataRow["description"].ToString();
            string responsibilites = TestContext.DataRow["responsibilites"].ToString();
            string joblink = TestContext.DataRow["joblink"].ToString();


            LoginPage.Login(url, username, password);
            ManageJobs.navigatetomanagejobs();
            ManageJobs.postjob(company, title, jobtype, requirements, description, responsibilites, joblink);

            WebDriverWait wait = new WebDriverWait(CorePage.driver, TimeSpan.FromSeconds(10));
            try
            {
                // Wait for the alert to appear by checking if it's present in the DOM
                wait.Until(driver => driver.SwitchTo().Alert() != null);
                // Get the alert text
                IAlert alert = CorePage.driver.SwitchTo().Alert();
                string alertText = alert.Text;
                Assert.AreEqual("Job Posted Successfully", alertText, "The alert message was not as expected.");
                alert.Accept();
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Alert did not appear.");
                Assert.Fail("Alert did not appear. Job may not have been posted successfully.");
            }

        }



    }
}

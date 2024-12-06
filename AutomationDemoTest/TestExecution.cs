
using AventStack.ExtentReports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Configuration;
using System.Drawing;


namespace AutomationFYPCDP
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
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            string ResultFile = @"C:\ExtentReports\TestExecLog_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".html";
            CorePage.CreateReport(ResultFile);
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            CorePage.extentReports.Flush();
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
            CorePage.driver.Url = ConfigurationManager.AppSettings["Url"].ToString();
            CorePage.Test = CorePage.extentReports.CreateTest(TestContext.TestName);

        }

        [TestCleanup()]
        public void TestCleanup()
        {
            CorePage.driver.Close();
        }
        
        
        #endregion 

        LoginPage LoginPage = new LoginPage();
        JobPage job = new JobPage();
        AlumniPage alumni = new AlumniPage();
        ManageJobs ManageJobs = new ManageJobs();


        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "Data.xml", "TestcasewithValidusernamevalidpassword", DataAccessMethod.Sequential)]
        public void TestcasewithValidusernamevalidpassword()
        {
            string username = TestContext.DataRow["username"].ToString();
            string password = TestContext.DataRow["password"].ToString();
            LoginPage.Login(username, password);
            // Use WebDriverWait to wait until the button text changes to "Logout"
            WebDriverWait wait = new WebDriverWait(CorePage.driver, TimeSpan.FromSeconds(10));
            IWebElement button = wait.Until(drv => drv.FindElement(By.XPath("//*[@id='home']/div[3]/div/button")));

            // Wait until the button text is "Logout"
            wait.Until(drv => button.Text == "Logout");
            String welcome = button.Text;

            Assert.AreEqual("Logout", welcome, "Button text did not update to 'Logout' after login.");
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "Data.xml", "Login_TC002", DataAccessMethod.Sequential)]
        public void Login_TC002()
        {
            string username = TestContext.DataRow["username"].ToString();
            string password = TestContext.DataRow["password"].ToString();
            LoginPage.LoginInvalid(username, password);

            // Wait for the error message to appear
            WebDriverWait wait = new WebDriverWait(CorePage.driver, TimeSpan.FromSeconds(20));
            IWebElement errorAlert = wait.Until(drv => drv.FindElement(By.XPath("//div[contains(@class, 'MuiAlert-message')]")));

            // Verify the error message text
            string actualErrorMessage = errorAlert.Text;

            Assert.AreEqual("The password you entered is incorrect.", actualErrorMessage, "Error message does not match.");
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "Data.xml", "searchJob_TC001", DataAccessMethod.Sequential)]
        public void searchJob_TC001()
        {
            Console.WriteLine("Starting searchJob_TC001 test...");
            string username = TestContext.DataRow["username"].ToString();
            string password = TestContext.DataRow["password"].ToString();
            string jobType = TestContext.DataRow["jobtype"].ToString();
            LoginPage.Login(username, password);   
            job.NavigateToJobs();
            job.FilterJobsByType(jobType);

            // Validate job cards are displayed and contain "Remote" text
            WebDriverWait wait = new WebDriverWait(CorePage.driver, TimeSpan.FromSeconds(30)); 
            try
            {
                Console.WriteLine("Waiting for job cards...");
                var jobCards = wait.Until(drv => drv.FindElements(By.XPath("//div[contains(@class, 'grid')]/div[contains(@class, 'bg-blue-100')]")));
                Assert.IsTrue(jobCards.Count > 0, "No job cards were displayed for the filter.");
                Console.WriteLine($"Found {jobCards.Count} job card(s). Validating content...");
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
            string username = TestContext.DataRow["username"].ToString();
            string password  = TestContext.DataRow["password"].ToString();
            string company = TestContext.DataRow["company"].ToString();
            string title = TestContext.DataRow["title"].ToString();
            string jobtype = TestContext.DataRow["jobtype"].ToString();
            string requirements = TestContext.DataRow["requirements"].ToString();
            string description = TestContext.DataRow["description"].ToString();
            string responsibilites = TestContext.DataRow["responsibilites"].ToString();
            string joblink = TestContext.DataRow["joblink"].ToString();

            LoginPage.Login(username, password);
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


        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "Data.xml", "AdminJobPost_TC002", DataAccessMethod.Sequential)]
        public void AdminJobPost_TC002() //empty fields
        {
            string username = TestContext.DataRow["username"].ToString();
            string password = TestContext.DataRow["password"].ToString();
            string company = TestContext.DataRow["company"].ToString();
            string title = TestContext.DataRow["title"].ToString();
            string jobtype = TestContext.DataRow["jobtype"].ToString();
            string requirements = TestContext.DataRow["requirements"].ToString();
            string description = TestContext.DataRow["description"].ToString();
            string responsibilites = TestContext.DataRow["responsibilites"].ToString();
            string joblink = TestContext.DataRow["joblink"].ToString();
               
            LoginPage.Login(username, password);
            ManageJobs.navigatetomanagejobs();
            ManageJobs.postjob(company, title, jobtype, requirements, description, responsibilites, joblink);


            WebDriverWait wait = new WebDriverWait(CorePage.driver, TimeSpan.FromSeconds(2));
         
            try
                {
                    
                    IAlert alert = wait.Until(driver => driver.SwitchTo().Alert());
                    // If alert appears, fail the test because no alert should appear for empty fields
                    Assert.Fail("Unexpected alert appeared: " + alert.Text);
                }
                catch (WebDriverTimeoutException)
                {
                    // No alert appeared, which is expected, so the test passes
                    Console.WriteLine("No alert appeared as expected.");
                }
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "Data.xml", "Alumni_TC001", DataAccessMethod.Sequential)]
        public void Alumni_TC001()
        {
            string firstName = TestContext.DataRow["firstname"].ToString();

            Console.WriteLine("Navigating to the Alumni page...");
            alumni.NavigateToAlumnis();
            alumni.SearchAlumni(firstName);
        }

    }
}

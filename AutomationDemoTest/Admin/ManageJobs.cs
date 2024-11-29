using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AutomationFYPCDP
{
    internal class ManageJobs : CorePage
    {
        #region Locators
        By adminDashboardbtn= By.XPath("//*[@id=\"home\"]/div[3]/div/a");
        By adminJobsidebar= By.Id("adminjobs");
        By newJob = By.Id("NewJob");
        By companyName = By.Id("company_name");
        By jobTitle = By.Id("title");
        By jobType = By.Id("job_type");
        By onsite = By.Id("Onsite");
        By remote = By.Id("Remote");
        By hybrid = By.Id("Hybrid");
        By internship = By.Id("Internship");

        By jobReq = By.Id("qualification_req");
        By jobDesc = By.Id("job_description");
        By jobResp = By.Id("responsibilities");
        By jobLink = By.Id("link");
        By submitBtn = By.Id("submit_job");


        #endregion

        public void navigatetomanagejobs()
        {
            driver.FindElement(adminDashboardbtn).Click();
            driver.FindElement(adminJobsidebar).Click();

        }

        public void postjob(string company, string title, string jobtype, string requirements, 
                            string description, string responsibilites, string joblink)
        {
            driver.FindElement(newJob).Click();
            driver.FindElement(companyName).SendKeys(company);
            driver.FindElement(jobTitle).SendKeys(title);
            driver.FindElement(jobType).Click();
            if (jobtype == "onsite") {driver.FindElement(onsite).Click(); }
            if (jobtype == "remote") { driver.FindElement(remote).Click(); }
            if (jobtype == "hybrid") { driver.FindElement(hybrid).Click(); }
            if (jobtype == "internship") { driver.FindElement(internship).Click(); }


            // Use JavaScript to fill ReactQuill fields
            FillReactQuillField(jobDesc, description);
            FillReactQuillField(jobReq, requirements);
            FillReactQuillField(jobResp, responsibilites);

            driver.FindElement(jobLink).SendKeys(joblink);
            driver.FindElement(submitBtn).Click();
        }

        private void FillReactQuillField(By locator, string text)
        {
            // Find the ReactQuill editor container
            IWebElement quillEditor = driver.FindElement(locator);

            // ReactQuill's content is inside the div with the class "ql-editor"
            IWebElement editorContent = quillEditor.FindElement(By.CssSelector(".ql-editor"));

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].innerHTML = arguments[1];", editorContent, text);
        }

    }
}

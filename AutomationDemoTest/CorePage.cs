using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationFYPCDP
{
    internal class CorePage
    {
        public static IWebDriver driver;

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
    }
}

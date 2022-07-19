using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFramework.Helper;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace TestFramework.Extensions
{
    public static class DriverExtensions
    {
        //public static IWebElement FindElementIgnoreException(this IWebDriver driver, By by, double waitInSeconds = 20)
        //{
        //    try
        //    {
        //        var recivedElement = FindElement(driver, by, waitInSeconds);
        //        return recivedElement;
        //    }
        //    catch
        //    {
        //        return null;
        //    }

        //}

        //public static IWebElement FindElement(this IWebDriver driver, By by, double waitInSeconds = 20)
        //{
        //    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitInSeconds));
        //    IWebElement result = null;
        //    try
        //    {
        //        if (waitInSeconds > 0)
        //        {
        //            result = wait.Until(drv => drv.FindElement(by));
        //            if (result != null)
        //            {
        //                ElementMapping.ElementSelectors[driver][result] = by;
        //            }
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw new Exception(string.Format($"Element not found for {by}. See Inner Exception:", ex.InnerException));
        //    }

        //    result = driver.FindElement(by);
        //    if (result != null)
        //    {
        //        ElementMapping.ElementSelectors[driver][result] = by;
        //    }
        //    return result;
        //}

        //public static void WaitForElementToBeClickable(this IWebDriver driver, IWebElement element, int waitSeconds = 20)
        //{
        //    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitSeconds));
        //    wait.IgnoreExceptionTypes(
        //        typeof(NoSuchElementException),
        //        typeof(ElementNotVisibleException),
        //        typeof(NoSuchWindowException),
        //        typeof(TimeoutException));

        //    wait.Until(ExpectedConditions.ElementToBeClickable(element));
        //}

        //public static void WaitForElementToBeDisplayed(this IWebDriver driver, IWebElement element, int waitSeconds = 20)
        //{
        //    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitSeconds));
        //    try
        //    {
        //        wait.Until(drv => element != null && element.Displayed == true);
        //    }
        //    catch
        //    {

        //    }

        //}
    }
}

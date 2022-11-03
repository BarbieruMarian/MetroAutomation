using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestFramework.Selenium.Helpers
{
    public class JSProxy
    {
        private readonly IWebDriver WebDriver;
        public readonly WebDriverWait Wait;
        private IJavaScriptExecutor JsExecutor => WebDriver as IJavaScriptExecutor;
        public JSProxy(IWebDriver webDriver, WebDriverWait wait)
        {
            WebDriver = webDriver;
            Wait = wait;
        }

        public object ExecuteScript(string script, params object[] args)
        {
            return JsExecutor.ExecuteScript(script, args);
        }
    }
}

using OpenQA.Selenium;
using TestFramework.Selenium.Interfaces;

namespace TestFramework.Helper
{
    public static class ElementMapping
    {
        public static IDictionary<IDriverProxy, Dictionary<IWebElement, By>> ElementSelectors;

        static ElementMapping()
        {
            ElementSelectors = new Dictionary<IDriverProxy, Dictionary<IWebElement, By>>();
        }

        public static string GetElementSelectorsAsText(IWebElement element)
        {
            string result = "SELECTOR UNAVAILABLE";
            foreach (var drv in ElementSelectors.Keys)
            {
                if (ElementSelectors[drv].ContainsKey(element))
                {
                    result = ElementSelectors[drv][element].ToString();
                    break;
                }
            }
            return result;
        }

        public static By GetElementSelector(IWebElement element)
        {
            By result = null;
            foreach (var drv in ElementSelectors.Keys)
            {
                if (ElementSelectors[drv].ContainsKey(element))
                {
                    result = ElementSelectors[drv][element];
                    break;
                }
            }
            return result;
        }
    }
}

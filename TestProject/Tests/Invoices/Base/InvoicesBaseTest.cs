using TestFramework.Configuration;
using TestFramework.Helper;
using TestFramework.Selenium.Interfaces;
using TestFramework.Test;

namespace TestProject.Tests.Invoices.Base
{
    public class InvoicesBaseTest : BaseTest
    {
        public InvoicesBaseTest(IDriverType browserDriverType) : base(browserDriverType)
        {
        }

        protected override void BeforeEach()
        {
            base.BeforeEach();
            FileManager.DeleteFilesFromFolder(Config.PDFDownloadsFolder);
        }

        protected override void AfterEach()
        {
            base.AfterEach();
        }
    }
}

using TestFramework.Configuration;

namespace TestFramework.Contexts
{
    public class CustomersContext : ICustomersContext
    {
        private readonly ContextConfig _config;

        public CustomersContext() =>
            _config = new ContextConfig();

        public string GetRandomActiveCustomerBarcode() => _config.Get("customers:activeCustomerBarcode");
    }
}

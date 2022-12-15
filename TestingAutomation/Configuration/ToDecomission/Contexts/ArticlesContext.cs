using System.Collections.Generic;
using TestFramework.Configuration.ToDecomission;

namespace TestFramework.Contexts
{
    public class ArticlesContext : IArticlesContext
    {
        private readonly ContextConfig _config;

        public ArticlesContext() =>
            _config = new ContextConfig();

        public Dictionary<string, string> GetRegularItem()
        {
            return new Dictionary<string, string>
            {
                { "name", _config.Get("items:regular:name") },
                { "number", _config.Get("items:regular:number") },
                { "barcode", _config.Get("items:regular:barcode") }
            };
        }
        public string GetRegularItemBarcode() => _config.Get("items:regular:barcode");
        public string GetPromotionItemBarcode() => _config.Get("items:promotion");
        public string GetDiscountItemBarcode() => _config.Get("items:discount");
        public string GetPriceVariableItemBarcode() => _config.Get("items:priceVariable");
        public string GetPriceVariableInBarcodeItemBarcode() => _config.Get("items:priceVariableInBarcode");
        public string GetWeightedItemBarcode() => _config.Get("items:weighted");
        public string GetWeightedInBarcodeItemBarcode() => _config.Get("items:weightedInBarcode");
        public string GetTraceabilityItemBarcode() => _config.Get("items:traceability");
        public string GetTraceabilityInBarcodeItemBarcode() => _config.Get("items:traceabilityInBarcode");
        public string GetDepositItemBarcode() => _config.Get("items:deposit");
        public string GetHamperItemBarcode() => _config.Get("items:hamper");
        public string GetCannotBeSoldItemBarcode() => _config.Get("items:cannotBeSold");
        public string GetInvalidItemBarcode() => _config.Get("items:invalid");
    }
}
